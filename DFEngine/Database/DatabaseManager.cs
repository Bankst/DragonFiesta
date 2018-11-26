using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Threading;
using DFEngine.Logging;
using DFEngine.Server;
using DFEngine.Threading;
using DFEngine.Utils;

namespace DFEngine.Database
{
	public class DatabaseManager : IServerTask, IDisposable
	{
		private readonly Dictionary<int, DatabaseClient> _clients = new Dictionary<int, DatabaseClient>();
		private int _starvationCounter;

		private int _clientIdGenerator;
		private readonly object _syncRoot;

		private DatabaseServer _server;
		private Database _database;
		private int _isDisposedInt;

		GameTime IServerTask.LastUpdate { get; set; }

		public ServerTaskTimes Interval { get; private set; }

		public void Dispose()
		{
			if (Interlocked.CompareExchange(ref _isDisposedInt, 1, 0) != 0) return;
			_server = null;
			_database = null;
		}

		public int ClientCount => _clients.Count;


		public DatabaseManager()
		{
		}

		internal DatabaseManager(DatabaseServer pServer, Database pDatabase)
		{
			_server = pServer;
			_database = pDatabase;

			_syncRoot = new object();
			Interval = (ServerTaskTimes)(_database.ClientLifeTime * 1000 / 2);
		}

		#region Util Function

		public string BuildConnectionString()
		{
			var cb = new SqlConnectionStringBuilder()
			{
				DataSource = _server.Host,
				UserID = _server.User,
				Password = _server.Password,
				InitialCatalog = _database.Name,
				MultipleActiveResultSets = true,
				IntegratedSecurity = false,
				MinPoolSize = _database.MinPoolSize,
				MaxPoolSize = _database.MaxPoolSize,
			}.ToString();

			return cb;
		}

		public bool TestConnection(out string exceptionMsg)
		{
			exceptionMsg = "";
			try
			{
				using (var connection = new SqlConnection(BuildConnectionString()))
				{
					connection.Open();
					return true;
				}
			}
			catch (SqlException ex)
			{
				switch ((SQLError)ex.Number)
				{
					case SQLError.BadPassword:
						exceptionMsg = "Bad Username or Password!";
						break;
					default:
						exceptionMsg = ex.StackTrace;
						break;
				}
				return false;
			}
		}

		public void PokeAllAwaiting()
		{
			Monitor.PulseAll(_syncRoot);
		}

		private int GenerateClientId()
		{
			lock (_syncRoot)
			{
				return _clientIdGenerator++;
			}
		}

		private DatabaseClient CreateClient(int id)
		{
			var connection = new SqlConnection(BuildConnectionString());
			connection.Open();

			return new DatabaseClient(id, connection, this);
		}

		bool IServerTask.Update(GameTime gameTime)
		{
			if (_isDisposedInt == 1) return false;

			if (ClientCount <= _database.MinPoolSize) return true;
			lock (_syncRoot)
			{
				var toDisconnect = new List<int>();

				foreach (var client in _clients.Values)
				{
					if (client.Available && client.TimeInactive >= _database.ClientLifeTime)
					{
						toDisconnect.Add(client.Id);
					}
				}

				foreach (var disconnectId in toDisconnect)
				{
					_clients[disconnectId].Close();
					_clients.Remove(disconnectId);
				}

				if (toDisconnect.Count > 0)
				{
					DatabaseLog.Write(DatabaseLogLevel.Debug, $"(Sql)Disconnected {toDisconnect.Count} inactive client(s).");
				}
				Monitor.PulseAll(_syncRoot);
			}
			return true;
		}

		public void SetClientAmount(int clientAmount, string logReason = "Unknown")
		{
			int diff;

			lock (_syncRoot)
			{
				diff = clientAmount - ClientCount;

				if (diff > 0)
				{
					for (var i = 0; i < diff; i++)
					{
						var newId = GenerateClientId();
						_clients.Add(newId, CreateClient(newId));
					}
				}
				else
				{
					var toDestroy = -diff;
					var destroyed = 0;

					foreach (var client in _clients.Values)
					{
						if (!client.Available)
						{
							continue;
						}

						if (destroyed >= toDestroy || ClientCount <= _database.MinPoolSize)
						{
							break;
						}

						client.Close();
						_clients.Remove(client.Id);
						destroyed++;
					}
				}
			}

			if (ServerMainDebug.DebugSql) DatabaseLog.Write(DatabaseLogLevel.Debug,
				$"(Sql) Client availability: {clientAmount}; modifier: {diff}; reason: {logReason}.");
		}

		public DatabaseClient GetClient()
		{
			lock (_syncRoot)
			{
				foreach (var client in _clients.Values)
				{
					if (!client.Available)
					{
						continue;
					}


					if (ServerMainDebug.DebugSql) DatabaseLog.Write(DatabaseLogLevel.Debug,
						$"(Sql) Assigned client {client.Id}.");

					if (!client.CheckConnection())
					{
						client.Dispose();
						return GetClient();
					}

					client.Available = false;
					return client;
				}

				if (_database.MaxPoolSize <= 0 || ClientCount < _database.MaxPoolSize) // Max pool size ignored if set to 0 or lower
				{
					SetClientAmount(ClientCount + 1, "out of assignable clients in GetClient()");
					return GetClient();
				}

				_starvationCounter++;

				DatabaseLog.Write(DatabaseLogLevel.Warning,
					$"(Sql) Client starvation; out of assignable clients/maximum pool size reached. Consider increasing the `mysql.pool.max` configuration value. Starvation count is {_starvationCounter}.");

				// Wait until an available client returns
				Monitor.Wait(_syncRoot);
				return GetClient();
			}
		}

		public int RunSQL(DatabaseType type, string sql, params object[] parameters)
		{
			using (var mClient = DB.GetDatabaseClient(type))
			{
				var sqlString = new StringBuilder();
				// Fix for floating point problems on some languages
				sqlString.AppendFormat(CultureInfo.GetCultureInfo("en-US").NumberFormat, sql, parameters);

				SqlCommand sqlCommand = null;
				try
				{
					sqlCommand = new SqlCommand(sqlString.ToString());
					sqlCommand.Parameters.AddRange(parameters);
					mClient.Command = sqlCommand;

					return mClient.ExecuteNonQuery();
				}
				catch (SqlException ex)
				{
					EngineLog.Write(ex, $"Failed to Execute Query {sqlCommand?.CommandText}");
					return 0;
				}
			}
		}

		public SQLResult Select(string sql, params object[] parameters)
		{
			using (var pClient = GetClient())
			{
				var sqlString = new StringBuilder();
				// Fix for floating point problems on some languages
				sqlString.AppendFormat(CultureInfo.GetCultureInfo("en-US").NumberFormat, sql, parameters);

				var sqlCommand = new SqlCommand(sqlString.ToString());

				try
				{
					sqlCommand.Parameters.AddRange(parameters);

					using (var sqlData = pClient.ExecuteReader(sqlCommand))
					{
						using (var retData = new SQLResult())
						{
							retData.Load(sqlData);
							retData.Count = retData.Rows.Count;

							return retData;
						}
					}
				}
				catch (SqlException ex)
				{
					DatabaseLog.Write(ex, $"Error With Query {sqlCommand.CommandText}");
					return null;
				}
			}
		}

		#endregion Util Function
	}
}
