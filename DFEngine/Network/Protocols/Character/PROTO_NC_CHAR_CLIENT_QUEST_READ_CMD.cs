using DFEngine.Content.GameObjects;

namespace DFEngine.Network
{
	public sealed class PROTO_NC_CHAR_CLIENT_QUEST_READ_CMD : NetworkMessage
	{
		public PROTO_NC_CHAR_CLIENT_QUEST_READ_CMD(Character character) : base(NetworkCommand.NC_CHAR_CLIENT_QUEST_READ_CMD)
		{
			Write(character.CharNo);
			Write((ushort) 0);
		}
	}
}
