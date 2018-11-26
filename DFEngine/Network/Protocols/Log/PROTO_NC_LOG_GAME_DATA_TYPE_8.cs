namespace DFEngine.Network.Protocols
{
	public class PROTO_NC_LOG_GAME_DATA_TYPE_8 : NetworkMessage
	{
		public PROTO_NC_LOG_GAME_DATA_TYPE_8(GameLogType nType, int nCharNo, string sMap, int nMapX, int nMapY, int nMapZ, int nInt1) : base(NetworkCommand.NC_LOG_GAME_DATA_TYPE_9)
		{
			Write((int)nType);
			Write(nCharNo);
			Write(sMap, 12);
			Write(nMapX);
			Write(nMapY);
			Write(nMapZ);
			Write(nInt1);
		}
	}
}
