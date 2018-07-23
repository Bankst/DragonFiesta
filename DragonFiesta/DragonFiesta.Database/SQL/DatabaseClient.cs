using System;
using System.Data;
using System.Data.SqlClient;
using DragonFiesta.Utils.Core;

namespace DragonFiesta.Database.SQL
{
    /// <summary>
    /// Represents a client of a database,
    /// </summary>
    public sealed class DatabaseClient : IDisposable
    {
        #region Fields

        private int mId;
        private double mLastActivity;
        public SqlConnection mConnection { get; private set; }
        public SqlCommand mCommand;
        private bool mAvailable;
        private DatabaseManager mManager;

        #endregion Fields

        #region Properties

        public int Id
        {
            get
            {
                return mId;
            }
        }

        public bool Available
        {
            get
            {
                return mAvailable;
            }

            set
            {
                mAvailable = value;
            }
        }

        public double TimeInactive
        {
            get
            {
                return UnixTimestamp.GetCurrent() - mLastActivity;
            }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Constructs a new database client with a given handle to a given database proxy.
        /// </summary>
        /// <param name="Handle">The identifier of this database client as an unsigned 32 bit integer.</param>
        /// <param name="pManager">The instance of the DatabaseManager that manages the database proxy of this database client.</param>
        internal DatabaseClient(int Id, SqlConnection Conn, DatabaseManager pManager)
        {
            mId = Id;
            mConnection = Conn;
            mCommand = mConnection.CreateCommand();
            mManager = pManager;
            mAvailable = true;
            UpdateLastActivity();
        }

        #endregion Constructor

        /// <summary>
        /// Called when released from using() scope - does not actually dispose, just marks as available
        /// </summary>
        public void Dispose()
        {
            ResetCommand();
            mAvailable = true;
            UpdateLastActivity();
	        if (ServerMainDebug.DebugSql) DatabaseLog.Write(DatabaseLogLevel.Debug, "(Sql)Released client " + Id + " for availability.");
        }

        public void Close()
        {
            mConnection.Close();
            mCommand.Dispose();

            mConnection = null;
            mCommand = null;
        }

        private void UpdateLastActivity()
        {
            mLastActivity = UnixTimestamp.GetCurrent();
        }

        public void ResetCommand()
        {
            mCommand.CommandText = null;

            ClearParameters();
        }

        public void ClearParameters()
        {
            mCommand.Parameters.Clear();
        }

        public void SetParameter(string Key, object Value)
        {
            mCommand.Parameters.Add(new SqlParameter(Key, Value));
        }

        public SqlParameter SetParameter(string Key, SqlDbType pType, ParameterDirection direction)
        {
            var idParam = mCommand.Parameters.Add(new SqlParameter(Key, pType)
            {
                Direction = direction,
            });
            return idParam;
        }

        public void CreateStoredProcedure(string CommandText)
        {
            SqlCommand cmd = mConnection.CreateCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = CommandText;
            mCommand = cmd;
        }

        public int ExecuteNonQuery()
        {
            mCommand.Connection = mConnection;

            int Effects = mCommand.ExecuteNonQuery();

            ResetCommand();

            return Effects;
        }

        public object ExecuteScalar()
        {
            mCommand.Connection = mConnection;
            object result = mCommand.ExecuteScalar();
            ResetCommand();

            return result;
        }

        public SqlDataReader ExecuteReader(SqlCommand cmd)
        {
            cmd.Connection = mConnection;
            return cmd.ExecuteReader();
        }

        public bool CheckConnection()
        {
            if (mConnection.State == ConnectionState.Open)
            {
                return true;
            }

            return false;
        }
    }
}