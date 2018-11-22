﻿namespace DFEngine.Network.Protocols.Log
{
	public class PROTO_NC_LOG_GAME_DATA_TYPE_A : NetworkMessage
	{
		public PROTO_NC_LOG_GAME_DATA_TYPE_A(GameLogType nType, int nCharNo, int nTargetCharNo, int nTargetID, long nItemKey, int nInt1, int nInt2, int nInt3) : base(NetworkCommand.NC_LOG_GAME_DATA_TYPE_A)
		{
			Write((int)nType);
			Write(nCharNo);
			Write(nTargetCharNo);
			Write(nTargetID);
			Write(nItemKey);
			Write(nInt1);
			Write(nInt2);
			Write(nInt3);
		}
	}
}