namespace DFEngine.Network.Protocols.Log
{
	public class PROTO_NC_LOG_GAME_DATA_TYPE_5 : NetworkMessage
	{
		public PROTO_NC_LOG_GAME_DATA_TYPE_5(GameLogType nType, int nCharNo, int nInt1, int nInt2) : base(NetworkCommand.NC_LOG_GAME_DATA_TYPE_5)
		{
			Write((int)nType);
			Write(nCharNo);
			Write(nInt1);
			Write(nInt2);
		}
	}
}
