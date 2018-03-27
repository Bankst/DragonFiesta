using DragonFiesta.Messages.Message.Auth;
using DragonFiesta.World.Config;

namespace DragonFiesta.World.InternNetwork.InternHandler.Response.Login
{
    public static class Auth_Response
    {
        public static void HandleAuthtecicateWorld_Response(IMessage msg)
        {
            var Response = (msg as AuthenticatetWorld_Response);

            switch (Response.Result)
            {
                case InternWorldAuthResult.AlredyRegister:
                    EngineLog.Write(EngineLogLevel.Exception, "World With ID {0} Alredy Resgitriert", WorldConfiguration.Instance.WorldID);
                    break;

                case InternWorldAuthResult.Error:
                    EngineLog.Write(EngineLogLevel.Exception, "Unkown Error to Auth Login");
                    break;

                case InternWorldAuthResult.InvalidPassword:
                    EngineLog.Write(EngineLogLevel.Exception, "Invalid Auth Password...");
                    break;

                case InternWorldAuthResult.InvalidWorldID:
                    EngineLog.Write(EngineLogLevel.Exception, "Invalid Auth WorldId..");
                    break;

                case InternWorldAuthResult.OK:
                    WorldConfiguration.Instance.ServerRegion = Response.Region;

                    SocketLog.Write(SocketLogLevel.Startup, "Authenticatet OK");

                    if (!ServerMain.InternalInstance.IsDataLoaded)
                    {
                        if (!ServerMain.LoadGameServer())
                            throw new StartupException("Invalid Load GameServer Modules...");

                        ServerMain.InternalInstance.IsDataLoaded = true;
                    }

                    ServerMain.InternalInstance.ServerIsReady = true;

                    EngineLog.Write(EngineLogLevel.Info, "WorldServer Start Success!!");
                    break;

                default:
                    break;
            }

            if (Response.Result != InternWorldAuthResult.OK)
                ServerMain.InternalInstance.Shutdown();
            else
                ServerMain.InternalInstance.Title.Update();
        }

        //When Auth Sending TimeOutet then again...
        public static void AuthLoginTimeout(IMessage msg) => InternLoginConnector.Instance.Dispose();
    }
}