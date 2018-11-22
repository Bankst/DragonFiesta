namespace DFEngine.Network.Protocols.Log
{
	public class PROTO_NC_LOG_GAME_DATA_TYPE_F : NetworkMessage
	{
		public PROTO_NC_LOG_GAME_DATA_TYPE_F(GameLogType nType, int nCharNo, string sMap, int nMapX, int nMapY, int nMapZ, int nTargetID, long nItemKey, int nInt1, int nInt2, int nInt3, long nBigint1) : base(NetworkCommand.NC_LOG_GAME_DATA_TYPE_F)
		{
			Write((int)nType);
			Write(nCharNo);
			Write(sMap, 12);
			Write(nMapX);
			Write(nMapY);
			Write(nMapZ);
			Write(nTargetID);
			Write(nItemKey);
			Write(nInt1);
			Write(nInt2);
			Write(nInt3);
			Write(nBigint1);
		}
	}
}
