using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Providers.Items.SHN
{
    public class ItemMerchantInfo
    {
        public string Map { get; }

        public byte Sub { get; }

        public ItemMerchantInfo(SHNResult pResult, int i)
        {
            Map = pResult.Read<string>(i, "Map");
            Sub = pResult.Read<byte>(i, "Sub");
        }
    }
}
