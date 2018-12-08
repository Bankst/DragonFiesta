using DFEngine.Content.Items;
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
		}
	}
}
