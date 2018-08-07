using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Zone.Data.CollectCard
{
    public sealed class CollectCardTitle
    {
        public uint Type { get; }

        public string ItemInx { get; }

        public CollectCardTitle(SHNResult pResult, int i)
        {
            Type = pResult.Read<uint>(i, "Type");
            ItemInx = pResult.Read<string>(i, "CC_ItemInx");
        }
    }
}
