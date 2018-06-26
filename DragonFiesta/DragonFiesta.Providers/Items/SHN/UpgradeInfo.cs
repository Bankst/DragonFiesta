using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Providers.Items.SHN
{
	public class UpgradeInfo
	{
		public ushort ID { get; private set; }
		public string InxName { get; private set; }
		public ItemUpgradeFactor UpFactor { get; private set; }
		public Dictionary<byte, short> UpgradeData { get; private set; }

		public UpgradeInfo(SHNResult pResult, int row)
		{
			ID = pResult.Read<ushort>(row, "ID");
			InxName = pResult.Read<string>(row, "InxName");
			UpFactor = (ItemUpgradeFactor)pResult.Read<uint>(row, "UpFactor");
			UpgradeData = new Dictionary<byte, short>();
			for (int i = 0; i < 11; i++)
			{
				string colname;
				if (i == 0) colname = "Updata";
				else colname = $"Unknown: {i}";
				UpgradeData.Add((byte)(i + 1), pResult.Read<short>(row, colname));
			}
		}
	}
}
