namespace DFEngine.Network.Protocols.Log
{
	public class PROTO_NC_LOG_GAME_DATA_TYPE_H : NetworkMessage
	{
		public PROTO_NC_LOG_GAME_DATA_TYPE_H(GameLogType nType, int nCharNo, string sMap, int nMapX, int nMapY, int nMapZ, int nTargetCharNo) : base(NetworkCommand.NC_LOG_GAME_DATA_TYPE_H)
		{
			Write((int)nType);
			Write(nCharNo);
			Write(sMap, 12);
			Write(nMapX);
			Write(nMapY);
			Write(nMapZ);
			Write(nTargetCharNo);
		}
	}
}
