using DFEngine.Content.GameObjects;

namespace DFEngine.Network
{
	public sealed class PROTO_NC_CHAR_CLIENT_PASSIVE_CMD : NetworkMessage
	{
		public PROTO_NC_CHAR_CLIENT_PASSIVE_CMD(Character character) : base(NetworkCommand.NC_CHAR_CLIENT_PASSIVE_CMD)
		{
			Write((ushort) 0);
		}
	}
}
