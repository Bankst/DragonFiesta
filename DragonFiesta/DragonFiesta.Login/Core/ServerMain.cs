using DragonFiesta.Database.SQL;
using DragonFiesta.Login.Config;
using DragonFiesta.Login.Game.Authentication;
using DragonFiesta.Login.ServerConsole.Title;
using DragonFiesta.Utils.Core;

namespace DragonFiesta.Login.Core
{
    public class ServerMain : ServerMainBase
    {
        public static new ServerMain InternalInstance { get; private set; }

        public LoginConsoleTitle Title { get; set; }

        public ServerMain() : base(ServerType.Login)
        {
            Title = new LoginConsoleTitle();
            Title.Update();
        }

        public override bool LoadBaseServerModule()
        {
            //For Handler Initials
            return base.LoadBaseServerModule();
        }

        public override void Shutdown()
        {
            LoginServerManager.StopListening();
      
            //Network
            base.Shutdown();

            //InternLoginListener.Shutdown();
            DB.DisposeManager(DatabaseType.Login);

            //SendEnter();//Exit readkey from console...
        }

        public static void Initialize()
        {
            InternalInstance = new ServerMain();
            InternalInstance.WriteConsoleLogo();
            if (!LoginConfiguration.Initialize())
            {
                throw new StartupException("Invalid Load LoginConfiguration");
            }

            if (!DB.AddManager(DatabaseType.Login, LoginConfiguration.Instance.LoginDatabaseSettings))
            {
                throw new StartupException("Invalid Add DatabaseManager");
            }

            ThreadPool.Start(LoginConfiguration.Instance.ThreadTaskPool);

            ThreadPool.AddUpdateAbleServer(ServerTaskManager.InitialInstance());
            
            if (!DB.AddDBMonitor(DatabaseType.Login))
            {
                throw new StartupException("Invalid Add Database Monitor");
            }
            if (!InternalInstance.LoadBaseServerModule())
            {
                throw new StartupException("Invalid Load Server");
            }
            if (!InternalInstance.LoadGameServerModules())
            {
                throw new StartupException("Invalid Load Server");
            }

            //GameNetwork always Last
            if (!LoginServerManager.StartListening())
            {
                throw new StartupException("Invalid Load Game Network");
            }
            InternalInstance.Title.Update();
            InternalInstance.ServerIsReady = true;
        }
    }
}