using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Zone.Data.CollectCard
{
    public sealed class CollectCardStarRate
    {
        public uint CardGradeType { get; }

        public byte StarLot { get; }

        public ushort StarRate { get; }

        public CollectCardStarRate(SHNResult pResult, int i)
        {
            CardGradeType = pResult.Read<uint>(i, "CC_CardGradeType");
            StarLot = pResult.Read<byte>(i, "CC_StarLot");
            StarRate = pResult.Read<ushort>(i, "CC_StarRate");
        }
    }
}
