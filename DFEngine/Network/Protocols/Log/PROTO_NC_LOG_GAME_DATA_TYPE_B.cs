namespace DFEngine.Network.Protocols
{
	public class PROTO_NC_LOG_GAME_DATA_TYPE_B : NetworkMessage
	{
		public PROTO_NC_LOG_GAME_DATA_TYPE_B(GameLogType nType, int nCharNo, string sMap, int nMapX, int nMapY, int nMapZ, int nTargetCharNo, int nTargetID, long nItemKey, int nInt1, int nInt2, int nInt3) : base(NetworkCommand.NC_LOG_GAME_DATA_TYPE_B)
		{
			Write((int)nType);
			Write(nCharNo);
			Write(sMap, 12);
			Write(nMapX);
			Write(nMapY);
			Write(nMapZ);
			Write(nTargetCharNo);
			Write(nTargetID);
			Write(nItemKey);
			Write(nInt1);
			Write(nInt2);
			Write(nInt3);
		}
	}
}
