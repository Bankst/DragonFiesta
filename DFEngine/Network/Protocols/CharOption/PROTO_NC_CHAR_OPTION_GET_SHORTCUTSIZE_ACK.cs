namespace DFEngine.Network
{
	public sealed class PROTO_NC_CHAR_OPTION_GET_SHORTCUTSIZE_ACK : NetworkMessage
	{
		public PROTO_NC_CHAR_OPTION_GET_SHORTCUTSIZE_ACK(byte[] shortcutSizeData) : base(NetworkCommand.NC_CHAR_OPTION_GET_SHORTCUTSIZE_ACK)
		{
			Write(true);
			Write(shortcutSizeData);
		}
	}
}
