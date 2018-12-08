using DFEngine.Content.GameObjects;

namespace DFEngine.Network
{
	public sealed class PROTO_NC_QUEST_RESET_TIME_CLIENT_CMD : NetworkMessage
	{
		public PROTO_NC_QUEST_RESET_TIME_CLIENT_CMD() : base(NetworkCommand.NC_QUEST_RESET_TIME_CLIENT_CMD)
		{
			Write(0);
			Write(0);
			Write(0);
			Write(0);
		}
	}
}
