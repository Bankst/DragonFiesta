namespace DFEngine.Network
{
	public sealed class PROTO_NC_CHAR_OPTION_IMPROVE_GET_GAMEOPTION_CMD : NetworkMessage
	{
		public PROTO_NC_CHAR_OPTION_IMPROVE_GET_GAMEOPTION_CMD(byte[] gameOption) : base(NetworkCommand.NC_CHAR_OPTION_IMPROVE_GET_GAMEOPTION_CMD)
		{
			Write(gameOption);
		}
	}
}
