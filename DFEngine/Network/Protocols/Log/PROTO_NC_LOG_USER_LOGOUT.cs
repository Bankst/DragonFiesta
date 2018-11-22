namespace DFEngine.Network.Protocols
{
	public class PROTO_NC_LOG_USER_LOGOUT : NetworkMessage
	{
		public PROTO_NC_LOG_USER_LOGOUT(uint userNo, char worldNum, ushort playMin, ushort errorCode) : base(NetworkCommand.NC_LOG_USER_LOGOUT)
		{
			Write(userNo);
			Write(worldNum);
			Write(playMin);
			Write(errorCode);
		}
	}
}
