namespace DFEngine.Network.Protocols
{
	public class PROTO_NC_LOG_GAME_DATA_TYPE_K : NetworkMessage
	{
		public PROTO_NC_LOG_GAME_DATA_TYPE_K(GameLogType nType, int nCharNo, string sMap, int nMapX, int nMapY, int nMapZ, int nInt1, int nInt2, long nBigint1) : base(NetworkCommand.NC_LOG_GAME_DATA_TYPE_K)
		{
			Write((int)nType);
			Write(nCharNo);
			Write(sMap, 12);
			Write(nMapX);
			Write(nMapY);
			Write(nMapZ);
			Write(nInt1);
			Write(nInt2);
			Write(nBigint1);
		}
	}
}
