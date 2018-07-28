using DragonFiesta.Database.Models;
using DragonFiesta.Database.SQL;
using DragonFiesta.Networking.Network.Session;
using DragonFiesta.Utils.Core;
using DragonFiesta.Utils.Utils;
using DragonFiesta.World.Config;
using DragonFiesta.World.InternNetwork;
using DragonFiesta.World.Network;
using DragonFiesta.World.ServerConsole.Title;

namespace DragonFiesta.World
{
    public class ServerMain : ServerMainBase
    {
        public new static ServerMain InternalInstance { get; private set; }

        public bool IsDataLoaded = false;

        public WorldConsoleTitle Title { get; set; }

        public ServerMain() : base(ServerType.World)
        {
            Title = new WorldConsoleTitle();
            Title.Update();
        }

        public override void Shutdown()
        {
            InternLoginConnector.Instance.Dispose();
            InternZoneServer.Instance.Stop();
            WorldServer.Instance.Stop();
            base.Shutdown();

            //SendEnter();//Exit readkey from console...
        }

	    public static bool LoadGameServer() => InternalInstance.LoadGameServerModules();
		
		public static bool Initialize()
        {
            InternalInstance = new ServerMain();
            InternalInstance.WriteConsoleLogo();
            //System.Threading.Thread.Sleep(2500); // TODO: Find better way to delay start based on Login Server status. Maybe 250ms pings?

            if (!WorldConfiguration.Initialize())
            {
                throw new StartupException("Failed to load WorldConfiguration");
            }

	        EngineLog.Write(EngineLogLevel.Startup, "Checking if Login is online");
	        if (!PortChecker.IsPortOpen(
				WorldConfiguration.Instance.ConnectToInfo.ConnectIP,
				WorldConfiguration.Instance.ConnectToInfo.ConnectPort, 250))
	        {
		        EngineLog.Write(EngineLogLevel.Startup, "Login offline, waiting...");
		        PortChecker.WaitForPort(
			        WorldConfiguration.Instance.ConnectToInfo.ConnectIP,
			        WorldConfiguration.Instance.ConnectToInfo.ConnectPort);
		        EngineLog.Write(EngineLogLevel.Startup, "World online, starting");
			} else EngineLog.Write(EngineLogLevel.Startup, "Login online, starting");


			ThreadPool.Start(WorldConfiguration.Instance.WorkTaskThreadCount);

            ThreadPool.AddUpdateAbleServer(ServerTaskManager.InitialInstance());

	        if (!EDM.TestWorldEntity(WorldConfiguration.Instance.WorldDatabaseSettings))
	        {
				throw new StartupException("Failed to load World EntityModel");
	        }

            if (!DB.AddManager(DatabaseType.World, WorldConfiguration.Instance.WorldDatabaseSettings))
            {
                throw new StartupException("Failed to add World DatabaseManager");
            }

            if (!DB.AddManager(DatabaseType.Data, WorldConfiguration.Instance.DataDatabaseSettings))
            {
                throw new StartupException("Failed to add Data DatabaseManager");
            }

            if (!InternalInstance.LoadBaseServerModule())
            {
                throw new StartupException("Failed to load Server");
            }

            if (!DB.AddDBMonitor(DatabaseType.Data) || !DB.AddDBMonitor(DatabaseType.World))
            {
                throw new StartupException("Failed to add Database Monitor");
            }

			return true;
        }
    }
}