using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Providers.Items.SHN
{
    public class ItemInfo
	{
		public ushort ID { get; }
		public string InxName { get; } // ref - NPCItemLists, GradeItemOption
		public string Name { get; }
		public uint Type { get; }
		public uint Class { get; }
		public uint MaxLot { get; }
		public uint Equip { get; }
		public byte ItemAuctionGroup { get; }
		public byte ItemGradeType { get; }
		public bool TwoHand { get; }
		public int AtkSpeed { get; }
		public byte DemandLv { get; }
		public uint Grade { get; }
		public uint MinWC { get; }
		public uint MaxWC { get; }
		public uint AC { get; }
		public uint MinMA { get; }
		public uint MaxMA { get; }
		public uint MR { get; }
		public uint TH { get; }
		public uint TB { get; }
		public uint WCRate { get; }
		public uint MARate { get; }
		public uint ACRate { get; }
		public uint MRRate { get; }	
		public uint CriRate { get; }
		public uint CriMinWc { get; }
		public uint CriMaxWc { get; }
		public uint CriMinMa { get; }
		public uint CriMaxMa { get; }
		public uint CrlTB { get; }
		public uint UseClass { get; }
		public uint BuyPrice { get; }
		public uint SellPrice { get; }
		public byte BuyDemandLv { get; }
		public uint BuyFame { get; }
		public uint BuyGToken { get; }
		public uint BuyGBCoin { get; }
		public uint WeaponType { get; }
		public uint ArmorType { get; }
		public byte UpLimit { get; }
		public ushort BasicUpInx { get; } // ref - UpgradeInfo
		public double UpSucRatio { get; }
		public double UpLuckRatio { get; }
		public byte UpResource { get; }
		public byte AddUpInx { get; }
		public uint ShieldAC { get; }
		public byte HitRatePlus { get; }
		public byte EvaRatePlus { get; }
		public byte MACriPlus { get; }
		public byte CriDamPlus { get; }
		public byte MagCriDamPlus { get; }
		public byte BT_Inx { get; } // ref - BelongTypeInfo
		public string TitleName { get; }
		public string ItemUseSkill { get; }
		public string SetItemIndex { get; }
		public byte ItemFunc { get; }
		
		public ItemInfo(SHNResult pResult, int i)
		{
			ID = pResult.Read<ushort>(i, "ID");
			InxName = pResult.Read<string>(i, "InxName");
			Name = pResult.Read<string>(i, "Name");
			Type = pResult.Read<uint>(i, "Type");
			Class = pResult.Read<uint>(i, "Class");
			MaxLot = pResult.Read<uint>(i, "MaxLot");
			Equip = pResult.Read<uint>(i, "Equip");
			ItemAuctionGroup = pResult.Read<byte>(i, "ItemAuctionGroup");
			ItemGradeType = pResult.Read<byte>(i, "ItemGradeType");
			TwoHand = pResult.Read<bool>(i, "TwoHand");
			AtkSpeed = pResult.Read<int>(i, "AtkSpeed");
			DemandLv = pResult.Read<byte>(i, "DemandLv");
			Grade = pResult.Read<uint>(i, "Grade");
			MinWC = pResult.Read<uint>(i, "MinWC");
			MaxWC = pResult.Read<uint>(i, "MaxWC");
			AC = pResult.Read<uint>(i, "AC");
			MinMA = pResult.Read<uint>(i, "MinMA");
			MaxMA = pResult.Read<uint>(i, "MaxMA");
			MR = pResult.Read<uint>(i, "MR");
			TH = pResult.Read<uint>(i, "TH");
			TB = pResult.Read<uint>(i, "TB");
			WCRate = pResult.Read<uint>(i, "WCRate");
			MARate = pResult.Read<uint>(i, "MARate");
			ACRate = pResult.Read<uint>(i, "ACRate");
			MRRate = pResult.Read<uint>(i, "MRRate");
			CriRate = pResult.Read<uint>(i, "CriRate");
			CriMinWc = pResult.Read<uint>(i, "CriMinWc");
			CriMaxWc = pResult.Read<uint>(i, "CriMaxWc");
			CriMinMa = pResult.Read<uint>(i, "CriMinMa");
			CriMaxMa = pResult.Read<uint>(i, "CriMaxMa");
			CrlTB = pResult.Read<uint>(i, "CrlTB");
			UseClass = pResult.Read<uint>(i, "UseClass");
			BuyPrice = pResult.Read<uint>(i, "BuyPrice");
			SellPrice = pResult.Read<uint>(i, "SellPrice");
			BuyDemandLv = pResult.Read<byte>(i, "BuyDemandLv");
			BuyFame = pResult.Read<uint>(i, "BuyFame");
			BuyGToken = pResult.Read<uint>(i, "BuyGToken");
			BuyGBCoin = pResult.Read<uint>(i, "BuyGBCoin");
			WeaponType = pResult.Read<uint>(i, "WeaponType");
			ArmorType = pResult.Read<uint>(i, "ArmorType");
			UpLimit = pResult.Read<byte>(i, "UpLimit");
			BasicUpInx = pResult.Read<ushort>(i, "BasicUpInx");
			UpSucRatio = pResult.Read<ushort>(i, "UpSucRatio");
			UpLuckRatio = pResult.Read<ushort>(i, "UpLuckRatio");
			UpResource = pResult.Read<byte>(i, "UpResource");
			AddUpInx = pResult.Read<byte>(i, "AddUpInx");
			ShieldAC = pResult.Read<uint>(i, "ShieldAC");
			HitRatePlus = pResult.Read<byte>(i, "HitRatePlus");
			EvaRatePlus = pResult.Read<byte>(i, "EvaRatePlus");
			MACriPlus = pResult.Read<byte>(i, "MACriPlus");
			CriDamPlus = pResult.Read<byte>(i, "CriDamPlus");
			MagCriDamPlus = pResult.Read<byte>(i, "MagCriDamPlus");
			BT_Inx = pResult.Read<byte>(i, "BT_Inx");
			TitleName = pResult.Read<string>(i, "TitleName");
			ItemUseSkill = pResult.Read<string>(i, "ItemUseSkill");
			SetItemIndex = pResult.Read<string>(i, "SetItemIndex");
			ItemFunc = pResult.Read<byte>(i, "ItemFunc");
		}
	}
}
