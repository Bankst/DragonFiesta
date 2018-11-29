using System;
using System.Collections.Generic;
using System.Text;

namespace DFEngine.Network.Protocols
{
	public class PROTO_NC_CHAR_LOGIN_ACK : NetworkMessage
	{
		public PROTO_NC_CHAR_LOGIN_ACK(string zoneIp, ushort zonePort) : base(NetworkCommand.NC_CHAR_LOGIN_ACK)
		{
			Write(zoneIp);
			Write(zonePort);
		}
	}
}
