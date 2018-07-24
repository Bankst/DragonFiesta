using System.Collections.Generic;
using System.Data.Entity;
using DragonFiesta.Utils.Config.Section;

namespace DragonFiesta.Database.SQL
{
	public class DB
	{
		private static readonly Dictionary<DatabaseType, DatabaseManager> DbManagerList = new Dictionary<DatabaseType, DatabaseManager>();
		
		public static bool AddManager(DatabaseType pType, DatabaseSection dbSection)
		{
			//Need Catch Ex ?

			var mManager = new DatabaseManager(GenerateDatabaseServer(dbSection), GenerateDatabase(dbSection));

			if (mManager.TestConnection())
			{
				DbManagerList.Add(pType, mManager);
				DatabaseLog.Write(DatabaseLogLevel.Startup, "Test {0} Database Connection Settings Success!", pType);

				return true;
			}

			DatabaseLog.Write(DatabaseLogLevel.DatabaseClientError, "Failed To Connect Please Check your {0} Database Settings", pType);

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

		private static DatabaseServer GenerateDatabaseServer(DatabaseSection dbConf) => new DatabaseServer(dbConf.SQLHost, dbConf.SQLUser, dbConf.SQLPassword);

		private static Database GenerateDatabase(DatabaseSection dbConf) => new Database(dbConf.SQLName, dbConf.MinPoolSize, dbConf.MaxPoolSize, dbConf.DatabaseClientLifeTime);

		public static bool AddDBMonitor(DatabaseType pType)
		{
			if (DbManagerList.TryGetValue(pType, out var pManager))
			{
				return ServerTaskManager.AddObject(pManager);
			}

			DatabaseLog.Write(DatabaseLogLevel.DatabaseClientError, "Failed to Add DB Monitor, Unknown Manager {0}", pType);
			return false;
		}

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