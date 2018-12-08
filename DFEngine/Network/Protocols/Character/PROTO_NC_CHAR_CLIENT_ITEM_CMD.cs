using DFEngine.Content.Items;

namespace DFEngine.Network
{
	public sealed class PROTO_NC_CHAR_CLIENT_ITEM_CMD : NetworkMessage
	{
		public PROTO_NC_CHAR_CLIENT_ITEM_CMD(Inventory inventory) : base(NetworkCommand.NC_CHAR_CLIENT_ITEM_CMD)
		{
			Write((byte) 0); // item count
			Write((byte) inventory.Type);
			Write(inventory.Flags);
		}
	}
}
