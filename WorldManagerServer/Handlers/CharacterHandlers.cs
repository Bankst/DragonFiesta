using System.Linq;
using DFEngine.Logging;
using DFEngine.Network;
using DFEngine.Network.Protocols;
using DFEngine.Server;
using WorldManagerServer.Services;

namespace WorldManagerServer.Handlers
{
	internal static class CharacterHandlers
	{
		internal static void NC_CHAR_LOGIN_REQ(NetworkMessage message, NetworkConnection connection)
		{
			if (CharacterService.CharLogin(connection, message.ReadByte(), out var zoneEndPoint, out var error))
			{
				new PROTO_NC_CHAR_LOGIN_ACK(zoneEndPoint.Address.ToString(), (ushort)zoneEndPoint.Port).Send(connection);
			}
			else
			{
				new PROTO_NC_CHAR_LOGINFAIL_ACK(error).Send(connection);
			}
		}
	}
}
