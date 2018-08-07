using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.World.Data.Guild
{
    public class GuildGradeData
    {
        public byte Type { get; }

        public uint NeedFame { get; }

        public ushort MaxOfMember { get; }

        public short MaxOfGradeMember { get; }

        public GuildGradeData(SHNResult pResult, int i)
        {
            Type = pResult.Read<byte>(i, "Type");
            NeedFame = pResult.Read<uint>(i, "NeedFame");
            MaxOfMember = pResult.Read<ushort>(i, "MaxOfMember");
            MaxOfGradeMember = pResult.Read<short>(i, "MaxOfGradeMember");
        }
    }
}
