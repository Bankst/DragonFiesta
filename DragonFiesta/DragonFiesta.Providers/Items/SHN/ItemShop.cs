using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Providers.Items.SHN
{
    public class ItemShop
    {
        public uint goodsNo { get; }

        public string InxName { get; }

        public uint Lot { get; }

        public ItemShop(SHNResult pResult, int i)
        {
            goodsNo = pResult.Read<uint>(i, "goodsNo");
            InxName = pResult.Read<string>(i, "InxName");
            Lot = pResult.Read<uint>(i, "Lot");
        }
    }
}
