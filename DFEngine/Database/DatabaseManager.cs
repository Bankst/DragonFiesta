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
		private readonly Dictionary<int, DatabaseClient> _mClients = new Dictionary<int, DatabaseClient>();
		private int _mStarvationCounter;

		private int _mClientIdGenerator;
		private readonly object _mSyncRoot;

		private DatabaseServer _mServer;
		private Database _mDatabase;
		private int _isDisposedInt;

		GameTime IServerTask.LastUpdate { get; set; }

		public ServerTaskTimes Interval { get; private set; }

		public void Dispose()
		{
			if (Interlocked.CompareExchange(ref _isDisposedInt, 1, 0) != 0) return;
			_mServer = null;
			_mDatabase = null;
		}

		public int ClientCount => _mClients.Count;


		public DatabaseManager()
		{
		}

		internal DatabaseManager(DatabaseServer pServer, Database pDatabase)
		{
			_mServer = pServer;
			_mDatabase = pDatabase;

			_mSyncRoot = new object();
			Interval = (ServerTaskTimes)(_mDatabase.ClientLifeTime * 1000 / 2);
		}

		#region Util Function

		public string BuildConnectionString()
		{
			var cb = new SqlConnectionStringBuilder()
			{
				DataSource = _mServer.Host,
				UserID = _mServer.User,
				Password = _mServer.Password,
				InitialCatalog = _mDatabase.Name,
				MultipleActiveResultSets = true,
				IntegratedSecurity = false,
				MinPoolSize = _mDatabase.MinPoolSize,
				MaxPoolSize = _mDatabase.MaxPoolSize,
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
			Monitor.PulseAll(_mSyncRoot);
		}

		private int GenerateClientId()
		{
			lock (_mSyncRoot)
			{
				return _mClientIdGenerator++;
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

			if (ClientCount <= _mDatabase.MinPoolSize) return true;
			lock (_mSyncRoot)
			{
				var toDisconnect = new List<int>();

				foreach (var client in _mClients.Values)
				{
					if (client.Available && client.TimeInactive >= _mDatabase.ClientLifeTime)
					{
						toDisconnect.Add(client.Id);
					}
				}

				foreach (var disconnectId in toDisconnect)
				{
					_mClients[disconnectId].Close();
					_mClients.Remove(disconnectId);
				}

				if (toDisconnect.Count > 0)
				{
					DatabaseLog.Write(DatabaseLogLevel.Debug, $"(Sql)Disconnected {toDisconnect.Count} inactive client(s).");
				}
				Monitor.PulseAll(_mSyncRoot);
			}
			return true;
		}

		public void SetClientAmount(int clientAmount, string logReason = "Unknown")
		{
			int diff;

			lock (_mSyncRoot)
			{
				diff = clientAmount - ClientCount;

				if (diff > 0)
				{
					for (var i = 0; i < diff; i++)
					{
						var newId = GenerateClientId();
						_mClients.Add(newId, CreateClient(newId));
					}
				}
				else
				{
					var toDestroy = -diff;
					var destroyed = 0;

					foreach (var client in _mClients.Values)
					{
						if (!client.Available)
						{
							continue;
						}

						if (destroyed >= toDestroy || ClientCount <= _mDatabase.MinPoolSize)
						{
							break;
						}

						client.Close();
						_mClients.Remove(client.Id);
						destroyed++;
					}
				}
			}

			if (ServerMainDebug.DebugSql) DatabaseLog.Write(DatabaseLogLevel.Debug,
				$"(Sql) Client availability: {clientAmount}; modifier: {diff}; reason: {logReason}.");
		}

		public DatabaseClient GetClient()
		{
			lock (_mSyncRoot)
			{
				foreach (var client in _mClients.Values)
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

				if (_mDatabase.MaxPoolSize <= 0 || ClientCount < _mDatabase.MaxPoolSize) // Max pool size ignored if set to 0 or lower
				{
					SetClientAmount(ClientCount + 1, "out of assignable clients in GetClient()");
					return GetClient();
				}

				_mStarvationCounter++;

				DatabaseLog.Write(DatabaseLogLevel.Warning,
					$"(Sql) Client starvation; out of assignable clients/maximum pool size reached. Consider increasing the `mysql.pool.max` configuration value. Starvation count is {_mStarvationCounter}.");

				// Wait until an available client returns
				Monitor.Wait(_mSyncRoot);
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
					mClient.mCommand = sqlCommand;

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
