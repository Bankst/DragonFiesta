using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Providers.Items.SHN
{
    public class ItemServerEquipTypeInfo
    {
        public uint ISET_Index { get; }

        public byte Equ_Neckles { get; }

        public byte Equ_Head { get; }

        public byte Equ_Ear { get; }

        public byte Equ_RightHand { get; }

        public byte Equ_Body { get; }

        public byte Equ_LeftHand { get; }

        public byte Equ_RingA { get; }

        public byte Equ_Pant { get; }

        public byte Equ_RingB { get; }

        public byte Equ_Boot { get; }

        public byte Equ_AccBoot { get; }

        public byte Equ_AccPant { get; }

        public byte Equ_AccBody { get; }

        public byte Equ_AccMouth { get; }

        public byte Equ_AccEye { get; }

        public byte Equ_AccHead { get; }

        public byte Equ_AccLeftHand { get; }

        public byte Equ_AccRightHand { get; }

        public byte Equ_AccBack { get; }

        public byte Equ_CosEff { get; }

        public byte Equ_AccHip { get; }

        public byte Equ_MiniMon { get; }

        public byte Equ_MiniMon_R { get; }

        public byte Equ_AccShield { get; }

        public byte Equ_Bracelet { get; }

        public ItemServerEquipTypeInfo(SHNResult pResult, int i)
        {
            ISET_Index = pResult.Read<uint>(i, "ISET_Index");
            Equ_Neckles = pResult.Read<byte>(i, "Equ_Neckles");
            Equ_Head = pResult.Read<byte>(i, "Equ_Head");
            Equ_Ear = pResult.Read<byte>(i, "Equ_Ear");
            Equ_RightHand = pResult.Read<byte>(i, "Equ_RightHand");
            Equ_Body = pResult.Read<byte>(i, "Equ_Body");
            Equ_LeftHand = pResult.Read<byte>(i, "Equ_LeftHand");
            Equ_RingA = pResult.Read<byte>(i, "Equ_RingA");
            Equ_Pant = pResult.Read<byte>(i, "Equ_Pant");
            Equ_RingB = pResult.Read<byte>(i, "Equ_RingB");
            Equ_Boot = pResult.Read<byte>(i, "Equ_Boot");
            Equ_AccBoot = pResult.Read<byte>(i, "Equ_AccBoot");
            Equ_AccPant = pResult.Read<byte>(i, "Equ_AccPant");
            Equ_AccBody = pResult.Read<byte>(i, "Equ_AccBody");
            Equ_AccMouth = pResult.Read<byte>(i, "Equ_AccMouth");
            Equ_AccEye = pResult.Read<byte>(i, "Equ_AccEye");
            Equ_AccHead = pResult.Read<byte>(i, "Equ_AccHead");
            Equ_AccLeftHand = pResult.Read<byte>(i, "Equ_AccLeftHand");
            Equ_AccRightHand = pResult.Read<byte>(i, "Equ_AccRightHand");
            Equ_AccBack = pResult.Read<byte>(i, "Equ_AccBack");
            Equ_CosEff = pResult.Read<byte>(i, "Equ_CosEff");
            Equ_AccHip = pResult.Read<byte>(i, "Equ_AccHip");
            Equ_MiniMon = pResult.Read<byte>(i, "Equ_MiniMon");
            Equ_MiniMon_R = pResult.Read<byte>(i, "Equ_MiniMon_R");
            Equ_AccShield = pResult.Read<byte>(i, "Equ_AccShield");
            Equ_Bracelet = pResult.Read<byte>(i, "Equ_Bracelet");
        }
    }
}
