namespace DFEngine.Network
{
	public sealed class PROTO_NC_CHAR_OPTION_GET_WINDOWPOS_ACK : NetworkMessage
	{
		public PROTO_NC_CHAR_OPTION_GET_WINDOWPOS_ACK(byte[] windowPosData) : base(NetworkCommand.NC_CHAR_OPTION_GET_WINDOWPOS_ACK)
		{
			Write(true);
			Write(windowPosData);
		}
	}
}
