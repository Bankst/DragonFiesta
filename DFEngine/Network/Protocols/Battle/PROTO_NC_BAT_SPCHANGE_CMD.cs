using DFEngine.Content.GameObjects;

namespace DFEngine.Network
{
	public sealed class PROTO_NC_BAT_SPCHANGE_CMD : NetworkMessage
	{
		public PROTO_NC_BAT_SPCHANGE_CMD(Character character) : base(NetworkCommand.NC_BAT_SPCHANGE_CMD)
		{
			Write(character.Stats.CurrentSP);
		}
	}
}
