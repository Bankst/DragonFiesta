using DragonFiesta.Messages.Zone;
using DragonFiesta.Messages.Zone.Zone;
using DragonFiesta.Utils.Logging;
using DragonFiesta.Zone.Core;
using DragonFiesta.Zone.Game.Zone;

namespace DragonFiesta.Zone.InternNetwork.InternHandler.Client.Zone
{
    public class ZoneHandler
    {
        [InternMessageHandler(typeof(ServerShutdown))]
        public static void HandleServerShutdown(ServerShutdown ZoneMsg, InternWorldConnector pSession)
            => ServerMain.InternalInstance.Shutdown();
        


        [InternMessageHandler(typeof(NewZoneStarted))]
        public static void HandleNewZoneStarted(NewZoneStarted ZoneMsg, InternWorldConnector pSession)
        {
            if (!ZoneManager.AddRemoteZone(new RemoteZone(ZoneMsg.ZoneServer.ID, ZoneMsg.ZoneServer.NetInfo)))
            {
                GameLog.Write(GameLogLevel.Internal, $"Invalid RemoteZone Start Because Zone {ZoneMsg.ZoneServer.ID } Already Registret");
                return;
            }

            GameLog.Write(GameLogLevel.Internal, $"Start New RemoteZone {ZoneMsg.ZoneServer.ID }");
            ServerMain.InternalInstance.Title.Update();
        }

        [InternMessageHandler(typeof(ZoneStopped))]
        public static void HandleZoneStopped(ZoneStopped Msg, InternWorldConnector pSession)
        {
            if (!ZoneManager.RemoveRemoteZone(Msg.ZoneId, out RemoteZone mZone))
                return;

            ServerMain.InternalInstance.Title.Update();

            //here stop Remote Maps
            GameLog.Write(GameLogLevel.Internal, $"Stop RemoteZone {mZone.ID }");
        }

        [InternMessageHandler(typeof(UpdateZoneServer))]
        public static void HandleRemoteZoneUpdate(UpdateZoneServer ZoneMsg, InternWorldConnector pSession)
        {
            if (!ZoneManager.GetRemoteZoneByID(ZoneMsg.ZoneId, out RemoteZone mZone))
                return;

            mZone.CurrentConnection = ZoneMsg.CurrentConnection;

            GameLog.Write(GameLogLevel.Debug, $"Update RemoteZone {mZone.ID }");
        }
    }
}