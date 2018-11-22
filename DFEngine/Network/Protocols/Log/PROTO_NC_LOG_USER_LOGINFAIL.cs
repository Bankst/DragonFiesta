using System.Net;

namespace DFEngine.Network.Protocols.Log
{
	public class PROTO_NC_LOG_USER_LOGINFAIL : NetworkMessage
	{
		public PROTO_NC_LOG_USER_LOGINFAIL(string userName, string userPass, string userIp, ushort error) : base(NetworkCommand.NC_LOG_USER_LOGINFAIL)
		{
			Write(userName, 256);
			Write(userPass, 32);
			var ipArr = IPAddress.Parse(userIp).GetAddressBytes();
			for (var i = 0; i < 4; i++)
			{
				Write(ipArr[i]);
			}
			Write(error);
		}
	}
}
