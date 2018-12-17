using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

using DFEngine.Content.GameObjects;
using DFEngine.Content.GameObjects.Movers;
using DFEngine.Network;
using DFEngine.Utils;

namespace DFEngine.Content.Items
{
	public class ItemInstance : GameObject
	{
		public Item Item { get; set; }

		public byte Lot { get; set; }

		public Inventory Inventory { get; set; }

		public byte InventoryNo { get; set; }

		public long Key { get; set; }

		public GameObject Owner { get; set; }

		public byte TradeInventoryNo { get; set; }

		public byte Upgrades { get; set; }

//		public int DeleteTime => GetDeleteTime();

		public int RealDeleteTime { get; set; }

		public ushort CurrentMobID { get; set; }

		public byte CurrentKillLevel { get; set; }

		public byte RerollCount { get; set; }

		public byte UpgradeFailCount { get; set; }

		public List<WeaponLicense> Licenses { get; set; }

		public WeaponTitle Title { get; set; }

		public byte MaxSocketCount { get; set; }

		public List<Socket> Sockets { get; set; }

		public bool IsExpired => RealDeleteTime <= Time.GetTimeStamp(DateTime.UtcNow);

		public bool IsCard
		{
			get
			{
				if (Item.Info.Class != ItemClass.CLOSEDCARD)
					return Item.Info.Class == ItemClass.OPENCARD;
				return true;
			}
		}

		public int Serial { get; set; }

		public byte StarCount { get; set; }

		public ushort CardID { get; set; }

		public ushort CardGroup { get; set; }

		public ItemInstance(Item item)
		{
			Item = item;
			Licenses = new List<WeaponLicense>();
			Sockets = new List<Socket>();
			if (Item.Mover != null)
				Mount = new MoverInstance(item.Mover);
			Type = GameObjectType.DROPITEM;
		}

		public void GenerateSockets(List<int> rates)
		{
			if (Item.Info.Class != ItemClass.WEAPON)
				return;
			var num = Mathd.Random(rates.Sum(x => x));
			for (var upperBound = rates.GetUpperBound(); upperBound >= 0; --upperBound)
			{
				var rate = rates[upperBound];
				if (num < rate)
				{
					MaxSocketCount = (byte)Math.Max(0, upperBound);
					break;
				}
				num -= rate;
			}
		}

		// TODO: Item RandomOptions!
		/*
		public void GenerateRandomOptions()
		{
			Stats.BonusSTR = 0;
			Stats.BonusEND = 0;
			Stats.BonusDEX = 0;
			Stats.BonusINT = 0;
			Stats.BonusSPR = 0;
			Stats.BonusAim = 0;
			Stats.CriticalRate = Decimal.Zero;
			Stats.BonusMinDmg = 0;
			Stats.BonusMaxDmg = 0;
			Stats.BonusDef = 0;
			Stats.BonusMinMDmg = 0;
			Stats.BonusMaxMDmg = 0;
			Stats.BonusMDef = 0;
			Stats.BonusEvasion = 0;
			RandomOptionGroup randomOptionGroup = Item.RandomOptionGroup;
			if (randomOptionGroup == null)
				return;
			int num1 = Mathd.Rnd.Next(randomOptionGroup.OptionCount.Sum<RandomOptionCount>((Func<RandomOptionCount, int>)(optCount => optCount.LimitDropRate)));
			var num2 = 0;
			for (int upperBound = randomOptionGroup.OptionCount.UpperBound; upperBound >= 0; --upperBound)
			{
				RandomOptionCount randomOptionCount = randomOptionGroup.OptionCount[upperBound];
				if (num1 < randomOptionCount.LimitDropRate)
				{
					num2 = randomOptionCount.LimitCount;
					break;
				}
				num1 -= randomOptionCount.LimitDropRate;
			}
			if (num2 == 0)
				return;
			var num3 = 0;
			foreach (KeyValuePair<RandomOptionType, RandomOption> keyValuePair in (IEnumerable<KeyValuePair<RandomOptionType, RandomOption>>)randomOptionGroup.Options.OrderBy<KeyValuePair<RandomOptionType, RandomOption>, int>((Func<KeyValuePair<RandomOptionType, RandomOption>, int>)(x => Mathd.Rnd.Next())))
			{
				if (num3 == num2)
					break;
				if (Mathd.Odds((double)keyValuePair.Value.DropRate / 10.0))
				{
					++num3;
					int num4 = Mathd.Rnd.Next(keyValuePair.Value.Values.Min, keyValuePair.Value.Values.Max);
					switch (keyValuePair.Key)
					{
						case RandomOptionType.ROT_STR:
							Stats.BonusSTR = num4;
							continue;
						case RandomOptionType.ROT_CON:
							Stats.BonusEND = num4;
							continue;
						case RandomOptionType.ROT_DEX:
							Stats.BonusDEX = num4;
							continue;
						case RandomOptionType.ROT_INT:
							Stats.BonusINT = num4;
							continue;
						case RandomOptionType.ROT_MEN:
							Stats.BonusSPR = num4;
							continue;
						case RandomOptionType.ROT_TH:
							Stats.BonusAim = num4;
							continue;
						case RandomOptionType.ROT_CRI:
							Stats.CriticalRate = (Decimal)((double)num4 / 100.0);
							continue;
						case RandomOptionType.ROT_WC:
							Stats.BonusMinDmg = num4;
							Stats.BonusMaxDmg = num4;
							continue;
						case RandomOptionType.ROT_AC:
							Stats.BonusDef = num4;
							continue;
						case RandomOptionType.ROT_MA:
							Stats.BonusMinMDmg = num4;
							Stats.BonusMaxMDmg = num4;
							continue;
						case RandomOptionType.ROT_MR:
							Stats.BonusMDef = num4;
							continue;
						case RandomOptionType.ROT_TB:
							Stats.BonusEvasion = num4;
							continue;
						default:
							continue;
					}
				}
			}
		}
		*/

