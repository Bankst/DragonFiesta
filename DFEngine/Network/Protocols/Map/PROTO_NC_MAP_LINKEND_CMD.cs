namespace DFEngine.Network
{
	public sealed class PROTO_NC_MAP_LINKEND_CMD : NetworkMessage
	{
		public PROTO_NC_MAP_LINKEND_CMD(ushort handle, string mapIndx) : base(NetworkCommand.NC_MAP_LINKEND_CMD)
		{
			Write(handle);
			Write(mapIndx, 12);
		}
	}
}
