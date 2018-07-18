using DragonFiesta.Game.Characters;
using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Networking.Network;
using System;
using DragonFiesta.Networking.Network.Session;

namespace DragonFiesta.Networking.Helpers
{
    public static class _SH04Helpers
    {
        public static void WriteBasicInfo(CharacterBase character, FiestaPacket packet)
        {
            packet.Write<int>(character.Info.CharacterID);
            packet.WriteString(character.Info.Name, 20);
            packet.Write<ushort>(character.Info.Level);
            packet.Write<byte>(character.Info.Slot);
            packet.WriteString(character.AreaInfo.MapInfo.Index, 12);

            // Delete Info
            packet.Write<byte>(0); // delete year
            packet.Write<byte>(0); // delete month
            packet.Write<byte>(0); // delete day
            packet.Write<byte>(0); // delete hour
            packet.Write<byte>(0); // delete min

            WriteLook(character, packet);
            WriteEquipment(character, packet);
            WriteRefinement(character, packet);

            packet.Write<uint>(0); // nKQHandle
            packet.WriteString(character.AreaInfo.MapInfo.Index, 12);
            packet.Write<uint>(character.AreaInfo.Position.X);
            packet.Write<uint>(character.AreaInfo.Position.Y);

            // KQ Date
            packet.Write<int>(0);   //ShineDateTime
            packet.Write<byte>(0);  // bneedchangeid
            packet.Write<byte>(0);  // Init
            packet.Write<uint>(0);  // RowNo
            /*enum TUTORIAL_STATE
            {
              TS_PROGRESS = 0x0,
              TS_DONE = 0x1,
              TS_SKIP = 0x2,
              TS_EXCEPTION = 0x3,
              TS_MAX = 0x4,
            };*/
            packet.Write<byte>(1);  // TutorialState
            packet.Write<byte>(0);  // TutorialStep
            packet.Fill(3, 0x00);   //unk
        }

        public static void WriteLook(CharacterBase character, FiestaPacket packet)
        {
            packet.Write<byte>(Convert.ToByte(0x01 | ((byte)character.Info.Class << 2) | (character.Info.IsMale ? 1 : 0) << 7));
            packet.Write<byte>(character.Style.Hair.ID);
            packet.Write<byte>(character.Style.HairColor.ID);
            packet.Write<byte>(character.Style.Face.ID);
        }

        public static void WriteEquipment(CharacterBase character, FiestaPacket packet)
        {
            packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_HAT);              // Equ_Head
            packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_MOUTH);            // Equ_Mouth
            packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_RIGHTHAND);        // Equ_RightHand
            packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_BODY);             // Equ_Body
            packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_LEFTHAND);         // Equ_LeftHand
            packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_LEG);              // Equ_Pant
            packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_SHOES);            // Equ_Boot
            packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_SHOESACC);         // Equ_AccBoot
            packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_LEGACC);           // Equ_AccPant
            packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_BODYACC);          // Equ_AccBody
            packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_HATACC);           // Equ_AccHeadA
            packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_MINIMON_R);        // Equ_MiniMon_R
            packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_EYE);              // Equ_Eye
            packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_LEFTHANDACC);      // Equ_AccLeftHand
            packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_RIGHTHANDACC);     // Equ_AccRightHand
            packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_BACK);             // Equ_AccBack
            packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_COSEFF);           // Equ_CosEff
            packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_TAIL);             // Equ_AccHip
            packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_MINIMON);          // Equ_Minimon
            packet.Write<ushort>(ItemEquipSlot.ITEMEQUIP_SHIELDACC);        // Equ_AccShield
        }

        public static void WriteRefinement(CharacterBase character, FiestaPacket packet)
        {
            packet.Write<byte>(0xFF); //(Convert.ToByte(Character.Items.GetItemUpgradesByEquipSlot(ItemEquipSlot.Weapon) << 4 | Character.Items.GetItemUpgradesByEquipSlot(ItemEquipSlot.Weapon2)));
            packet.Write<byte>(0xFF);
            packet.Write<byte>(0xFF);
        }

        public static void SendCharacterError<TSession>(FiestaSession<TSession> mSession, ConnectionError error) where TSession : FiestaSession<TSession>
        {
            SendCharacterError(mSession, (CharacterErrors)error);
        }

        public static void SendCharacterError<TSession>(FiestaSession<TSession> mSession, CharacterErrors error) where TSession : FiestaSession<TSession>
        {
            using (var packet = new FiestaPacket(Handler04Type._Header, Handler04Type.SMSG_CHAR_LOGINFAIL_ACK))
            {
                packet.Write<ushort>((ushort)error);
                mSession.SendPacket(packet);
            }
        }
    }
}