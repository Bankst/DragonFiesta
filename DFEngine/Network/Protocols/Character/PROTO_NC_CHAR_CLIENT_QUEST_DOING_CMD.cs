using DFEngine.Content.GameObjects;

namespace DFEngine.Network
{
	public sealed class PROTO_NC_CHAR_CLIENT_QUEST_DOING_CMD : NetworkMessage
	{
		public PROTO_NC_CHAR_CLIENT_QUEST_DOING_CMD(Character character) : base(NetworkCommand.NC_CHAR_CLIENT_QUEST_DOING_CMD)
		{
			Write(character.CharNo);
			Write(false);
			Write((byte) 0); // quest count
		}
	}
}
