namespace DFEngine.Content.Items
{
	public enum ItemState : byte
	{
		GUILD = 0,
		REWARD_STORAGE = 8,
		STORAGE = 24, // 0x18
		EQUIPPED = 32, // 0x20
		CHAR_INVENTORY = 36, // 0x24
		HOUSE = 48, // 0x30
		PREM = 155, // 0x9B
		NONE = 255, // 0xFF
	}
}
