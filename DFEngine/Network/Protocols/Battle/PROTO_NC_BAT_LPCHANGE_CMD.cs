using DFEngine.Content.GameObjects;

namespace DFEngine.Network
{
	public sealed class PROTO_NC_BAT_LPCHANGE_CMD : NetworkMessage
	{
		public PROTO_NC_BAT_LPCHANGE_CMD(Character character) : base(NetworkCommand.NC_BAT_LPCHANGE_CMD)
		{
			Write(character.Stats.CurrentLP);
		}
	}
}
