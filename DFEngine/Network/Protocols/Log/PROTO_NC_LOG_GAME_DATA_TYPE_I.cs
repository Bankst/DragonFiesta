namespace DFEngine.Network.Protocols.Log
{
	public class PROTO_NC_LOG_GAME_DATA_TYPE_I : NetworkMessage
	{
		public PROTO_NC_LOG_GAME_DATA_TYPE_I(GameLogType nType, int nCharNo, string sMap, int nMapX, int nMapY, int nMapZ, int nTargetCharNo, int nTargetID) : base(NetworkCommand.NC_LOG_GAME_DATA_TYPE_I)
		{
			Write((int)nType);
			Write(nCharNo);
			Write(sMap, 12);
			Write(nMapX);
			Write(nMapY);
			Write(nMapZ);
			Write(nTargetCharNo);
			Write(nTargetID);
		}
	}
}
