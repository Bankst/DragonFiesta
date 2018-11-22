using System;

namespace DFEngine.Network.Protocols.Misc
{
	public class PROTO_NC_MISC_GAMETIME_ACK : NetworkMessage
	{
		public PROTO_NC_MISC_GAMETIME_ACK() : base(NetworkCommand.NC_MISC_GAMETIME_ACK)
		{
			Write((char)DateTime.Now.Hour);
			Write((char)DateTime.Now.Minute);
			Write((char)DateTime.Now.Second);
		}
	}
}
