namespace DFEngine.Network.Protocols.Log
{
	public class PROTO_NC_LOG_GAME_DATA_TYPE_7 : NetworkMessage
	{
		public PROTO_NC_LOG_GAME_DATA_TYPE_7(GameLogType nType, int nCharNo, string sMap, int nMapX, int nMapY, int nMapZ, int nTargetCharNo, int nInt1, long nInt2) : base(NetworkCommand.NC_LOG_GAME_DATA_TYPE_7)
		{
			Write((int)nType);
			Write(nCharNo);
			Write(sMap, 12);
			Write(nMapX);
			Write(nMapY);
			Write(nMapZ);
			Write(nTargetCharNo);
			Write(nInt1);
			Write(nInt2);
		}
	}
}
