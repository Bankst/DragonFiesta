using System.Net;

namespace DFEngine.Network.Protocols
{
	public class PROTO_NC_LOG_USER_LOGIN : NetworkMessage
	{
		public PROTO_NC_LOG_USER_LOGIN(int userNo, byte worldNum, string ip) : base(NetworkCommand.NC_LOG_USER_LOGIN)
		{
			Write((uint)userNo);
			Write(worldNum);

			var ipArr = IPAddress.Parse(ip).GetAddressBytes();
			for (var i = 0; i < 4; i++)
			{
				Write(ipArr[i]);
			}
		}
	}
}
