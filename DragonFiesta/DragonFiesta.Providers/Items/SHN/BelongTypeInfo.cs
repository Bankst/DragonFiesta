using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Providers.Items.SHN
{
    public class BelongTypeInfo
	{
		public byte BT_Inx { get; private set; }
		public bool PutOnBelonged { get; private set; }
		public bool NoDrop { get; private set; }
		public bool NoSell { get; private set; }
		public bool NoStorage { get; private set; }
		public bool NoTrade { get; private set; }
		public bool NoDelete { get; private set; }

		public BelongTypeInfo(SHNResult pResult, int row)
		{
			BT_Inx = pResult.Read<byte>(row, "BT_Inx");
			PutOnBelonged = pResult.Read<bool>(row, "PutOnBelonged");
			NoDrop = pResult.Read<bool>(row, "NoDrop");
			NoSell = pResult.Read<bool>(row, "NoSell");
			NoStorage = pResult.Read<bool>(row, "NoStorage");
			NoTrade = pResult.Read<bool>(row, "NoTrade");
			NoDelete = pResult.Read<bool>(row, "NoDelete");
		}
	}
}
