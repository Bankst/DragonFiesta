using System;
using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Networking.Helpers;
using DragonFiesta.Zone.Game.Character;

namespace DragonFiesta.Zone.Network.Helpers
{
    public class SH04Helpers
    {
        public static void SendZoneError(ZoneSession mSession, ConnectionError error)
        {
            SendZoneError(mSession, (CharacterErrors)error);
        }

        public static void SendZoneError(ZoneSession mSession, CharacterErrors error)
        {
            using (var packet = new FiestaPacket(Handler04Type._Header, Handler04Type.SMSG_CHAR_LOGINFAIL_ACK))
            {
                packet.Write<ushort>((ushort)error);
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
        public static void WriteDetailedInfo(ZoneCharacter character, FiestaPacket packet)
        {
            packet.Write<int>(character.Info.CharacterID);
            packet.WriteString(character.Info.Name, 20);

            packet.Write<byte>(character.Info.Slot);//slot
            packet.Write<byte>(character.Info.Level);
            packet.Write<ulong>(character.Info.EXP);//exp

            packet.Write<ushort>(0);//pwrstone
            packet.Write<ushort>(0);//Grstone
            packet.Write<ushort>(character.Info.HPStones);//HpStones
            packet.Write<ushort>(character.Info.SPStones);//SpStone

            packet.Write<uint>(character.LivingStats.HP);//hp
            packet.Write<uint>(character.LivingStats.SP);//sp
            packet.Write<uint>(character.LivingStats.LP);//lightpower

            packet.Write<uint>(character.Info.Fame);//fame
            packet.Write<ulong>(character.Info.Money);//money

            packet.WriteString(character.AreaInfo.MapInfo.Index, 12);
            packet.Write<uint>(character.AreaInfo.Position.X);
            packet.Write<uint>(character.AreaInfo.Position.Y);
            packet.Write<byte>(character.AreaInfo.Position.Rotation);

            //stat points
            packet.Write<byte>(character.Info.FreeStats.StatPoints_STR); // str points
            packet.Write<byte>(character.Info.FreeStats.StatPoints_END); // end points
            packet.Write<byte>(character.Info.FreeStats.StatPoints_DEX); // dex points
            packet.Write<byte>(character.Info.FreeStats.StatPoints_INT); // int points
            packet.Write<byte>(character.Info.FreeStats.StatPoints_SPR); // spr points

            packet.Fill(2, 0); // tmp ?

            packet.Write<uint>(character.Info.KillPoints);

            packet.Fill(7, 0); // tmp ?
        }

        public static void WriteShape(ZoneCharacter character, FiestaPacket packet)
        {
            packet.Write<byte>(0); //Unk
            packet.Write<byte>(character.Style.Hair.ID);
            packet.Write<byte>(character.Style.HairColor.ID);
            packet.Write<byte>(character.Style.Face.ID);
        }

        public static void WriteMinihouse(ZoneCharacter character, FiestaPacket packet)
        {
            packet.Write<ushort>(0); // minihouse (handle?)
            packet.WriteHexAsBytes("00 FF FF FF FF FF 50 00 00 00"); // dummy[10] -> captured from gamigo NA
        }

        public static void WriteCharacterDisplay(ZoneCharacter character, FiestaPacket packet)
        {
            packet.Write<ushort>(character.MapObjectId);
            packet.WriteString(character.Info.Name, 20);

            packet.Write<uint>(character.Position.X);
            packet.Write<uint>(character.Position.Y);
            packet.Write<byte>(character.Position.Rotation);

            packet.Write<byte>(character.State);
            packet.Write<byte>(character.Info.Class);
            _SH04Helpers.WriteLook(character, packet);

            switch (character.State)
            {
                case CharacterState.Player:
                    _SH04Helpers.WriteEquipment(character, packet);
                    _SH04Helpers.WriteRefinement(character, packet);
                    break;

                case CharacterState.Resting:
                    WriteMinihouse(character, packet);
                        break;

                case CharacterState.Vendor:
                    WriteMinihouse(character, packet);
                    packet.Write<byte>(0); // isSell
                    packet.Fill(30, 0x00); // signboard[30] 
                    break;

                case CharacterState.OnMount:
                    _SH04Helpers.WriteEquipment(character, packet);
                    _SH04Helpers.WriteRefinement(character, packet);
                    packet.Write<ushort>(0);
                    break;
            }

            packet.Fill(2, 0xFF);
            packet.Write<ushort>(0xFF); // polymorph id (some skill idk lmao)

            //STOPEMOTICON_DESCRIPT
            packet.Write<byte>(0); // emoticonId
            packet.Write<ushort>(0); // emoticonFrame

            //CHARTITLE_BRIEFINFO
            packet.Write<byte>(0); // Type
            packet.Write<byte>(0); // ElementNo
            packet.Write<ushort>(0); // MobID

            //ABNORMAL_STATE_BIT
            packet.Fill(105, 0xFF); // statebit[105]

            packet.Write<uint>(0); //myGuild
            packet.Write<byte>(character.Type);
            packet.Write<byte>(0); // isGuildAcademyMember
            packet.Write<byte>(0); // isAutoPick
            packet.Write<byte>(character.Level);
            packet.Fill(32, 0x00); // sAnimation[32]
            packet.Write<ushort>(0); // nMoverHnd
            packet.Write<byte>(0); // nMoverSlot
            packet.Write<byte>(0); // nKQTeamType
            packet.Write<byte>(0); // IsUseItemMinimon
        }

        public static void WriteEquippedItemList(ZoneCharacter character, FiestaPacket packet)
        {
        }

        public static void WriteInventoryItemList(ZoneCharacter character, FiestaPacket packet)
        {
        }

        public static void WriteMiniHouseList(ZoneCharacter character, FiestaPacket packet)
        {
        }

        public static void WritePremiumEmotionsList(ZoneCharacter character, FiestaPacket packet)
        {
        }

        public static void WriteTimedItemList(ZoneCharacter character, FiestaPacket packet)
        {
        }
    }
}