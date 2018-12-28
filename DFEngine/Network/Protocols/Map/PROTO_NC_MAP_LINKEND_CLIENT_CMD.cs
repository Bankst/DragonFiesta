namespace DFEngine.Network
{
	public sealed class PROTO_NC_MAP_LINKEND_CLIENT_CMD : NetworkMessage
	{
		public PROTO_NC_MAP_LINKEND_CLIENT_CMD() : base(NetworkCommand.NC_MAP_LINKEND_CLIENT_CMD)
		{
			Write((byte) 0);
		}
	}
}