		// TODO: Charged items!
		/*
		public int GetDeleteTime()
		{
			if (RealDeleteTime <= 0)
				return 0;
			ushort? keepTimeHour = Item.ChargedEffect?.KeepTimeHour;
			var nullable = keepTimeHour.HasValue ? keepTimeHour.GetValueOrDefault() : new int?();
			var num = 0;
			if ((nullable.GetValueOrDefault() == num ? (nullable.HasValue ? 1 : 0) : 0) != 0)
				return 1992027391;
			return Time.UnixTimeStampToDateTime(RealDeleteTime).Shift(false);
		}
		*/

		public void SetOption(ItemOptionType optionType, long value)
		{
			switch (optionType)
			{
				case ItemOptionType.IOT_NUMBER:
					Lot = (byte)value;
					break;
				case ItemOptionType.IOT_STR:
					Stats.BonusSTR = (int)value;
					break;
				case ItemOptionType.IOT_CON:
					Stats.BonusEND = (int)value;
					break;
				case ItemOptionType.IOT_DEX:
					Stats.BonusDEX = (int)value;
					break;
				case ItemOptionType.IOT_INT:
					Stats.BonusINT = (int)value;
					break;
				case ItemOptionType.IOT_MEN:
					Stats.BonusSPR = (int)value;
					break;
				case ItemOptionType.IOT_UP:
					Upgrades = (byte)value;
					break;
				case ItemOptionType.IOT_DELTIME:
					RealDeleteTime = (int)value;
					break;
				case ItemOptionType.IOT_REROLLCOUNT:
					RerollCount = (byte)value;
					break;
				case ItemOptionType.IOT_TH:
					Stats.BonusAim = (int)value;
					break;
				case ItemOptionType.IOT_CRI:
					Stats.CriticalRate = (decimal)(value / 100.0);
					break;
				case ItemOptionType.IOT_WC:
					Stats.BonusMinDmg = (int)value;
					Stats.BonusMaxDmg = (int)value;
					break;
				case ItemOptionType.IOT_AC:
					Stats.BonusDef = (int)value;
					break;
				case ItemOptionType.IOT_MA:
					Stats.BonusMinMDmg = (int)value;
					Stats.BonusMaxMDmg = (int)value;
					break;
				case ItemOptionType.IOT_MR:
					Stats.BonusMDef = (int)value;
					break;
				case ItemOptionType.IOT_TB:
					Stats.BonusEvasion = (int)value;
					break;
				case ItemOptionType.IOT_MAXSOCKETCOUNT:
					MaxSocketCount = (byte)value;
					break;
				case ItemOptionType.IOT_MOUNTHP:
					if (Mount == null)
						break;
					Mount.CurrentHP = (int)value;
					break;
				case ItemOptionType.IOT_MOUNTGRADE:
					if (Mount == null)
						break;
					Mount.Grade = (byte)value;
					break;
				case ItemOptionType.IOT_CC_SERIAL:
					Serial = (int)value;
					break;
				case ItemOptionType.IOT_CC_STAR:
					StarCount = (byte)value;
					break;
				case ItemOptionType.IOT_CC_CARDID:
					CardID = (ushort)value;
					break;
				case ItemOptionType.IOT_CC_CARDGROUP:
					CardGroup = (ushort)value;
					break;
			}
		}

		public ItemState GetState()
		{
			switch (Inventory.Type)
			{
				case InventoryType.REWARD_STORAGE:
					return ItemState.REWARD_STORAGE;
				case InventoryType.GUILD:
					return ItemState.GUILD;
				case InventoryType.STORAGE:
					return ItemState.STORAGE;
				case InventoryType.EQUIPPED:
					return ItemState.EQUIPPED;
				case InventoryType.CHAR_INVENTORY:
					return ItemState.CHAR_INVENTORY;
				case InventoryType.MINIHOUSE_SKIN:
					return ItemState.HOUSE;
				default:
					return ItemState.NONE;
			}
		}

