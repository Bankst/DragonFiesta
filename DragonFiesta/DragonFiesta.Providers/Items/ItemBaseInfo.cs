using DragonFiesta.Providers.Characters;
using DragonFiesta.Providers.Items.SHN;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonFiesta.Providers.Items
{
    public class ItemBaseInfo
    {
        public ushort ID { get; private set; }
        public ItemType Type { get; private set; }
        public ItemClass Class { get; private set; }
        public uint MaxLot { get; private set; }
        public bool IsWeapon { get { return ((EquipSlot == ItemEquipSlot.ITEMEQUIP_LEFTHAND || EquipSlot == ItemEquipSlot.ITEMEQUIP_RIGHTHAND) && Class != ItemClass.ITEMCLASS_COSSHIELD); } }
        public bool IsRing { get { return (EquipSlot == ItemEquipSlot.ITEMEQUIP_LEFTRING || EquipSlot == ItemEquipSlot.ITEMEQUIP_RIGHTRING); } }
        public bool IsEquippable { get { return (EquipSlot != ItemEquipSlot.ITEMEQUIP_NONE); } }
        public ItemEquipSlot EquipSlot { get; private set; }
        public bool IsTwoHand { get; private set; }
        public TimeSpan AttackSpeed { get; private set; }
        public byte RequiredLevel { get; private set; }

        //  public StatsHolder Stats { get; private set; }
        public WhoEquip WhoEquip { get; private set; }

        public uint BuyPrice { get; private set; }
        public uint SellPrice { get; private set; }
        public uint BuyFame { get; private set; }
        public WeaponType WeaponType { get; private set; }
        public bool IsBelonged { get; private set; }
        public bool NoDrop { get; private set; }
        public bool NoSell { get; private set; }
        public bool NoStorage { get; private set; }
        public bool NoTrade { get; private set; }
        public bool NoDelete { get; private set; }
        public byte UpgradeLimit { get; private set; }
        public double UpgradeSuccessRate { get; private set; }
        public double UpgradeLuckRate { get; private set; }
        public UpgradeResource UpgradeResource { get; private set; }
        public List<UpgradeInfo> UpgradeInfos { get; protected set; }
        public ItemGradeType GradeType { get; private set; }
        public bool IsAutoPickup { get; private set; }


		public ItemBaseInfo(ItemInfo itemInfo, SecureCollection<BelongTypeInfo> belongTypeInfos)
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

			// BT_Inx stuff
			var btData = belongTypeInfos.Where(x => x.BT_Inx == itemInfo.BT_Inx).FirstOrDefault();
			IsBelonged = btData.PutOnBelonged;
			NoDrop = btData.NoDrop;
			NoSell = btData.NoSell;
			NoStorage = btData.NoStorage;
			NoTrade = btData.NoTrade;
			NoDelete = btData.NoDelete;

			UpgradeLimit = itemInfo.UpLimit;
			UpgradeSuccessRate = itemInfo.UpSucRatio;
			UpgradeLuckRate = itemInfo.UpLuckRatio;
			UpgradeResource = (UpgradeResource)itemInfo.UpResource;
			UpgradeInfos = null; // TODO: wtf is this
			GradeType = (ItemGradeType)itemInfo.Grade;
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