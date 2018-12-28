using DFEngine.Worlds;

namespace DFEngine.Network
{
	public sealed class PROTO_NC_MAP_FIELD_ATTRIBUTE_CMD : NetworkMessage
	{
		public PROTO_NC_MAP_FIELD_ATTRIBUTE_CMD(Map map) : base(NetworkCommand.NC_MAP_FIELD_ATTRIBUTE_CMD)
		{
			Write((byte) map.Field.Type);
		}
	}
}
