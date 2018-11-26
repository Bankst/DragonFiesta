namespace DFEngine.Network.Protocols
{
	public class PROTO_NC_LOG_GAME_DATA_TYPE_E : NetworkMessage
	{
		public PROTO_NC_LOG_GAME_DATA_TYPE_E(GameLogType nType, string sMap, int nMapX, int nMapY, int nMapZ, int nTargetID, long nItemKey, int nInt1, int nInt2) : base(NetworkCommand.NC_LOG_GAME_DATA_TYPE_E)
		{
			Write((int)nType);
			Write(sMap, 12);
			Write(nMapX);
			Write(nMapY);
			Write(nMapZ);
			Write(nTargetID);
			Write(nItemKey);
			Write(nInt1);
			Write(nInt2);
		}
	}
}
