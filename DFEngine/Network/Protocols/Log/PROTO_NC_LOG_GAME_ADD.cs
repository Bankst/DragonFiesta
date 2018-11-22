using DFEngine.Content.GameObjects;

namespace DFEngine.Network.Protocols.Log
{
	public class PROTO_NC_LOG_GAME_ADD : NetworkMessage
	{
		public PROTO_NC_LOG_GAME_ADD(Character charac) : base(NetworkCommand.NC_LOG_GAME_ADD)
		{
			// unsigned int nType;
			// unsigned int nCharNo;
			// Name3 sMap;
			// unsigned int nMapX;
			// unsigned int nMapY;
			// unsigned int nMapZ;
			// unsigned int nTargetCharNo;
			// unsigned int nTargetID;
			// SHINE_ITEM_REGISTNUMBER nItemKey;
			// unsigned int nInt1;
			// unsigned int nInt2;
			// unsigned int nInt3;
			// unsigned __int64 nBigint1;
		}
	}
}
