using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Providers.Items
{
    public class ItemInfoServer
	{
		public ushort ID { get; private set; }
		public string InxName { get; private set; }
		public string MarketIndex { get; private set; }
		public byte City { get; private set; }
		public string DropGroupA { get; private set; }
		public string DropGroupB { get; private set; }
		public string RandomOptionDropGroup { get; private set; }
		public int Vanish { get; private set; }
		public int looting { get; private set; }
		public byte DropRateKilledByMob { get; private set; }
		public byte DropRateKilledByPlayer { get; private set; }
		public byte ISET_Index { get; private set; }
		public string ItemSort_Index { get; private set; }
		public byte KQItem { get; private set; }
		public byte PK_KQ_USE { get; private set; }
		public byte KQ_Item_Drop { get; private set; }
		public byte PreventAttack { get; private set; }

		public ItemInfoServer(SHNResult pResult, int row)
		{
			ID = pResult.Read<ushort>(row, "ID");
			InxName = pResult.Read<string>(row, "InxName");
			MarketIndex = pResult.Read<string>(row, "MarketIndex");
			City = pResult.Read<byte>(row, "City");
			DropGroupA = pResult.Read<string>(row, "DropGroupA");
			DropGroupB = pResult.Read<string>(row, "DropGroupB");
			RandomOptionDropGroup = pResult.Read<string>(row, "RandomOptionDropGroup");
			Vanish = pResult.Read<int>(row, "Vanish");
			looting = pResult.Read<int>(row, "looting");
			DropRateKilledByMob = pResult.Read<byte>(row, "DropRateKilledByMob");
			DropRateKilledByPlayer = pResult.Read<byte>(row, "DropRateKilledByPlayer");
			ISET_Index = pResult.Read<byte>(row, "ISET_Index");
			ItemSort_Index = pResult.Read<string>(row, "ItemSort_Index");
			KQItem = pResult.Read<byte>(row, "KQItem");
			PK_KQ_USE = pResult.Read<byte>(row, "PK_KQ_USE");
			KQ_Item_Drop = pResult.Read<byte>(row, "KQ_Item_Drop");
			PreventAttack = pResult.Read<byte>(row, "PreventAttack");
		}
	}
}
