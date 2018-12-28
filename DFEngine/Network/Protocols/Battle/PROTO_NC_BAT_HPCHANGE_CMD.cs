using DFEngine.Content.GameObjects;

namespace DFEngine.Network
{
	public sealed class PROTO_NC_BAT_HPCHANGE_CMD : NetworkMessage
	{
		public PROTO_NC_BAT_HPCHANGE_CMD(Character character) : base(NetworkCommand.NC_BAT_HPCHANGE_CMD)
		{
			Write(character.Stats.CurrentHP);
			Write(character.HPChangeOrder);
		}
	}
}
