namespace DFEngine.Network
{
	public sealed class PROTO_NC_CHAR_OPTION_IMPROVE_GET_SHORTCUTDATA_CMD: NetworkMessage
	{
		public PROTO_NC_CHAR_OPTION_IMPROVE_GET_SHORTCUTDATA_CMD(byte[] shortcutData) : base(NetworkCommand.NC_CHAR_OPTION_IMPROVE_GET_SHORTCUTDATA_CMD)
		{
			Write(shortcutData);
		}
	}
}
