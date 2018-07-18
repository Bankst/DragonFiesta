using System.Collections.Generic;
using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Providers.Items.SHN
{
    public class ItemUpgradeInfo
	{
		public ushort ID { get; private set; }
		public string InxName { get; private set; }
		public ItemUpgradeFactor UpFactor { get; private set; }
		public Dictionary<byte, short> UpgradeData { get; private set; }

		public ItemUpgradeInfo(SHNResult pResult, int row)
		{
			ID = pResult.Read<ushort>(row, "ID");
			InxName = pResult.Read<string>(row, "InxName");
			UpFactor = (ItemUpgradeFactor)pResult.Read<uint>(row, "UpFactor");
			UpgradeData = new Dictionary<byte, short>();
			for (var i = 0; i < 11; i++)
			{
				var colname = i == 0 ? "Updata" : $"Unknown: {i}";
				UpgradeData.Add((byte)(i + 1), pResult.Read<short>(row, colname));
			}
		}
	}
}
