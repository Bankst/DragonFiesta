using System;
using System.Data;
using System.Data.SqlClient;

using DFEngine.Logging;
using DFEngine.Server;

namespace DFEngine.Database
{
	/// <summary>
	/// Represents a client of a database,
	/// </summary>
	public sealed class DatabaseClient : IDisposable
	{
		#region Fields

		private double _lastActivity;
		public SqlConnection Connection { get; private set; }
		public SqlCommand Command;
		private DatabaseManager _manager;

		#endregion Fields

		#region Properties

		public int Id { get; }

		public bool Available { get; set; }

		public double TimeInactive => UnixTimestamp.GetCurrent() - _lastActivity;

		#endregion Properties

		#region Constructor

		/// <summary>
		/// Constructs a new database client with a given handle to a given database proxy.
		/// </summary>
		/// <param name="id">The identifier of this database client as an unsigned 32 bit integer.</param>
		/// <param name="conn">The <see cref="SqlConnection"/> to use</param>
		/// <param name="manager">The instance of the DatabaseManager that manages the database proxy of this database client.</param>
		internal DatabaseClient(int id, SqlConnection conn, DatabaseManager manager)
		{
			this.Id = id;
			Connection = conn;
			Command = Connection.CreateCommand();
			_manager = manager;
			Available = true;
			UpdateLastActivity();
		}

		#endregion Constructor

		/// <summary>
		/// Called when released from using() scope - does not actually dispose, just marks as available
		/// </summary>
		public void Dispose()
		{
			ResetCommand();
			Available = true;
			UpdateLastActivity();
			if (ServerMainDebug.DebugSql) DatabaseLog.Write(DatabaseLogLevel.Debug, "(Sql)Released client " + Id + " for availability.");
		}

		public void Close()
		{
			Connection.Close();
			Command.Dispose();

			Connection = null;
			Command = null;
		}

		private void UpdateLastActivity()
		{
			_lastActivity = UnixTimestamp.GetCurrent();
		}

		public void ResetCommand()
		{
			Command.CommandText = null;

			ClearParameters();
		}

		public void ClearParameters()
		{
			Command.Parameters.Clear();
		}

		public void SetParameter(string Key, object Value)
		{
			Command.Parameters.Add(new SqlParameter(Key, Value));
		}

		public SqlParameter SetParameter(string Key, SqlDbType pType, ParameterDirection direction)
		{
			var idParam = Command.Parameters.Add(new SqlParameter(Key, pType)
			{
				Direction = direction,
			});
			return idParam;
		}

		public void CreateStoredProcedure(string CommandText)
		{
			SqlCommand cmd = Connection.CreateCommand();

			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandText = CommandText;
			Command = cmd;
		}

		public int ExecuteNonQuery()
		{
			Command.Connection = Connection;

			int Effects = Command.ExecuteNonQuery();

			ResetCommand();

			return Effects;
		}

		public object ExecuteScalar()
		{
			Command.Connection = Connection;
			object result = Command.ExecuteScalar();
			ResetCommand();

			return result;
		}

		public SqlDataReader ExecuteReader(SqlCommand cmd)
		{
			cmd.Connection = Connection;
			return cmd.ExecuteReader();
		}

		public bool CheckConnection()
		{
			return Connection.State == ConnectionState.Open;
		}
	}
}
