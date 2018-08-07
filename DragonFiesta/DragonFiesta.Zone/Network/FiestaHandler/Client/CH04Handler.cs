using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Zone.Network.FiestaHandler.Server;
using DragonFiesta.Zone.Network.Helpers;

namespace DragonFiesta.Zone.Network.FiestaHandler.Client
{
    [PacketHandlerClass(Handler04Type._Header)]
    public static class CH04Handler
    {
        [PacketHandler(Handler04Type.CMSG_CHAR_LOGOUTREADY_CMD)]
        public static void CMSG_LOGOUT_BEGIN(ZoneSession sender, FiestaPacket packet)
        {
            if (!sender.Ingame)
            {
                sender.Dispose();
                return;
            }
            sender.IsLoggingOut = true;
            sender.Character.Save();
        }

        [PacketHandler(Handler04Type.CMSG_CHAR_LOGOUTCANCEL_CMD)]
        public static void CMSG_LOGOUT_CANCEL(ZoneSession sender, FiestaPacket packet)
        {
            if (!sender.Ingame)
            {
                sender.Dispose();
                return;
            }
            sender.IsLoggingOut = false;
        }


        [PacketHandler(Handler04Type.CMSG_CHAR_STAT_INCPOINT_REQ)]
        public static void CMSG_CHARACTER_STAT_POINTS(ZoneSession sender, FiestaPacket packet)
        {
            if (!sender.Ingame ||
                !packet.Read(out byte Type))
            {
                SH04Helpers.SendZoneError(sender, ConnectionError.ClientManipulation);
                sender.Dispose();
                return;
            }


            if (sender.Character.Info.FreeStats.FreeStat_Points < 1)
                return;

            switch ((FreeStatType)Type)
            {
                case FreeStatType.STR:
                    sender.Character.Info.FreeStats.StatPoints_STR++;
                    sender.Character.Info.Stats.Update_STR();
                    break;
                case FreeStatType.END:
                    sender.Character.Info.FreeStats.StatPoints_END++;
                    sender.Character.Info.Stats.Update_END();
                    break;
                case FreeStatType.DEX:
                    sender.Character.Info.FreeStats.StatPoints_DEX++;
                    sender.Character.Info.Stats.Update_DEX();
                    break;
                case FreeStatType.INT:
                    sender.Character.Info.FreeStats.StatPoints_INT++;
                    sender.Character.Info.Stats.Update_INT();
                    break;
                case FreeStatType.SPR:
                    sender.Character.Info.FreeStats.StatPoints_SPR++;
                    sender.Character.Info.Stats.Update_SPR();
                    break;
                default:
                    return;
            }

            sender.Character.Info.FreeStats.FreeStat_Points--;

            SH04Handler.SendPointOnStat(sender, Type);
        }

        // soon...
        [PacketHandler(Handler04Type.CMSG_CHAR_REVIVE_REQ)]
        public static void CMSG_CHAR_REVIVE_REQ(ZoneSession session, FiestaPacket packet)
        {

        }
    }
}
