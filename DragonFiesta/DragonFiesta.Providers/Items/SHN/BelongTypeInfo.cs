using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Providers.Items.SHN
{
    public class BelongTypeInfo
	{
		public byte BT_Inx { get; }
		public bool PutOnBelonged { get; }
		public bool NoDrop { get; }
		public bool NoSell { get; }
		public bool NoStorage { get; }
		public bool NoTrade { get; }
		public bool NoDelete { get; }

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
