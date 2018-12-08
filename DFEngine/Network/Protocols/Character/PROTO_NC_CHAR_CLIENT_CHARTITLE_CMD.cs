using DFEngine.Content.GameObjects;

namespace DFEngine.Network
{
	public sealed class PROTO_NC_CHAR_CLIENT_CHARTITLE_CMD : NetworkMessage
	{
		public PROTO_NC_CHAR_CLIENT_CHARTITLE_CMD(Character character) : base(NetworkCommand.NC_CHAR_CLIENT_CHARTITLE_CMD)
		{
			Write((byte) 0);
			Write((byte) 0);
			Write((ushort) 0);
			Write((ushort) 0);
		}
	}
}
