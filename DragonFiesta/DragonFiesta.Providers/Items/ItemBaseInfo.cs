using DragonFiesta.Providers.Characters;
using DragonFiesta.Providers.Items.SHN;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonFiesta.Providers.Items
{
    public class ItemBaseInfo
    {
        public ushort ID { get; }
        public ItemType Type { get; }
        public ItemClass Class { get; }
        public uint MaxLot { get; }
        public bool IsWeapon => ((EquipSlot == ItemEquipSlot.ITEMEQUIP_LEFTHAND || EquipSlot == ItemEquipSlot.ITEMEQUIP_RIGHTHAND) && Class != ItemClass.ITEMCLASS_COSSHIELD);
	    public bool IsRing => (EquipSlot == ItemEquipSlot.ITEMEQUIP_LEFTRING || EquipSlot == ItemEquipSlot.ITEMEQUIP_RIGHTRING);
	    public bool IsEquippable => (EquipSlot != ItemEquipSlot.ITEMEQUIP_NONE);
	    public ItemEquipSlot EquipSlot { get; }
        public bool IsTwoHand { get; }
        public TimeSpan AttackSpeed { get; }
        public byte RequiredLevel { get; }

        public StatsHolder Stats { get; }
        public WhoEquip WhoEquip { get; private set; }

        public uint BuyPrice { get; }
        public uint SellPrice { get; }
        public uint BuyFame { get; }
        public WeaponType WeaponType { get; }
        public bool IsBelonged { get; }
        public bool NoDrop { get; }
        public bool NoSell { get; }
        public bool NoStorage { get; }
        public bool NoTrade { get; }
        public bool NoDelete { get; }
        public byte UpgradeLimit { get; }
        public double UpgradeSuccessRate { get; }
        public double UpgradeLuckRate { get; }
        public UpgradeResource UpgradeResource { get; }
        public List<ItemUpgradeInfo> UpgradeInfos { get; protected set; }
        public ItemGradeType GradeType { get; }
        public bool IsAutoPickup { get; }


		public ItemBaseInfo(ItemInfo itemInfo, List<ItemUpgradeInfo> upgradeInfos, BelongTypeInfo belongTypeInfo, GradeItemOption gradeItemOption)
		{
			ID = itemInfo.ID;
			Type = (ItemType)itemInfo.Type;
			Class = (ItemClass)itemInfo.Class;
			MaxLot = itemInfo.MaxLot;
			EquipSlot = (ItemEquipSlot)itemInfo.Equip;
			IsTwoHand = itemInfo.TwoHand;
			AttackSpeed = TimeSpan.FromMilliseconds(itemInfo.AtkSpeed);
			RequiredLevel = itemInfo.DemandLv;
			BuyPrice = itemInfo.BuyPrice;
			SellPrice = itemInfo.SellPrice;
			BuyFame = itemInfo.BuyFame;
			WeaponType = (WeaponType)itemInfo.WeaponType;

			Stats = new StatsHolder();
			Stats.Evasion = (int) itemInfo.TB;
			Stats.WeaponDamage = new MinMax<int>((int) itemInfo.MinWC, (int) itemInfo.MaxWC);
			Stats.WeaponDefense = (int) itemInfo.AC;
			Stats.MagicDamage = new MinMax<int>((int) itemInfo.MinMA, (int) itemInfo.MaxMA);
			Stats.MagicDefense = (int) itemInfo.MR;
			Stats.CriticalRate = (int) itemInfo.CriRate;
			Stats.CriticalWeaponDamage = new MinMax<int>((int) itemInfo.CriMinWc, (int) itemInfo.CriMaxWc);
			Stats.CriticalMagicDamage = new MinMax<int>((int) itemInfo.CriMinMa, (int) itemInfo.CriMaxMa);
			Stats.Aim = (int) itemInfo.TH;

			if (gradeItemOption != null)
			{
				Stats.MaxHP = (int) gradeItemOption.MaxHP;
				Stats.MaxSP = (int) gradeItemOption.MaxSP;
			}

			// BT_Inx stuff
			var btData = belongTypeInfo;
			IsBelonged = btData.PutOnBelonged;
			NoDrop = btData.NoDrop;
			NoSell = btData.NoSell;
			NoStorage = btData.NoStorage;
			NoTrade = btData.NoTrade;
			NoDelete = btData.NoDelete;

			UpgradeLimit = itemInfo.UpLimit;
			UpgradeSuccessRate = itemInfo.UpSucRatio;
			UpgradeLuckRate = itemInfo.UpLuckRatio;
			UpgradeResource = (UpgradeResource) itemInfo.UpResource;
			UpgradeInfos = upgradeInfos; // TODO: wtf is this
			GradeType = (ItemGradeType) itemInfo.Grade;
			IsAutoPickup = btData.BT_Inx == 0 || btData.BT_Inx == 1 || btData.BT_Inx == 10;
		}

		/* New Stuff
        public uint ItemActionGroup { get; private set; }
        public uint BuyGToken { get; private set; }
        public uint BuyGBCoin { get; private set; }
        */
		/*
        public ItemBaseInfo(int ItemID, SecureCollection<ItemInfo> itemInfos, SecureCollection<ItemInfoServer> itemInfoServers, SecureCollection<BT_Inx> btInx)
        {
			Type = 1;
            Class = (ItemClass)pResult.Read<int>(row, "Class");
            MaxLot = pResult.Read<uint>(row, "MaxLot");
            EquipSlot = (ItemEquipSlot)pResult.Read<uint>(row, "Equip");
            IsTwoHand = pResult.Read<bool>(row, "TwoHand");
            AttackSpeed = TimeSpan.FromMilliseconds(pResult.Read<int>(row, "AtkSpeed"));
            RequiredLevel = pResult.Read<byte>(row, "DemandLv");
           
            WhoEquip = new WhoEquip(pResult.Read<uint>(row, "WhoEquip"));
            BuyPrice = pResult.Read<uint>(row, "BuyPrice");
            SellPrice = pResult.Read<uint>(row, "SellPrice");
            BuyFame = pResult.Read<uint>(row, "BuyFame");
            WeaponType = (WeaponType)pResult.Read<uint>(row, "WeaponType");
            IsBelonged = pResult.Read<bool>(row, "Belonged");
            NoDrop = pResult.Read<bool>(row, "NoDrop");
            NoSell = pResult.Read<bool>(row, "NoSell");
            NoStorage = pResult.Read<bool>(row, "NoStorage");
            NoTrade = pResult.Read<bool>(row, "NoTrade");
            NoDelete = pResult.Read<bool>(row, "NoDelete");
            UpgradeLimit = pResult.Read<byte>(row, "UpLimit");
            UpgradeSuccessRate = pResult.Read<ushort>(row, "UpSucRatio");
            UpgradeLuckRate = pResult.Read<ushort>(row, "UpLuckRatio");
            UpgradeResource = (UpgradeResource)pResult.Read<byte>(row, "UpResource");
            GradeType = (ItemGradeType)pResult.Read<uint>(row, "ItemGradeType");
            IsAutoPickup = pResult.Read<bool>(row, "AutoMon");
        }


        /*
        public ItemBaseInfo(SQLResult pResult,int i)
        {
            ID = pResult.Read<ushort>(i, "ID");
            Type = pResult.Read<uint>(i, "Type");
            Class = (ItemClass)pResult.Read<int>(i, "Class");
            MaxLot = pResult.Read<uint>(i, "MaxLot");
            EquipSlot = (ItemEquipSlot)pResult.Read<uint>(i, "Equip");
            TwoHand = pResult.Read<bool>(i, "TwoHand");
            AttackSpeed = TimeSpan.FromMilliseconds(pResult.Read<int>(i, "AtkSpeed"));
            RequiredLevel = pResult.Read<byte>(i, "DemandLv");
            Stats = new StatsHolder()
            {
                MaxHP = pResult.Read<int>(i, "MaxHP"),
                MaxSP = pResult.Read<int>(i, "MaxSP"),
                Aim = pResult.Read<int>(i, "TH"),
                Evasion = pResult.Read<int>(i, "TB"),
                WeaponDamage = new MinMax<int>(pResult.Read<int>(i, "MinWC"), pResult.Read<int>(i, "MaxWC")),
                WeaponDefense = (int)pResult.Read<int>(i, "AC"),
                MagicDamage = new MinMax<int>(pResult.Read<int>(i, "MinMA"), pResult.Read<int>(i, "MaxMA")),
                MagicDefense = pResult.Read<int>(i, "MR"),
                CriticalRate = pResult.Read<int>(i, "CriRate"),
                CriticalWeaponDamage = new MinMax<int>(pResult.Read<int>(i, "CriMinWc"), pResult.Read<int>(i, "CriMaxWc")),
                CriticalMagicDamage = new MinMax<int>(pResult.Read<int>(i, "CriMinMa"), pResult.Read<int>(i, "CriMaxMa")),
            };
            UseClass = pResult.Read<uint>(i, "UseClass");
            BuyPrice = pResult.Read<uint>(i, "BuyPrice");
            SellPrice = pResult.Read<uint>(i, "SellPrice");
            BuyFame = pResult.Read<uint>(i, "BuyFame");
            BuyGToken = pResult.Read<uint>(i, "BuyGToken");
            BuyGBCoin = pResult.Read<uint>(i, "BuyGBCoin");
            WeaponType = (WeaponType)pResult.Read<uint>(i, "WeaponType");
            UpgradeLimit = pResult.Read<byte>(i, "UpLimit");
            UpgradeSuccessRate = pResult.Read<ushort>(i, "UpSucRatio");
            UpgradeLuckRate = pResult.Read<ushort>(i,"UpLuckRatio");
            UpgradeResource = (UpgradeResource)pResult.Read<byte>(i, "UpResource");
            GradeType = (ItemGradeType)pResult.Read<uint>(i, "ItemGradeType");
        }
        */
	}
}