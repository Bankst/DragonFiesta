namespace DFEngine.Network.Protocols.Log
{
	public class PROTO_NC_LOG_GAME_DATA_TYPE_9 : NetworkMessage
	{
		public PROTO_NC_LOG_GAME_DATA_TYPE_9(GameLogType nType, int nCharNo, string sMap, int nMapX, int nMapY, int nMapZ, long nItemKey, int nInt1, int nInt2) : base(NetworkCommand.NC_LOG_GAME_DATA_TYPE_9)
		{
			Write((int)nType);
			Write(nCharNo);
			Write(sMap, 12);
			Write(nMapX);
			Write(nMapY);
			Write(nMapZ);
			Write(nItemKey);
			Write(nInt1);
			Write(nInt2);
		}
	}
}
