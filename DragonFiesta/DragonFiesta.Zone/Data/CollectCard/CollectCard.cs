using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Zone.Data.CollectCard
{
    public sealed class CollectCard
    {
        public ushort CardID { get; }

        public string ItemInx { get; }

        public uint CardGradeType { get; }

        public uint CardMobGroup { get; }


        public CollectCard(SHNResult pResult, int i)
        {
            CardID = pResult.Read<ushort>(i, "CC_CardID");
            ItemInx = pResult.Read<string>(i, "CC_ItemInx");
            CardGradeType = pResult.Read<uint>(i, "CC_CardGradeType");
            CardMobGroup = pResult.Read<uint>(i, "CC_CardMobGroup");
        }
    }
}
