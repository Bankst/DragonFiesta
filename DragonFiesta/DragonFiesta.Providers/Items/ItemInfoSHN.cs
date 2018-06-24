using System;
using System.Linq;
using System.Reflection;
using System.Text;
using DragonFiesta.Providers.Characters;
using DragonFiesta.Utils.IO.CS;
using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Providers.Items
{
	public class ItemInfoSHN
	{
		public ushort ID { get; private set; }
		public string InxName { get; private set; }
		public string Name { get; private set; }
		public uint Type { get; private set; }
		public ItemClass Class { get; private set; }
		public uint MaxLot { get; private set; }
		public ItemEquipSlot Equip { get; private set; } // type:ItemEquipSlot, EquipSlot
		public byte ItemAuctionGroup { get; private set; }
		public bool TwoHand { get; private set; }
		public TimeSpan AtkSpeed { get; private set; } // type:TimeSpan
		public byte DemandLv { get; private set; } // RequiredLevel
		public byte Grade { get; private set; }
		public int MinWC { get; private set; }
		public int MaxWC { get; private set; }
		public int AC { get; private set; }
		public int MinMA { get; private set; }
		public int MaxMA { get; private set; }
		public int MR { get; private set; }
		public int TH { get; private set; }
		public int WCRate { get; private set; }
		public int MARate { get; private set; }
		public int ACRate { get; private set; }
		public int MRRate { get; private set; }
		public int CriRate { get; private set; }
		public int CriMinWc { get; private set; }
		public int CriMaxWc { get; private set; }
		public int CriMinMa { get; private set; }
		public int CriMaxMa { get; private set; }
		public int CrITB { get; private set; }
		public WhoEquip UseClass { get; private set; } // WhoEquip
		public int BuyPrice { get; private set; }
		public int SellPrice { get; private set; }
		public byte BuyDemandLv { get; private set; }
		public int BuyFame { get; private set; }
		public int BuyGToken { get; private set; }
		public int BuyGBCoin { get; private set; }
		public byte WeaponType { get; private set; }
		public byte ArmorType { get; private set; }
		public byte UpLimit { get; private set; }
		public int BasicUpInx { get; private set; }
		public int UpSucRatio { get; private set; }
		public int UpLuckRatio { get; private set; }
		public byte UpResource { get; private set; }
		public byte AddUpInx { get; private set; }
		public int ShieldAC { get; private set; }
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

		public ItemInfoSHN()
		{
			
		}
	}
}
