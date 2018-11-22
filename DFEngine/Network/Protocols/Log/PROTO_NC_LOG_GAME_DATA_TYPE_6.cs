namespace DFEngine.Network.Protocols.Log
{
	public class PROTO_NC_LOG_GAME_DATA_TYPE_6 : NetworkMessage
	{
		public PROTO_NC_LOG_GAME_DATA_TYPE_6(GameLogType nType, int nCharNo, string sMap, int nMapX, int nMapY, int nMapZ, int nInt1, long nBigint1) : base(NetworkCommand.NC_LOG_GAME_DATA_TYPE_6)
		{
			Write((int)nType);
			Write(nCharNo);
			Write(sMap, 12);
			Write(nMapX);
			Write(nMapY);
			Write(nMapZ);
			Write(nInt1);
			Write(nBigint1);
		}
	}
}
