using DFEngine.Content.GameObjects;
using DFEngine.Content.Items;

namespace DFEngine.Network
{
	public sealed class PROTO_NC_BRIEFINFO_CHANGEUPGRADE_CMD : NetworkMessage
	{
		public PROTO_NC_BRIEFINFO_CHANGEUPGRADE_CMD(GameObject obj, ItemInstance item) : base(NetworkCommand.NC_BRIEFINFO_CHANGEUPGRADE_CMD)
		{
			Write(obj.Handle);
			Write(item.Item.Info.ID);
			Write(item.Upgrades);
			Write(item.InventoryNo);
		}
	}
}
