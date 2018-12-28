namespace DFEngine.Network
{
	public sealed class PROTO_NC_CHAR_OPTION_IMPROVE_GET_KEYMAP_CMD : NetworkMessage
	{
		public PROTO_NC_CHAR_OPTION_IMPROVE_GET_KEYMAP_CMD(byte[] keyMap) : base(NetworkCommand.NC_CHAR_OPTION_IMPROVE_GET_KEYMAP_CMD)
		{
			Write(keyMap);
		}
	}
}
