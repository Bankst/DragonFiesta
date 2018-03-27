using DragonFiesta.Utils.Config.Section;
using DragonFiesta.Utils.Database;
using System.Collections.Generic;
using System.Data.SqlClient;

public class DB
{
    private static readonly Dictionary<DatabaseType, DatabaseManager> DBManagerList = new Dictionary<DatabaseType, DatabaseManager>();

    public static bool AddManager(DatabaseType pType, DatabaseSection DBSection)
    {
        //Need Catch Ex ?

        DatabaseManager mManger = new DatabaseManager(GenerateDatabaseServer(DBSection), GenerateDatabase(DBSection));

        if (mManger.TestConnection())
        {
            DBManagerList.Add(pType, mManger);
            DatabaseLog.Write(DatabaseLogLevel.Startup, "Test  {0} Database Connection Settings Success!", pType);

            return true;
        }
        else
        {
            DatabaseLog.Write(DatabaseLogLevel.DatabaseClientError, "Failed To Connect Please Check you {0} Database Settings", pType);

            return false;
        }
    }

    public static void DisposeManager(DatabaseType pType)
    {
        if (DBManagerList.TryGetValue(pType, out DatabaseManager pManager))
        {
            pManager.Dispose();
        }
        else
        {
            DatabaseLog.Write(DatabaseLogLevel.DatabaseClientError, "failed to Dispose {0} DB Monitor Unkown Manager", pType);
        }
    }

    private static DatabaseServer GenerateDatabaseServer(DatabaseSection DBConf)
    {
        return new DatabaseServer(DBConf.SQLHost, DBConf.SQLUser, DBConf.SQLPassword);
    }

    private static Database GenerateDatabase(DatabaseSection DBConf)
    {
        return new Database(DBConf.SQLName, DBConf.MinPoolSize, DBConf.MaxPoolSize, DBConf.DatabaseClientLifeTime);
    }

    public static bool AddDBMonitor(DatabaseType pType)
    {
        if (DBManagerList.TryGetValue(pType, out DatabaseManager pManager))
        {
            return ServerTaskManager.AddObject(pManager);
        }
        else
        {
            DatabaseLog.Write(DatabaseLogLevel.DatabaseClientError, "failed to Add DB Monitor Unkown Manager {0}", pType);
            return false;
        }
    }

    public static DatabaseClient GetDatabaseClient(DatabaseType pType)
    {
        if (DBManagerList.TryGetValue(pType, out DatabaseManager pManager))
        {
            return pManager.GetClient();
        }

        return null;
    }

    public static void RunSQL(DatabaseType pType, string sql, params SqlParameter[] parameter)
    {
        if (DBManagerList.TryGetValue(pType, out DatabaseManager pManager))
        {
            pManager.RunSQL(pType, sql, parameter);
        }
        else
        {
            DatabaseLog.Write(DatabaseLogLevel.DatabaseClientError, "Unkown Manager {0}", pType);
        }
    }

    public static SQLResult Select(DatabaseType pType, string sql, params SqlParameter[] Parameters)
    {
        if (DBManagerList.TryGetValue(pType, out DatabaseManager pManager))
        {
            return pManager.Select(sql, Parameters);
        }
        else
        {
            DatabaseLog.Write(DatabaseLogLevel.DatabaseClientError, "Unkown Manager {0}", pType);
            return null;
        }
    }
}