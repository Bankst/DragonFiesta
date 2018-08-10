using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Zone.Data.MiniHouse
{
    public class MiniHouseObjAni
    {
        public ushort Handle { get; }

        public ushort ItemID { get; }

        public byte AniGroupIDMaxNum { get; }

        public ushort AniGroupID { get; }

        public uint EventCode { get; }

        public ushort NextAniHandle { get; }

        public byte ActorMaxNum { get; }

        public byte Actor01 { get; }

        public byte Actor02 { get; }

        public byte Actor03 { get; }

        public byte ActeeMaxNum { get; }

        public byte Actee01 { get; }

        public byte Actee02 { get; }

        public byte Actee03 { get; }

        public MiniHouseObjAni(SHNResult pResult, int row)
        {
            Handle = pResult.Read<ushort>(row, "Handle");
            ItemID = pResult.Read<ushort>(row, "ItemID");
            AniGroupIDMaxNum = pResult.Read<byte>(row, "AniGroupIDMaxNum");
            AniGroupID = pResult.Read<ushort>(row, "AniGroupID");
            EventCode = pResult.Read<uint>(row, "EventCode");
            NextAniHandle = pResult.Read<ushort>(row, "NextAniHandle");
            ActorMaxNum = pResult.Read<byte>(row, "ActorMaxNum");
            Actor01 = pResult.Read<byte>(row, "Actor01");
            Actor02 = pResult.Read<byte>(row, "Actor02");
            Actor03 = pResult.Read<byte>(row, "Actor03");
            ActeeMaxNum = pResult.Read<byte>(row, "ActeeMaxNum");
            Actee01 = pResult.Read<byte>(row, "Actee01");
            Actee02 = pResult.Read<byte>(row, "Actee02");
            Actee03 = pResult.Read<byte>(row, "Actee03");
        }
    }
}
