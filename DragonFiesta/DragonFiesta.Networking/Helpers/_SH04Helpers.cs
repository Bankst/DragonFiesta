using DragonFiesta.Game.Characters;
using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Networking.Network;
using System;

namespace DragonFiesta.Networking.Helpers
{
    public static class _SH04Helpers
    {
        public static void WriteBasicInfo(CharacterBase Character, FiestaPacket Packet)
        {
            Packet.Write<int>(Character.Info.CharacterID);
            Packet.WriteString(Character.Info.Name, 20);
            Packet.Write<ushort>(Character.Info.Level);
            Packet.Write<byte>(Character.Info.Slot);
            Packet.WriteString(Character.AreaInfo.MapInfo.Index, 12);

            // Delete Info
            Packet.Write<byte>(0); // delete year
            Packet.Write<byte>(0); // delete moth
            Packet.Write<byte>(0); // delete day
            Packet.Write<byte>(0); // delete hour
            Packet.Write<byte>(0); // delete min

            WriteLook(Character, Packet);
            WriteEquipment(Character, Packet);
            WriteRefinement(Character, Packet);

            Packet.Write<uint>(0); // nKQHandle
            Packet.WriteString(Character.AreaInfo.MapInfo.Index, 12);
            Packet.Write<uint>(Character.AreaInfo.Position.X);
            Packet.Write<uint>(Character.AreaInfo.Position.Y);

            // KQ Date
            Packet.Write<int>(0);   //ShineDateTime
            Packet.Write<byte>(0);  // bneedchangeid
            Packet.Write<byte>(0);  // Init
            Packet.Write<uint>(0);  // RowNo
            /*enum TUTORIAL_STATE
            {
              TS_PROGRESS = 0x0,
              TS_DONE = 0x1,
              TS_SKIP = 0x2,
              TS_EXCEPTION = 0x3,
              TS_MAX = 0x4,
            };*/
            Packet.Write<byte>(1);  // TutorialState
            Packet.Write<byte>(0);  // TutorialStep
            Packet.Fill(3, 0x00);   //unk
        }

        public static void WriteLook(CharacterBase Character, FiestaPacket Packet)
        {
            Packet.Write<byte>(Convert.ToByte(0x01 | ((byte)Character.Info.Class << 2) | (Character.Info.IsMale ? 1 : 0) << 7));
            Packet.Write<byte>(Character.Style.Hair.ID);
            Packet.Write<byte>(Character.Style.HairColor.ID);
            Packet.Write<byte>(Character.Style.Face.ID);
        }

        public static void WriteEquipment(CharacterBase Character, FiestaPacket Packet)
        {
            Packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_HAT);              // Equ_Head
            Packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_MOUTH);            // Equ_Mouth
            Packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_RIGHTHAND);        // Equ_RightHand
            Packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_BODY);             // Equ_Body
            Packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_LEFTHAND);         // Equ_LeftHand
            Packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_LEG);              // Equ_Pant
            Packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_SHOES);            // Equ_Boot
            Packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_SHOESACC);         // Equ_AccBoot
            Packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_LEGACC);           // Equ_AccPant
            Packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_BODYACC);          // Equ_AccBody
            Packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_HATACC);           // Equ_AccHeadA
            Packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_MINIMON_R);        // Equ_MiniMon_R
            Packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_EYE);              // Equ_Eye
            Packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_LEFTHANDACC);      // Equ_AccLeftHand
            Packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_RIGHTHANDACC);     // Equ_AccRightHand
            Packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_BACK);             // Equ_AccBack
            Packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_COSEFF);           // Equ_CosEff
            Packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_TAIL);             // Equ_AccHip
            Packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_MINIMON);          // Equ_Minimon
            Packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_SHIELDACC);        // Equ_AccShield
        }

        public static void WriteRefinement(CharacterBase Character, FiestaPacket Packet)
        {
            Packet.Write<byte>(0xff); //(Convert.ToByte(Character.Items.GetItemUpgradesByEquipSlot(ItemEquipSlot.Weapon) << 4 | Character.Items.GetItemUpgradesByEquipSlot(ItemEquipSlot.Weapon2)));
            Packet.Write<byte>(0xff);
            Packet.Write<byte>(0xff);
        }

        public static void SendCharacterError<TSession>(FiestaSession<TSession> mSession, ConnectionError Error) where TSession : FiestaSession<TSession>
        {
            SendCharacterError(mSession, (CharacterErrors)Error);
        }

        public static void SendCharacterError<TSession>(FiestaSession<TSession> mSession, CharacterErrors Error) where TSession : FiestaSession<TSession>
        {
            using (var packet = new FiestaPacket(Handler04Type._Header, Handler04Type.SMSG_CHAR_LOGINFAIL_ACK))
            {
                packet.Write<ushort>((ushort)Error);
                mSession.SendPacket(packet);
            }
        }
    }
}