using DragonFiesta.Utils.Core;
using DragonFiesta.Zone.Config;
using DragonFiesta.Zone.Network;
using DragonFiesta.Zone.ServerConsole.Title;
using System;
using DragonFiesta.Database.Models;
using DragonFiesta.Database.SQL;
using DragonFiesta.Utils.Utils;

namespace DragonFiesta.Zone.Core
{
    public class ServerMain : ServerMainBase
    {
        public new static ServerMain InternalInstance { get; private set; }

        public ZoneConsoleTitle Title { get; set; }

        public bool IsDataLoaded = false;

        public ServerMain() : base(ServerType.Zone)
        {
            Title = new ZoneConsoleTitle();
            Title.Update();
        }

        public override bool LoadBaseServerModule()
        {

            return base.LoadBaseServerModule();
        }

        public override void Shutdown()
        {
            base.Shutdown();

            InternWorldConnector.Instance.Dispose();
            ZoneServer.Shutdown();
           
            DB.DisposeManager(DatabaseType.World);
            DB.DisposeManager(DatabaseType.Data);

            Environment.Exit(0);
          
        }
        public static bool LoadGameServer() => InternalInstance.LoadGameServerModules();


	    public static bool Initialize(byte zoneId = 0)
	    {
		    InternalInstance = new ServerMain();
		    InternalInstance.WriteConsoleLogo();

			if (!ZoneConfiguration.Initialize(zoneId))
            {
                throw new StartupException("Invalid Load ZoneConfiguration");
            }

			EngineLog.Write(EngineLogLevel.Startup, "Checking if World is online");
			if (!PortChecker.IsPortOpen(
			    ZoneConfiguration.Instance.WorldConnectInfo.ConnectIP,
			    ZoneConfiguration.Instance.WorldConnectInfo.ConnectPort, 250))
		    {
			    EngineLog.Write(EngineLogLevel.Startup, "World offline, waiting...");
			    PortChecker.WaitForPort(
				    ZoneConfiguration.Instance.WorldConnectInfo.ConnectIP,
				    ZoneConfiguration.Instance.WorldConnectInfo.ConnectPort);
			    EngineLog.Write(EngineLogLevel.Startup, "World online, starting");
			} else EngineLog.Write(EngineLogLevel.Startup, "World online, starting");

			ThreadPool.Start(ZoneConfiguration.Instance.WorkThreadCount);

            ThreadPool.AddUpdateAbleServer(ServerTaskManager.InitialInstance());

		    if (!EDM.TestWorldEntity(ZoneConfiguration.Instance.WorldDatabaseSettings))
		    {
			    throw new StartupException("Failed to load World EntityModel");
		    }

			if (!DB.AddManager(DatabaseType.World, ZoneConfiguration.Instance.WorldDatabaseSettings))
            {
                throw new StartupException("Invalid Add World DatabaseManager");
            }
            if (!DB.AddManager(DatabaseType.Data, ZoneConfiguration.Instance.DataDatabaseSettings))
            {
                throw new StartupException("Invalid Add Data DatabaseManager");
            }

            if (!InternalInstance.LoadBaseServerModule())
            {
                throw new StartupException("Invalid Load Server");
            }

            if (!DB.AddDBMonitor(DatabaseType.Data) || !DB.AddDBMonitor(DatabaseType.World))
            {
                throw new StartupException("Invalid Add Database Monitor");
            }

            return true;
        }
    }
}