using DragonFiesta.Messages.Zone;
using DragonFiesta.Messages.Zone.Maps;
using DragonFiesta.Messages.Zone.Zone;
using DragonFiesta.Utils.Logging;
using DragonFiesta.Zone.Core;
using DragonFiesta.Zone.Game.Character;
using DragonFiesta.Zone.Game.Zone;
using DragonFiesta.Zone.InternNetwork.InternHandler.Server.Zone;
using DragonFiesta.Zone.Network;

namespace DragonFiesta.Zone.InternNetwork.InternHandler.Response.Auth
{
    public class Auth_Response
    {
        public static void HandleAuthenticatetZone_Response(IMessage msg)
        {
            var Response = (msg as AuthenticatedZone_Response);

            switch (Response.Result)
            {
                case InternZoneAuthResult.InvalidPassword:
                    GameLog.Write(GameLogLevel.Internal, "Invalid Authenticate Password Please check you Configurartion");
                    break;

                case InternZoneAuthResult.IdAlreadyRegistered:
                    GameLog.Write(GameLogLevel.Internal, "ZoneId is Already Connectet Please check you config!");
                    break;

                case InternZoneAuthResult.InvalidZoneId:
                    GameLog.Write(GameLogLevel.Internal, "Invalid ZoneId Please check you WorldConfiguration!");
                    break;

                case InternZoneAuthResult.OK:

                    GameLog.Write(GameLogLevel.Internal, "Authenticated OK");

                    //Register Remote Zones
                    Response.RemoteZoneList.ForEach(Zone => ZoneManager.AddRemoteZone(new RemoteZone(Zone.ID, Zone.NetInfo)));

                    ServerMain.InternalInstance.Title.Update();

                    if (!ServerMain.InternalInstance.IsDataLoaded) //not loaded by reconnect
                    {
                        if (!ServerMain.LoadGameServer())
                            throw new StartupException("Invalid Load GameServer Modules...");

                        if (!ZoneServer.Start(Response.Region))
                        {
                            ServerMain.InternalInstance.Shutdown();
                            return;
                        }

                        ServerMain.InternalInstance.IsDataLoaded = true;
                    }

                    ZoneServerMethods.SendMapListRequest((resp) =>
                    {
                        if (resp is MapListRequest)
                        {

                            ZoneServerMethods.SendOnlineCharacterRequest((Request) =>
                            {
                                if (Request is OnlineCharacterListRequest res)
                                {
                                    foreach (var OnlineInfo in res.OnlineCharacters)
                                    {
                                        if (!ZoneCharacterManager.Instance.GetCharacterByCharacterID(OnlineInfo.CharacterId,
                                            out ZoneCharacter Character,
                                            out CharacterErrors Result,
                                            true))
                                        {
                                            GameLog.Write(GameLogLevel.Warning, "Reconnecting Zone Failed Load Online Character {0}", OnlineInfo.CharacterId);
                                            continue;
                                        }

                                        ZoneCharacterManager.Instance.LogCharacterIn(Character);
                                    }
                                    ServerMain.InternalInstance.ServerIsReady = true;

                                    EngineLog.Write(EngineLogLevel.Startup, "ZoneServer Start Success!");

                                }
                            });
                        }
                    });


                    break;
            }

            if (Response.Result != InternZoneAuthResult.OK)
                ServerMain.InternalInstance.Shutdown();
            else
                ServerMain.InternalInstance.Title.Update();
        }

        public static void AuthWorldTimeout(IMessage msg) => InternWorldConnector.Instance.SendAuth();
    }
}