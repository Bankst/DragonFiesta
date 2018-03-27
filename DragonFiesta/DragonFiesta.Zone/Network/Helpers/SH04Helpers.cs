using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Zone.Game.Character;

namespace DragonFiesta.Zone.Network.Helpers
{
    public class SH04Helpers
    {
        public static void SendZoneError(ZoneSession mSession, ConnectionError Error)
        {
            SendZoneError(mSession, (CharacterErrors)Error);
        }

        public static void SendZoneError(ZoneSession mSession, CharacterErrors Error)
        {
            using (var packet = new FiestaPacket(Handler04Type._Header, Handler04Type.SMSG_CHAR_LOGINFAIL_ACK))
            {
                packet.Write<ushort>((ushort)Error);
                mSession.SendPacket(packet);
            }
        }
        /*
        struct PROTO_NC_CHAR_BASE_CMD::<unnamed-type-flags>::<unnamed-type-str>
        {
            int _bf0;
        };


        union PROTO_NC_CHAR_BASE_CMD::<unnamed-type-flags>
        {
            unsigned int bin;
            PROTO_NC_CHAR_BASE_CMD::<unnamed-type-flags>::<unnamed-type-str> str;
        };

        struct PROTO_NC_CHAR_BASE_CMD
        {
            unsigned int chrregnum;
            Name5 charid;
            char slotno;
            char Level;
            unsigned __int64 Experience;
            unsigned __int16 CurPwrStone;
            unsigned __int16 CurGrdStone;
            unsigned __int16 CurHPStone;
            unsigned __int16 CurSPStone;
            unsigned int CurHP;
            unsigned int CurSP;
            unsigned int CurLP;
            unsigned int fame;
            unsigned __int64 Cen;
            PROTO_NC_CHAR_BASE_CMD::LoginLocation logininfo;
            CHARSTATDISTSTR statdistribute;
            char pkyellowtime;
            unsigned int pkcount;
            unsigned __int16 prisonmin;
            char adminlevel;
            PROTO_NC_CHAR_BASE_CMD::<unnamed-type-flags> flags;
        };

    */
        public static void WriteDetailedInfo(ZoneCharacter Character, FiestaPacket Packet)
        {
            Packet.Write<int>(Character.Info.CharacterID);
            Packet.WriteString(Character.Info.Name, 20);

            Packet.Write<byte>(Character.Info.Slot);//slot
            Packet.Write<byte>(Character.Info.Level);
            Packet.Write<ulong>(Character.Info.EXP);//exp

            Packet.Write<ushort>(0);//pwrstone
            Packet.Write<ushort>(0);//Grstone
            Packet.Write<ushort>(Character.Info.HPStones);//HpStones
            Packet.Write<ushort>(Character.Info.SPStones);//SpStone

            Packet.Write<uint>(Character.LivingStats.HP);//hp
            Packet.Write<uint>(Character.LivingStats.SP);//sp
            Packet.Write<uint>(Character.LivingStats.LP);//lightpower

            Packet.Write<uint>(Character.Info.Fame);//fame
            Packet.Write<ulong>(Character.Info.Money);//money

            Packet.WriteString(Character.AreaInfo.MapInfo.Index, 12);
            Packet.Write<uint>(Character.AreaInfo.Position.X);
            Packet.Write<uint>(Character.AreaInfo.Position.Y);
            Packet.Write<byte>(Character.AreaInfo.Position.Rotation);

            //stat points
            Packet.Write<byte>(Character.Info.FreeStats.StatPoints_STR); // str points
            Packet.Write<byte>(Character.Info.FreeStats.StatPoints_END); // end points
            Packet.Write<byte>(Character.Info.FreeStats.StatPoints_DEX); // dex points
            Packet.Write<byte>(Character.Info.FreeStats.StatPoints_INT); // int points
            Packet.Write<byte>(Character.Info.FreeStats.StatPoints_SPR); // spr points

            Packet.Fill(2, 0); // tmp ?

            Packet.Write<uint>(Character.Info.KillPoints);

            Packet.Fill(7, 0); // tmp ?
        }



        public static void WriteCharacterDisplay(ZoneCharacter Character, FiestaPacket Packet)
        {
            Packet.Write<ushort>(Character.MapObjectId);
            Packet.WriteString(Character.Info.Name, 20);

            Packet.Write<uint>(Character.Position.X);
            Packet.Write<uint>(Character.Position.Y);
            Packet.Write<byte>(Character.Position.Rotation);
            Packet.WriteHexAsBytes("01 17 DD 06 02 00 43 E0 FF FF FB E0 45 E0 FF FF 44 E0 42 E1 FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF FF E3 94 FF FF 00 00 00 00 00 FF FF FF 00 00 4A 00 00 00 00 00 90 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 40 00 00 01 04 00 00 00 00 00 08 20 00 00 00 00 00 00 00 00 00 00 00 A0 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 4E 43 00 00 02 01 00 41 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF 0A 02 00");
        }

        public static void WriteEquippedItemList(ZoneCharacter Character, FiestaPacket Packet)
        {
        }

        public static void WriteInventoryItemList(ZoneCharacter Character, FiestaPacket Packet)
        {
        }

        public static void WriteMiniHouseList(ZoneCharacter Character, FiestaPacket Packet)
        {
        }

        public static void WritePremiumEmotionsList(ZoneCharacter Character, FiestaPacket Packet)
        {
        }

        public static void WriteTimedItemList(ZoneCharacter Character, FiestaPacket Packet)
        {
        }
    }
}