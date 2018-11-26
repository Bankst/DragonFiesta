using System;
using System.Collections.Generic;
using System.Text;
using DFEngine.Config;
using DFEngine.Logging;

namespace DFEngine.Database
{
	public class DB
	{
		private static readonly Dictionary<DatabaseType, DatabaseManager> DbManagerList = new Dictionary<DatabaseType, DatabaseManager>();

		public static bool AddManager(DatabaseType pType, DatabaseConfiguration dbConfig)
		{
			string dbName;
			switch (pType)
			{
				case DatabaseType.Account:
					dbName = dbConfig.AccountDatabase;
					break;
				case DatabaseType.AccountLog:
					dbName = dbConfig.AccountLogDatabase;
					break;
				case DatabaseType.Character:
					dbName = dbConfig.CharacterDatabase;
					break;
				case DatabaseType.GameLog:
					dbName = dbConfig.GameLogDatabase;
					break;
				default:
					return false;
			}

			var mManager = new DatabaseManager(GenerateDatabaseServer(dbConfig), GenerateDatabase(dbConfig, dbName));

			if (mManager.TestConnection(out var exceptionMsg))
			{
				DbManagerList.Add(pType, mManager);
				DatabaseLog.Write(DatabaseLogLevel.Startup, $"Successfully connected to {dbName} Database!");
				return true;
			}

			DatabaseLog.Write(DatabaseLogLevel.DatabaseClientError, $"Failed to connect {dbName} Database: {exceptionMsg}");
			return false;
		}

		public static void DisposeManager(DatabaseType pType)
		{
			if (DbManagerList.TryGetValue(pType, out var pManager))
			{
				pManager.Dispose();
			}
			else
			{
				DatabaseLog.Write(DatabaseLogLevel.DatabaseClientError, "Failed to Dispose {0} DB Monitor, Unknown Manager", pType);
			}
		}

		private static DatabaseServer GenerateDatabaseServer(DatabaseConfiguration dbConf) => new DatabaseServer(dbConf.Host, dbConf.Username, dbConf.Password);

		private static Database GenerateDatabase(DatabaseConfiguration dbConf, string dbName) => new Database(dbName, dbConf.MinPoolSize, dbConf.MaxPoolSize, dbConf.ClientLifeTime);

		public static DatabaseClient GetDatabaseClient(DatabaseType pType)
		{
			return DbManagerList.TryGetValue(pType, out var pManager) ? pManager.GetClient() : null;
		}

		public static void RunSQL(DatabaseType pType, string sql, params object[] parameter)
		{
			if (DbManagerList.TryGetValue(pType, out var pManager))
			{
				pManager.RunSQL(pType, sql, parameter);
				return;
			}

			DatabaseLog.Write(DatabaseLogLevel.DatabaseClientError, "Unknown Manager {0}", pType);
		}

		public static SQLResult Select(DatabaseType pType, string sql, params object[] parameters)
		{
			if (DbManagerList.TryGetValue(pType, out var pManager))
			{
				return pManager.Select(sql, parameters);
			}

			DatabaseLog.Write(DatabaseLogLevel.DatabaseClientError, "Unknown Manager {0}", pType);
			return null;
		}
	}
}