		public byte GetStatCount()
		{
			byte num = 0;
			if (!Item.CanEquip)
				return num;
			if (Stats.BonusSTR > 0)
				++num;
			if (Stats.BonusEND > 0)
				++num;
			if (Stats.BonusDEX > 0)
				++num;
			if (Stats.BonusINT > 0)
				++num;
			if (Stats.BonusSPR > 0)
				++num;
			if (Stats.BonusAim > 0)
				++num;
			if (Stats.CriticalRate > Decimal.Zero)
				++num;
			if (Stats.BonusMinDmg > 0)
				++num;
			if (Stats.BonusDef > 0)
				++num;
			if (Stats.BonusMinMDmg > 0)
				++num;
			if (Stats.BonusMDef > 0)
				++num;
			if (Stats.BonusEvasion > 0)
				++num;
			return num;
		}

		public byte GetDataLength()
		{
			switch (Item.Info.Class)
			{
				case ItemClass.BYTELOT:
				case ItemClass.RECALLSCROLL:
				case ItemClass.BINDITEM:
				case ItemClass.UPSOURCE:
				case ItemClass.UPRED:
				case ItemClass.UPBLUE:
				case ItemClass.FEED:
				case ItemClass.UPGOLD:
				case ItemClass.NOEFFECT:
					return 5;
				case ItemClass.WORDLOT:
				case ItemClass.QUESTITEM:
				case ItemClass.KQSTEP:
				case ItemClass.ACTIVESKILL:
					return 6;
				case ItemClass.DWRDLOT:
				case ItemClass.HOUSESKIN:
				case ItemClass.AMOUNT:
				case ItemClass.COSWEAPON:
				case ItemClass.ACTIONITEM:
				case ItemClass.ENCHANT:
				case ItemClass.COSSHIELD:
					return 8;
				case ItemClass.AMULET:
					return (byte)(18 + GetStatCount() * 3);
				case ItemClass.WEAPON:
					return (byte)(69 + GetStatCount() * 3);
				case ItemClass.ARMOR:
				case ItemClass.SHIELD:
				case ItemClass.BOOT:
					return (byte)(17 + GetStatCount() * 3);
				case ItemClass.FURNITURE:
				case ItemClass.DECORATION:
				case ItemClass.ITEMCHEST:
					return 12;
				case ItemClass.SKILLSCROLL:
				case ItemClass.WTLICENCE:
				case ItemClass.KQ:
				case ItemClass.GBCOIN:
					return 4;
				case ItemClass.RIDING:
					return 23;
				case ItemClass.CLOSEDCARD:
					return 13;
				case ItemClass.OPENCARD:
					return 9;
				case ItemClass.PUP:
					return 30;
				case ItemClass.BRACELET:
					return 17;
				default:
					return 0;
			}
		}

		public NetworkMessage GetEquipMessage(Character character)
		{
			switch (Item.Slot)
			{
				case ItemSlot.HEAD:
				case ItemSlot.NECK:
				case ItemSlot.BODY:
				case ItemSlot.LEFTHAND:
				case ItemSlot.BRACELET:
				case ItemSlot.RING:
				case ItemSlot.RING_R:
				case ItemSlot.PANT:
				case ItemSlot.BOOT:
				case ItemSlot.EAR:
				case ItemSlot.MINIMON:
				case ItemSlot.MINIMON_R:
					if (Item.Slot == ItemSlot.LEFTHAND && character.Shape.BaseClass == CharacterClass.CC_ARCHER)
						return new PROTO_NC_BRIEFINFO_CHANGEWEAPON_CMD(character, this);
					return new PROTO_NC_BRIEFINFO_CHANGEUPGRADE_CMD(character, this);
				case ItemSlot.ACCBODY:
				case ItemSlot.ACCBACK:
				case ItemSlot.ACCLEFTHAND:
				case ItemSlot.ACCRIGHTHAND:
				case ItemSlot.COSEFF:
				case ItemSlot.ACCHIP:
				case ItemSlot.ACCPANT:
				case ItemSlot.ACCBOOT:
				case ItemSlot.MOUTH:
				case ItemSlot.EYE:
				case ItemSlot.ACCHEAD:
				case ItemSlot.ACCSHIELD:
					return new PROTO_NC_BRIEFINFO_CHANGEDECORATE_CMD(character, this);
				case ItemSlot.RIGHTHAND:
					return new PROTO_NC_BRIEFINFO_CHANGEWEAPON_CMD(character, this);
				default:
					return null;
			}
		}

		public bool TryGetSocketIndex(out byte indx)
		{
			indx = byte.MaxValue;
			if (Sockets.Count >= MaxSocketCount)
				return false;
			for (var i = 0; i < MaxSocketCount; i++)
			{
				var i1 = i;
				if (Sockets.Exists(s => s.Index == i1)) continue;
				indx = (byte)i;
				return true;
			}
			return false;
		}

		public ItemInstance Duplicate()
		{
			return (ItemInstance)MemberwiseClone();
		}
	}
}
