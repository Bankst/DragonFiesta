using DFEngine.Content.GameObjects;

namespace DFEngine.Network
{
	public sealed class PROTO_NC_CHAR_CLIENT_COININFO_CMD : NetworkMessage
	{
		public PROTO_NC_CHAR_CLIENT_COININFO_CMD(Character character) : base(NetworkCommand.NC_CHAR_CLIENT_COININFO_CMD)
		{
			Write((long) 0);
			Write((long) 0);
		}
	}
}
