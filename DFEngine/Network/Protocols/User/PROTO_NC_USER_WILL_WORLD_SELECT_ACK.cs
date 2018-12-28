namespace DFEngine.Network.Protocols
{
	public class PROTO_NC_USER_WILL_WORLD_SELECT_ACK : NetworkMessage
	{
		public PROTO_NC_USER_WILL_WORLD_SELECT_ACK(ushort error, string otp) : base(NetworkCommand.NC_USER_WILL_WORLD_SELECT_ACK)
		{
			Write(error);
			Write(otp, 32);
		}
	}
}
