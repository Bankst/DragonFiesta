using DFEngine.Content.GameObjects;

namespace DFEngine.Network
{
	public sealed class PROTO_NC_CHAR_CLIENT_QUEST_DONE_CMD : NetworkMessage
	{
		public PROTO_NC_CHAR_CLIENT_QUEST_DONE_CMD(Character character) : base(NetworkCommand.NC_CHAR_CLIENT_QUEST_DONE_CMD)
		{
			Write(character.CharNo);
			Write((ushort) 0);
			Write((ushort) 0);
			Write(0);
		}
	}
}
