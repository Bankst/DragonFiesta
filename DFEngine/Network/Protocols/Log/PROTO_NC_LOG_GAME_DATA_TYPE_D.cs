namespace DFEngine.Network.Protocols
{
	public class PROTO_NC_LOG_GAME_DATA_TYPE_D : NetworkMessage
	{
		public PROTO_NC_LOG_GAME_DATA_TYPE_D(GameLogType nType, int nCharNo, long nItemKey, int nInt1, int nInt2) : base(NetworkCommand.NC_LOG_GAME_DATA_TYPE_D)
		{
			Write((int)nType);
			Write(nCharNo);
			Write(nItemKey);
			Write(nInt1);
			Write(nInt2);
		}
	}
}
