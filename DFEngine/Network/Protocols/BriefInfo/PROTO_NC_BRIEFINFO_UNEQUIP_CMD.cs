using DFEngine.Content.GameObjects;

namespace DFEngine.Network
{
	public sealed class PROTO_NC_BRIEFINFO_UNEQUIP_CMD : NetworkMessage
	{
		public PROTO_NC_BRIEFINFO_UNEQUIP_CMD(Character character, byte slot) : base(NetworkCommand.NC_BRIEFINFO_UNEQUIP_CMD)
		{
			Write(character.Handle);
			Write(slot);
		}
	}
}
