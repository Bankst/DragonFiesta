using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Providers.Maps;
using DragonFiesta.Zone.Game.Zone;
using DragonFiesta.Zone.Network.Helpers;

namespace DragonFiesta.Zone.Network.FiestaHandler.Server
{
    public static class SH06Handler
    {
        public static void SendChangeMap(ZoneSession Sesssion, MapInfo NewMap, uint SpawnX, uint SpawnY, RemoteZone NewZone = null)
        {
            var IsZoneTransfer = (NewZone != null);

            using (var Packet = new FiestaPacket(Handler06Type._Header, IsZoneTransfer ? Handler06Type.SMSG_MAP_LINKOTHER_CMD : Handler06Type.SMSG_MAP_LINKSAME_CMD))
            {
                Packet.Write<ushort>(NewMap.ID);
                Packet.Write<uint>(SpawnX);
                Packet.Write<uint>(SpawnY);

                if (IsZoneTransfer)
                {
                    Packet.WriteString(NewZone.NetInfo.ExternalIP, 16);
                    Packet.Write<ushort>(NewZone.NetInfo.ListeningPort);
                    Packet.Write<ushort>(Sesssion.Character.WorldSessionId);
                }

                Sesssion.SendPacket(Packet);
            }
        }
        public static void SendMapConnectError(ZoneSession Session, ConnectionError Error)
        {
            using (var packet = new FiestaPacket(Handler06Type._Header, Handler06Type.SMSG_MAP_LOGINFAIL_ACK))
            {
                packet.Write<ushort>(Error);
                packet.Write<byte>(0);  //nWrongDataFileIndex
                Session.SendPacket(packet);
            }
        }

        public static void SendDetailedInfoExtra(ZoneSession pSession)
        {
            using (var pPacket = new FiestaPacket(Handler06Type._Header, Handler06Type.SMSG_MAP_LOGIN_ACK))
            {
                SH06Helper.WriteDetailedInfoExtra(pSession.Character, pPacket);
                pSession.SendPacket(pPacket);
            }
        }
    }
}