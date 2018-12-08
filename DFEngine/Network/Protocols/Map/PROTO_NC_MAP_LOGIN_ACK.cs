using DFEngine.Content.GameObjects;

namespace DFEngine.Network
{
	class PROTO_NC_MAP_LOGIN_ACK : NetworkMessage
	{
		public PROTO_NC_MAP_LOGIN_ACK(Character character) : base(NetworkCommand.NC_MAP_LOGIN_ACK)
		{
			Write(character.Handle);
			this.WriteParameters(character);
		}
	}
}
