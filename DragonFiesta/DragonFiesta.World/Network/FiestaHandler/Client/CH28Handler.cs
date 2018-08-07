using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.World.Network.FiestaHandler.Server;
using System;
using DragonFiesta.Utils.Logging;

namespace DragonFiesta.World.Network.FiestaHandler.Client
{
    [PacketHandlerClass(Handler28Type._Header)]
    public class CH28Handler
    {
        [PacketHandler(Handler28Type.CMSG_CHAR_OPTION_IMPROVE_INIT_KEYMAP_REQ)]
        public static void CMSG_CHAR_OPTION_IMPROVE_INIT_KEYMAP_REQ(WorldSession sender, FiestaPacket packet)
        {
            if(!sender.Ingame)
            {
                sender.Dispose();
                return;
            }

            SH28Handler.SendDefaultKeyMap(sender);
        }

        [PacketHandler(Handler28Type.CMSG_CHAR_OPTION_IMPROVE_INIT_GAMEOPTION_REQ)]
        public static void CMSG_CHAR_OPTION_IMPROVE_INIT_GAMEOPTION_REQ(WorldSession sender, FiestaPacket packet)
        {
            if (!sender.Ingame)
            {
                sender.Dispose();
                return;
            }

            sender.Character.Options.GameSettings.LoadDefaultGameSettings();

            SH28Handler.SendGameOptions(sender);
        }

        [PacketHandler(Handler28Type.CMSG_CHAR_OPTION_IMPROVE_SET_KEYMAP_REQ)]
        public static void CMSG_CHAR_OPTION_IMPROVE_SET_KEYMAP_REQ(WorldSession sender, FiestaPacket packet)
        {
            if (!packet.Read(out ushort UpdateCount))
            {
                sender.Dispose();
                return;
            }

            for (int i = 0; i < UpdateCount; i++)
            {
                if(!packet.Read(out ushort KeyType) || !packet.Read(out byte ExtendKey) || ! packet.Read(out char ASCIKEY))
                {
                    sender.Dispose();
                    break;
                }

                if(!sender.Character.Options.KeyMap.UpdateKey(KeyType, ExtendKey, ASCIKEY))
                {
                    GameLog.Write(GameLogLevel.Warning, "Failed Update KeyMap for Character {0} Key {1} {2}", KeyType, ExtendKey, ASCIKEY);
                }

            }
        }
        [PacketHandler(Handler28Type.CMSG_CHAR_OPTION_GET_SHORTCUTSIZE_REQ)]
        public static void CMSG_CHAR_OPTION_GET_SHORTCUTSIZE_REQ(WorldSession sender, FiestaPacket packet)
        {
            using (var oPacket = new FiestaPacket(Handler28Type._Header, Handler28Type.SMSG_CHAR_OPTION_GET_SHORTCUTSIZE_ACK))
            {            
                oPacket.WriteHexAsBytes("01 05 00 03 18 00 00 05 00 0C 00 00 0C 01 00 0C 02 00 0C 03 00 0C 04 00 00");
                sender.SendPacket(oPacket);
            }
        }

        [PacketHandler(Handler28Type.CMSG_CHAR_OPTION_IMPROVE_SET_GAMEOPTION_REQ)]
        public static void CMSG_CHAR_OPTION_IMPROVE_SET_GAMEOPTION_REQ(WorldSession sender, FiestaPacket packet)
        {
            if (!sender.Ingame || !packet.Read(out ushort UpdateCount))
            {
                sender.Dispose();
                return;
            }

            for (ushort i = 0; i < UpdateCount; i++)
            {
                if (!packet.Read<ushort>(out ushort Type) ||
                    !packet.Read(out bool Enable))
                    continue;

                if (!Enum.IsDefined(typeof(GameSettingType), Type))
                    continue;

                sender.Character.Options.GameSettings.UpdateSettings((GameSettingType)Type, Enable);
            }
        }

        [PacketHandler(Handler28Type.CMSG_CHAR_OPTION_GET_WINDOWPOS_REQ)]
        public static void CMSG_CHARACTER_CLIENT_SETTINGS(WorldSession sender, FiestaPacket packet)
        {
            using (var oPacket = new FiestaPacket(31, 7))
            {
                oPacket.WriteHexAsBytes("B1 0D 00 00");
                sender.SendPacket(oPacket);
            }
        }
    }
}