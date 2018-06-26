using System.Collections.Generic;
using DragonFiesta.Providers.Items.SHN;

namespace DragonFiesta.Providers.Items
{
    public sealed class ItemUpgradeInfo
    {
        public ushort ID { get; private set; }

        public ItemUpgradeFactor UpgradeFactor { get; private set; }
        public Dictionary<byte, short> UpgradeData { get; private set; }

        public ItemUpgradeInfo(UpgradeInfo upInfo)
        {
			ID = upInfo.ID;
			UpgradeFactor = upInfo.UpFactor;
			UpgradeData = upInfo.UpgradeData;
        }
    }
}