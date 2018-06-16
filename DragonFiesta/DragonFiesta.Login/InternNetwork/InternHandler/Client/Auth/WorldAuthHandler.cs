using DragonFiesta.Game.Accounts;
using DragonFiesta.Login.Config;
using DragonFiesta.Login.Core;
using DragonFiesta.Login.Game.Accounts;
using DragonFiesta.Login.Game.Worlds;
using DragonFiesta.Login.InternNetwork;
using DragonFiesta.Messages.Message.Auth;

namespace DragonFiesta.Login.Network.InternHandler.Client
{
    public class WorldAuthHandler
    {
        [InternMessageHandler(typeof(AuthenticatedWorld))]
        public static void HandleAuthenticatedWorld(AuthenticatedWorld Request, InternWorldSession pSession)
        {
            var Result = InternWorldAuthResult.Error;
            var Region = ClientRegion.None;

            if (!Request.Password.Equals(LoginConfiguration.Instance.InternServerInfo.Password))
            {
                Result = InternWorldAuthResult.InvalidPassword;
            }
            else if (!WorldManager.Instance.GetWorldByID(Request.WorldId, out World MyWorld))
            {
                Result = InternWorldAuthResult.InvalidWorldID;
            }
            else if (MyWorld.IsConnected)
            {
                Result = InternWorldAuthResult.Error;
            }
            else
            {
                pSession.SessionStateInfo.Authenticated = true;

                MyWorld.Session = pSession;
                pSession.World = MyWorld;

                MyWorld.ConnectionInfo = new WorldConnectionInfo
                {
                    WorldIP = Request.IP,
					WorldExternalIP = Request.ExternalIP,
                    WorldPort = Request.Port,
                    MaxPlayers = Request.MaxConnection,
                };
                Region = MyWorld.Info.Region;
                Result = InternWorldAuthResult.OK;

                foreach (var OnlineAccountId in Request.ActiveAccounts) //Reconnect Stuff...
                {
                    if (AccountManager.GetAccountByID(OnlineAccountId, out Account mAcount))
                    {
                        if (!mAcount.IsOnline)
                        {
                            mAcount.IsOnline = true;
                            AccountManager.UpdateAccountState(mAcount);
                        }
                    }
                }

                ServerMain.InternalInstance.Title.Update();
                EngineLog.Write(EngineLogLevel.Info, "Assigned World {0}", MyWorld.Info.WorldID);
            }

            var Response = new AuthenticatedWorld_Response
            {
                Id = Request.Id,
                Result = Result,
                Region = Region,
            };

            pSession.SendMessage(Response);
        }

        [InternMessageHandler(typeof(UpdateWorldServer))]
        public static void HandleUpdateServer(UpdateWorldServer Request, InternWorldSession pSession)
        {
            if (pSession.HasWorld)
            {
                if (Request.WorldReady && !pSession.World.IsReady)
                {
                    EngineLog.Write(EngineLogLevel.Debug, "World {0} is out of Maintenance", pSession.World.Info.WorldID);

                    pSession.World.IsReady = true;
                }
                pSession.World.OnlinePlayers = Request.NowPlayerCount;
            }
        }
    }
}