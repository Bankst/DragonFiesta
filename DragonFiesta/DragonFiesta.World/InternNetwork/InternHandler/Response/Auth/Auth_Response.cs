using DragonFiesta.Messages.Message.Auth;
using DragonFiesta.Networking.Network.Session;
using DragonFiesta.World.Config;

namespace DragonFiesta.World.InternNetwork.InternHandler.Response.Login
{
    public static class Auth_Response
    {
        public static void HandleAuthenticateWorld_Response(IMessage msg)
        {
            var response = (msg as AuthenticatedWorld_Response);

            switch (response.Result)
            {
                case InternWorldAuthResult.AlredyRegister:
                    EngineLog.Write(EngineLogLevel.Exception, "World With ID {0} Alredy Resgitriert", WorldConfiguration.Instance.WorldID);
                    break;

                case InternWorldAuthResult.Error:
                    EngineLog.Write(EngineLogLevel.Exception, "Unknown Error to Auth Login");
                    break;

                case InternWorldAuthResult.InvalidPassword:
                    EngineLog.Write(EngineLogLevel.Exception, "Invalid Auth Password...");
                    break;

                case InternWorldAuthResult.InvalidWorldID:
                    EngineLog.Write(EngineLogLevel.Exception, "Invalid Auth WorldId..");
                    break;

                case InternWorldAuthResult.OK:
                    WorldConfiguration.Instance.ServerRegion = response.Region;

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
            }

            if (response.Result != InternWorldAuthResult.OK)
                ServerMain.InternalInstance.Shutdown();
            else
                ServerMain.InternalInstance.Title.Update();
        }

        //When Auth Sending TimeOutet then again...
        public static void AuthLoginTimeout(IMessage msg) => InternConnector.Instance.Dispose();
    }
}