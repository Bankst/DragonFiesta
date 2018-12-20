using System;
using DFEngine.Content.Game;
using DFEngine.Content.GameObjects.MiniHouse;
using DFEngine.Content.Items;
using DFEngine.Logging;
using DFEngine.Network;
using DFEngine.Network.Protocols;

namespace DFEngine.Content.GameObjects
{
	/// <summary>
	/// Class that contains character data.
	/// </summary>
	public class Character : GameObject
	{
		public static CharacterCommon Global { get; set; }

		public NetworkConnection Client { get; set; }

		public int CharNo { get; set; }
		public int UserNo { get; set; }
		public string Name { get; set; }
		public byte Slot { get; set; }
		public byte AdminLevel { get; set; }
		public ushort PrisonMinutes { get; set; }

		public long EXP { get; set; }
		public long Cen { get; set; }
		public long UserCen { get; set; }
		public int Fame { get; set; }

		public int Flags { get; set; }
		public byte PKYellowTime { get; set; }

		public byte StatPoints { get; set; }
		public byte SkillPoints { get; set; }
		public int KillPoints { get; set; }

		public CharacterShape Shape { get; set; }
		public Parameters Parameters { get; set; }
		public Inventory Inventory { get; set; }
		public Equipment Equipment { get; set; }
		public Inventory MiniHouseSkins { get; set; }
		public Inventory ActionBoxes { get; set; }
		public MiniHouseInstance MiniHouse { get; set; }

		public long NextMHHPTick { get; set; }
		public long NextMHSPTick { get; set; }

		public long LastLPUpdate { get; set; }

		public bool IsLoggingOut { get; set; }
		public bool IsLoggedOut { get; set; }
		public bool IsTrading { get; set; }

		public bool IsInHouse
		{
			get
			{
				if (State != GameObjectState.HOUSE)
					return State == GameObjectState.VENDOR;
				return true;
			}
		}

		public Character()
		{
			Type = GameObjectType.CHARACTER;
			Shape = new CharacterShape();

			Inventory = new Inventory(InventoryType.CHAR_INVENTORY, 0);
			Equipment = new Equipment();
			MiniHouseSkins = new Inventory(InventoryType.MINIHOUSE_SKIN, 0);
			ActionBoxes = new Inventory(InventoryType.ACTION_BOX, 0);
		}

		public void Login()
		{
			new PROTO_NC_CHAR_CLIENT_BASE_CMD(this).Send(Client);
			new PROTO_NC_CHAR_CLIENT_SHAPE_CMD(this).Send(Client);
			new PROTO_NC_CHAR_CLIENT_QUEST_DOING_CMD(this).Send(Client);
			new PROTO_NC_CHAR_CLIENT_QUEST_DONE_CMD(this).Send(Client);
			new PROTO_NC_CHAR_CLIENT_QUEST_READ_CMD(this).Send(Client);
			new PROTO_NC_CHAR_CLIENT_SKILL_CMD(this).Send(Client);
			new PROTO_NC_CHAR_CLIENT_PASSIVE_CMD(this).Send(Client);
			new PROTO_NC_CHAR_CLIENT_ITEM_CMD(Inventory).Send(Client);
			new PROTO_NC_CHAR_CLIENT_ITEM_CMD(Equipment).Send(Client);
			new PROTO_NC_CHAR_CLIENT_ITEM_CMD(MiniHouseSkins).Send(Client);
			new PROTO_NC_CHAR_CLIENT_ITEM_CMD(ActionBoxes).Send(Client);
			new PROTO_NC_CHAR_CLIENT_CHARTITLE_CMD(this).Send(Client);
			new PROTO_NC_CHAR_CLIENT_CHARGEDBUFF_CMD(this).Send(Client);
			new PROTO_NC_CHAR_CLIENT_GAME_CMD().Send(Client);
			new PROTO_NC_CHAR_CLIENT_COININFO_CMD(this).Send(Client);

			new PROTO_NC_QUEST_RESET_TIME_CLIENT_CMD().Send(Client);

			new PROTO_NC_MAP_LOGIN_ACK(this).Send(Client);
			GameLog.Write(GameLogLevel.Internal, $"{Name} is logging in.");
		}

		public void FinalizeLogin()
		{
			Position.Map.Objects.Add(this);
			VisibleObjects = Position.GetSurroundingObjects((int) Position.Map.Info.Sight);

			if (IsDead || Stats.CurrentHP <= 0)
			{
				Stats.CurrentHP = Stats.CurrentMaxHP / 2;
				Stats.CurrentSP = Stats.CurrentMaxSP / 2;
				Stats.CurrentLP = Stats.CurrentMaxLP;
			}

			IsDead = false;

			for (byte i = 1; i <= 29; i++)
			{
				new PROTO_NC_BRIEFINFO_UNEQUIP_CMD(this, i).Send(this);
			}

			new PROTO_NC_MAP_FIELD_ATTRIBUTE_CMD(Position.Map).Send(this);

			new PROTO_NC_BAT_HPCHANGE_CMD(this).Send(this);
			new PROTO_NC_BAT_SPCHANGE_CMD(this).Send(this);
			if (Shape.BaseClass == CharacterClass.CC_CRUSADER)
			{
				new PROTO_NC_BAT_LPCHANGE_CMD(this).Send(this);
			}

			GameLog.Write(GameLogLevel.Internal, $"{Name} has logged in.");
		}

		public void PrepareLogout()
		{
			IsLoggingOut = true;
		}

		public void CancelLogout()
		{
			IsLoggingOut = false;
		}

		public void Move(Vector2 from, Vector2 to, MoveType type)
		{
			switch (type)
			{
				case MoveType.MT_WALK:
				case MoveType.MT_RUN:
					IsWalking = type == MoveType.MT_WALK;
					Behavior?.MoveTo(from, to);
					break;
				case MoveType.MT_REINFORCEMOVE:
					Behavior?.MoveTo(from, to, 0D, true);
					break;
				case MoveType.MT_HALT:
				case MoveType.MT_REINFORCEHALT:
					Behavior?.Stop(to, type == MoveType.MT_REINFORCEHALT);
					break;
			}
		}
	}
}
