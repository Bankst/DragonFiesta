using DragonFiesta.Providers.Characters;
using DragonFiesta.Providers.Items.SHN;
using System;
using System.Collections.Generic;

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

        public AuctionGroupType AuctionGroup { get; }
        public bool IsTwoHand { get; }
        public TimeSpan AttackSpeed { get; }
        public byte RequiredLevel { get; }

        public StatsHolder Stats { get; }
        public WhoEquip WhoEquip { get; private set; }
        public uint Grade { get; }
        public uint BuyPrice { get; }
        public uint SellPrice { get; }
        public uint BuyFame { get; }
        public uint UseClass { get; }
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
            AuctionGroup = (AuctionGroupType)itemInfo.ItemAuctionGroup;
            GradeType = (ItemGradeType)itemInfo.Grade;
            IsTwoHand = itemInfo.TwoHand;
			AttackSpeed = TimeSpan.FromMilliseconds(itemInfo.AtkSpeed);
			RequiredLevel = itemInfo.DemandLv;
            Grade = itemInfo.Grade;

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

            UseClass = itemInfo.UseClass; // TODO

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
			IsAutoPickup = btData.BT_Inx == 0 || btData.BT_Inx == 1 || btData.BT_Inx == 10;
		}
	}
}