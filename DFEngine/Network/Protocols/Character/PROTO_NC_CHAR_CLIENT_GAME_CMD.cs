namespace DFEngine.Network
{
	public class PROTO_NC_CHAR_CLIENT_GAME_CMD : NetworkMessage
	{
		public PROTO_NC_CHAR_CLIENT_GAME_CMD() : base(NetworkCommand.NC_CHAR_CLIENT_GAME_CMD)
		{
			Write(ushort.MaxValue);
			Write(ushort.MaxValue);
		}
	}
}
