using DragonFiesta.Utils.Core;
using DragonFiesta.World.Config;
using DragonFiesta.World.InternNetwork;
using DragonFiesta.World.Network;
using DragonFiesta.World.ServerConsole.Title;

namespace DragonFiesta.World
{
    public class ServerMain : ServerMainBase
    {
        public static new ServerMain InternalInstance { get; private set; }

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

        public override bool LoadBaseServerModule()
        {
            //Here bevor starts Provider
            return base.LoadBaseServerModule();
        }

        public static bool LoadGameServer()
        {
            if (InternalInstance.LoadGameServerModules())
            {
                //Here All Modules After Register
                //GameNetwork ever Last
                return true;
            }
            return true;
        }

        public static bool Initialize()
        {
            InternalInstance = new ServerMain();
            InternalInstance.WriteConsoleLogo();
            System.Threading.Thread.Sleep(5000);
            if (!WorldConfiguration.Initialize())
            {
                throw new StartupException("Invalid Load WorldConfiguration");
            }

            ThreadPool.Start(WorldConfiguration.Instance.WorkTaskThreadCount);

            ThreadPool.AddUpdateAbleServer(ServerTaskManager.InitialInstance());

            if (!DB.AddManager(DatabaseType.World, WorldConfiguration.Instance.WorldDatabaseSettings))
            {
                throw new StartupException("Invalid Add World DatabaseManager");
            }
            if (!DB.AddManager(DatabaseType.Data, WorldConfiguration.Instance.DataDatabaseSettings))
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