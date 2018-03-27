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
                    Packet.WriteString(NewZone.NetInfo.ListeningIP, 16);
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
                //   pPacket.WriteHexAsBytes("40 24 48 03 00 00 00 00 00 00 B0 04 00 00 00 00 00 00 18 00 00 00 18 00 00 00 11 00 00 00 11 00 00 00 17 00 00 00 17 00 00 00 06 00 00 00 06 00 00 00 00 00 00 00 00 00 FF FF 10 00 00 00 10 00 00 00 00 00 00 00 3D 00 00 00 00 00 00 00 4E 00 00 00 00 00 00 00 11 00 00 00 00 00 00 00 32 00 00 00 00 00 00 00 17 00 00 00 00 00 00 00 06 00 00 00 00 00 00 00 06 00 00 00 00 00 00 00 10 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 B1 00 00 00 82 00 00 00 00 00 00 00 00 00 00 00 17 00 00 00 11 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4D 19 00 00 C3 28 00 00");
                SH06Helper.WriteDetailedInfoExtra(pSession.Character, pPacket);
                pSession.SendPacket(pPacket);
            }
        }
    }
}