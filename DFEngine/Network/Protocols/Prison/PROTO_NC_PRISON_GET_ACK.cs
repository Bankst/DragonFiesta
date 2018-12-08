namespace DFEngine.Network
{
	public sealed class PROTO_NC_PRISON_GET_ACK : NetworkMessage
	{
		public PROTO_NC_PRISON_GET_ACK(ushort error) : base(NetworkCommand.NC_PRISON_GET_ACK)
		{
			Write(error);
			Write((ushort)0);

			// Write the prison
		}
	}
}
