using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.World.Game.Character;
using DragonFiesta.World.Game.Zone;

namespace DragonFiesta.World.Network.FiestaHandler.Server
{
    internal class SH04Handler
    {
        public static void SendIngameChunk(WorldCharacter Character)
        {
            SH04Handler.SendZoneServerIP(Character.Session, Character.Map.Zone);
            SH28Handler.SendQuickBarEntry(Character.Session);
            SH28Handler.SendKeyMap(Character.Session);
            SH28Handler.SendGameSettingsInital(Character.Session);
        }
        public static void SendZoneServerIP(WorldSession mSession, ZoneServer Zone)
        {
            using (var mPacket = new FiestaPacket(Handler04Type._Header, Handler04Type.SMSG_CHAR_LOGIN_ACK))
            {
                mPacket.WriteString(Zone.NetInfo.ListeningIP, 16);
                mPacket.Write<ushort>(Zone.NetInfo.ListeningPort);
                mSession.SendPacket(mPacket);
            }
        }

        public static void SendTutorialRequest(WorldSession Session)
        {
            using (var Packet = new FiestaPacket(Handler04Type._Header, Handler04Type.SMSG_CHAR_TUTORIAL_POPUP_REQ))
            {
                Packet.Write<byte>(1);//unk
                Session.SendPacket(Packet);
            }
        }
    }
}