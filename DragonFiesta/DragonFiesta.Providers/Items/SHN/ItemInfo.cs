using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Providers.Items
{
    public class ItemInfo
	{
		public ushort ID { get; private set; }
		public string InxName { get; private set; }
		public string Name { get; private set; }
		public uint Type { get; private set; }
		public uint Class { get; private set; }
		public uint MaxLot { get; private set; }
		public uint Equip { get; private set; } // type:ItemEquipSlot, EquipSlot
		public byte ItemAuctionGroup { get; private set; }
		public byte ItemGradeType { get; private set; }
		public bool TwoHand { get; private set; }
		public int AtkSpeed { get; private set; } // type:TimeSpan
		public byte DemandLv { get; private set; } // RequiredLevel
		public uint Grade { get; private set; }
		public uint MinWC { get; private set; }
		public uint MaxWC { get; private set; }
		public uint AC { get; private set; }
		public uint MinMA { get; private set; }
		public uint MaxMA { get; private set; }
		public uint MR { get; private set; }
		public uint TH { get; private set; } // Aim
		public uint TB { get; private set; }
		public uint WCRate { get; private set; } // ??
		public uint MARate { get; private set; } // ??
		public uint ACRate { get; private set; } // ??
		public uint MRRate { get; private set; } // ??
		public uint CriRate { get; private set; } // CriticalRate
		public uint CriMinWc { get; private set; } // 
		public uint CriMaxWc { get; private set; }
		public uint CriMinMa { get; private set; }
		public uint CriMaxMa { get; private set; }
		public uint CrlTB { get; private set; }
		public uint UseClass { get; private set; } // WhoEquip
		public uint BuyPrice { get; private set; }
		public uint SellPrice { get; private set; }
		public byte BuyDemandLv { get; private set; }
		public uint BuyFame { get; private set; }
		public uint BuyGToken { get; private set; }
		public uint BuyGBCoin { get; private set; }
		public uint WeaponType { get; private set; }
		public uint ArmorType { get; private set; }
		public byte UpLimit { get; private set; }
		public uint BasicUpInx { get; private set; }
		public double UpSucRatio { get; private set; }
		public double UpLuckRatio { get; private set; }
		public byte UpResource { get; private set; }
		public byte AddUpInx { get; private set; }
		public uint ShieldAC { get; private set; }
		public byte HitRatePlus { get; private set; }
		public byte EvaRatePlus { get; private set; }
		public byte MACriPlus { get; private set; }
		public byte CriDamPlus { get; private set; }
		public byte MagCriDamPlus { get; private set; }
		public byte BT_Inx { get; private set; }
		public string TitleName { get; private set; }
		public string ItemUseSkill { get; private set; }
		public string SetItemIndex { get; private set; }
		public byte ItemFunc { get; private set; }
		
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
			BasicUpInx = pResult.Read<uint>(i, "BasicUpInx");
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
