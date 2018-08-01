using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Zone.Data.CollectCard
{
    public sealed class CollectCardDropRate
    {
        public ushort CardID { get; }

        public ushort CardGetRate { get; }

        public CollectCardDropRate(SHNResult pResult, int i)
        {
            CardID = pResult.Read<ushort>(i, "CC_CardID");
            CardGetRate = pResult.Read<ushort>(i, "CC_CardGetRate");
        }
    }
}
