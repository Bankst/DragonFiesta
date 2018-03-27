using System.Collections.Generic;

namespace DragonFiesta.Providers.Items
{
    public sealed class ItemUpgradeInfo
    {
        public ushort ID { get; private set; }

        public ItemUpgradeFactor UpgradeFactor { get; private set; }
        public Dictionary<byte, short> UpgradeData { get; private set; }

        public ItemUpgradeInfo(SQLResult pResult, int i)
        {
            ID = pResult.Read<ushort>(i, "ID");
            UpgradeFactor = (ItemUpgradeFactor)pResult.Read<ushort>(i, "UpFactor");
            UpgradeData = new Dictionary<byte, short>();
            for (int i2 = 0; i2 < 11; i2++)
            {
                string colname = "State" + i2;
                UpgradeData.Add((byte)(i2 + 1), pResult.Read<short>(i, colname));
            }
        }
    }
}