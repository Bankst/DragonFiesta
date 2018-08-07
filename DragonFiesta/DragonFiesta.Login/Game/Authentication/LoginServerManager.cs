using System.Collections.Concurrent;
using DragonFiesta.Database.SQL;

namespace DragonFiesta.Login.Game.Authentication
{
    [ServerModule(ServerType.Login, InitializationStage.PreData)]
    public class LoginServerManager
    {
        private static ConcurrentDictionary<ClientRegion, LoginServer> LoginServers = new ConcurrentDictionary<ClientRegion, LoginServer>();

        [InitializerMethod]
        public static bool OnStart()
        {
            LoadRegionServers();
            return true;
        }

        private static void LoadRegionServers()
        {
            LoginServers = new ConcurrentDictionary<ClientRegion, LoginServer>();

            SQLResult pResult = DB.Select(DatabaseType.Login, "SELECT * FROM RegionServers");

            DatabaseLog.WriteProgressBar(">> Load RegionServers");

            using (ProgressBar mBar = new ProgressBar((pResult.Count)))
            {
                for (int i = 0; i < pResult.Count; i++)
                {
                    LoginServer mServer = new LoginServer(pResult, i);

                    mBar.Step();

                    if (!LoginServers.TryAdd(mServer.Region, mServer))
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate RegionServer Found region {0} ", mServer.Region);
                }

                DatabaseLog.WriteProgressBar(">> Loaded {0} RegionServers", LoginServers.Count);
            }
        }

        public static bool StartListening()
        {
            foreach (var mServer in LoginServers.Values)
            {
                if (!mServer.Start())
                    return false;
            }

            return true;
        }

        public static void StopListening()
        {
            foreach (var mServer in LoginServers.Values)
            {
                mServer.Stop();
            }
        }
    }
}