using DFEngine.Server;

namespace DFEngine.Network
{
	public class PROTO_NC_CHAR_LOGIN_ACK : NetworkMessage
	{
		public PROTO_NC_CHAR_LOGIN_ACK(Zone zone) : base(NetworkCommand.NC_CHAR_LOGIN_ACK)
		{
			Write(zone.IP, 16);
			Write(zone.Port);
		}
	}
}
