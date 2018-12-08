namespace DFEngine.Network
{
	public class PROTO_NC_CHAR_LOGINFAIL_ACK : NetworkMessage
	{
		public PROTO_NC_CHAR_LOGINFAIL_ACK(ushort error) : base(NetworkCommand.NC_CHAR_LOGINFAIL_ACK)
		{
			Write(error);
		}
	}
}