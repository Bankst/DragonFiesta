using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Networking.Helpers;
using DragonFiesta.Zone.Game.Character;
using DragonFiesta.Zone.Network.Helpers;

namespace DragonFiesta.Zone.Network.FiestaHandler.Server
{
    public class SH04Handler
    {
        public static void SendCharacterInfo(ZoneSession pSession)
        {
            using (var pPacket = new FiestaPacket(Handler04Type._Header, Handler04Type.SMSG_CHAR_CLIENT_BASE_CMD))
            {
                SH04Helpers.WriteDetailedInfo(pSession.Character, pPacket);
                pSession.SendPacket(pPacket);
            }
        }

        public static void SendCharacterLook(ZoneSession pSession)
        {
            using (var pPacket = new FiestaPacket(Handler04Type._Header, Handler04Type.SMSG_CHAR_CLIENT_SHAPE_CMD))
            {
                _SH04Helpers.WriteLook(pSession.Character, pPacket);
                pSession.SendPacket(pPacket);
            }
        }

        public static void SendCharacterInfoEnd(ZoneSession pSession)
        {
            using (var packet = new FiestaPacket(Handler04Type._Header, Handler04Type.SMSG_CHAR_CLIENT_GAME_CMD))
            {
                packet.Write<ushort>(0xffff); // unk ?
                packet.Write<ushort>(0xffff);
                pSession.SendPacket(packet);
            }
        }

        public static void SendUpdateMoney(ZoneCharacter Character)
        {
            using (var Packet = new FiestaPacket(Handler04Type._Header, Handler04Type.SMSG_CHAR_CENCHANGE_CMD))
            {
                Packet.Write<ulong>(Character.Info.Money);
                Character.Session.SendPacket(Packet);
            }
        }
        public static void SendRemainingStatPoints(ZoneSession Session)
        {
            using (var Packet = new FiestaPacket(Handler04Type._Header, Handler04Type.SMSG_CHAR_STAT_REMAINPOINT_CMD))
            {
                Packet.Write<byte>(Session.Character.Info.FreeStats.FreeStat_Points);
                Session.SendPacket(Packet);
            }
        }
        public static void SendPointOnStat(ZoneSession Session,byte Type)
        {
            using (var Packet = new FiestaPacket(Handler04Type._Header, Handler04Type.SMSG_CHAR_STAT_INCPOINTSUC_ACK))
            {
                Packet.Write<byte>(Type);
                Session.SendPacket(Packet);
            }
        }
        public static void SendUpdateCharacterStats(ZoneCharacter pChar, params StatsType[] ToUpdateStats)
        {
            if (pChar.IsConnected)
            {
                using (var Packet = new FiestaPacket(Handler04Type._Header, Handler04Type.SMSG_CHAR_CHANGEPARAMCHANGE_CMD))
                {
                    Packet.Write<byte>(ToUpdateStats.Length);

                    for (int i = 0; i < ToUpdateStats.Length; i++)
                    {
                        Packet.Write<byte>(ToUpdateStats[i]);
                        Packet.Write<int>(pChar.Info.Stats.GetStatByType(ToUpdateStats[i]));
                    }
                }
            }
        }

        // works ? not tested..
        public static void SendRevive(ZoneSession session)
        {
            using (var Packet = new FiestaPacket(Handler04Type._Header, Handler04Type.SMSG_CHAR_REVIVESAME_CMD))
            {
                Packet.Write<ushort>(session.Character.Map.MapId);
                Packet.Write<uint>(session.Character.Position.X);
                Packet.Write<uint>(session.Character.Position.Y);
                session.SendPacket(Packet);
            }
        }

	    [PacketHandler(Handler04Type.SMSG_CHAR_CLIENT_ITEM_CMD)]
	    public static void SendCharItems(ZoneSession session)
	    {
		    using (var packet = new FiestaPacket(Handler04Type._Header, Handler04Type.SMSG_CHAR_CLIENT_ITEM_CMD))
		    {
			    SH04Helpers.WriteInventoryItemList(session.Character, packet);
			}
	    }
	}
}