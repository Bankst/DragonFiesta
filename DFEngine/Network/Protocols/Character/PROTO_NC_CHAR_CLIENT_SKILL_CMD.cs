using DFEngine.Content.GameObjects;

namespace DFEngine.Network
{
	public sealed class PROTO_NC_CHAR_CLIENT_SKILL_CMD : NetworkMessage
	{
		public PROTO_NC_CHAR_CLIENT_SKILL_CMD(Character character) : base(NetworkCommand.NC_CHAR_CLIENT_SKILL_CMD)
		{
			Write((byte) 0);
			Write((byte) 0);
			Write((ushort) 0);
			Write(character.CharNo);
			Write((ushort) 0);
		}
	}
}
