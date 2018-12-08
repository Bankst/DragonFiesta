using DFEngine.Content.GameObjects;

namespace DFEngine.Network
{
	class PROTO_NC_CHAR_CLIENT_SHAPE_CMD : NetworkMessage
	{
		public PROTO_NC_CHAR_CLIENT_SHAPE_CMD(Character character) : base(NetworkCommand.NC_CHAR_CLIENT_SHAPE_CMD)
		{
			this.Write(character.Shape);
		}
	}
}
