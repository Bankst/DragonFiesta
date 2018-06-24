using DragonFiesta.Providers.Characters;
using System;
using System.Collections.Generic;

namespace DragonFiesta.Providers.Items
{
    public abstract class ItemBaseInfo
    {
        public ushort ID { get; private set; }
        public uint Type { get; private set; }
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
        public List<ItemUpgradeInfo> UpgradeInfos { get; protected set; }
        public ItemGradeType GradeType { get; private set; }
        public bool IsAutoPickup { get; private set; }

        /* New Stuff
        public uint ItemActionGroup { get; private set; }
        public uint BuyGToken { get; private set; }
        public uint BuyGBCoin { get; private set; }
        */

        public ItemBaseInfo(SQLResult pResult, int i)
        {
            ID = pResult.Read<ushort>(i, "ID");
            Type = pResult.Read<uint>(i, "Type");
            Class = (ItemClass)pResult.Read<int>(i, "Class");
            MaxLot = pResult.Read<uint>(i, "MaxLot");
            EquipSlot = (ItemEquipSlot)pResult.Read<uint>(i, "Equip");
            IsTwoHand = pResult.Read<bool>(i, "TwoHand");
            AttackSpeed = TimeSpan.FromMilliseconds(pResult.Read<int>(i, "AtkSpeed"));
            RequiredLevel = pResult.Read<byte>(i, "DemandLv");
            /*Stats = new StatsHolder()
            {
                MaxHP = pResult.Read<int>(i, "MaxHP"),
                MaxSP = pResult.Read<int>(i, "MaxSP"),
                Aim = pResult.Read<int>(i, "TH"),
                Evasion = pResult.Read<int>(i, "TB"),
                WeaponDamage = new MinMax<int>(pResult.Read<int>(i, "MinWC"), pResult.Read<int>(i, "MaxWC")),
                WeaponDefense = pResult.Read<int>(i, "AC"),
                MagicDamage = new MinMax<int>(pResult.Read<int>(i, "MinMA"), pResult.Read<int>(i, "MaxMA")),
                MagicDefense = pResult.Read<int>(i, "MR"),
                CriticalRate = pResult.Read<int>(i, "CriRate"),
                CriticalWeaponDamage = new MinMax<int>(pResult.Read<int>(i, "CriMinWc"), pResult.Read<int>(i, "CriMaxWc")),
                CriticalMagicDamage = new MinMax<int>(pResult.Read<int>(i, "CriMinMa"), pResult.Read<int>(i, "CriMaxMa")),
            };*/
            WhoEquip = new WhoEquip(pResult.Read<uint>(i, "WhoEquip"));
            BuyPrice = pResult.Read<uint>(i, "BuyPrice");
            SellPrice = pResult.Read<uint>(i, "SellPrice");
            BuyFame = pResult.Read<uint>(i, "BuyFame");
            WeaponType = (WeaponType)pResult.Read<uint>(i, "WeaponType");
            IsBelonged = pResult.Read<bool>(i, "Belonged");
            NoDrop = pResult.Read<bool>(i, "NoDrop");
            NoSell = pResult.Read<bool>(i, "NoSell");
            NoStorage = pResult.Read<bool>(i, "NoStorage");
            NoTrade = pResult.Read<bool>(i, "NoTrade");
            NoDelete = pResult.Read<bool>(i, "NoDelete");
            UpgradeLimit = pResult.Read<byte>(i, "UpLimit");
            UpgradeSuccessRate = pResult.Read<ushort>(i, "UpSucRatio");
            UpgradeLuckRate = pResult.Read<ushort>(i, "UpLuckRatio");
            UpgradeResource = (UpgradeResource)pResult.Read<byte>(i, "UpResource");
            GradeType = (ItemGradeType)pResult.Read<uint>(i, "ItemGradeType");
            IsAutoPickup = pResult.Read<bool>(i, "AutoMon");
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