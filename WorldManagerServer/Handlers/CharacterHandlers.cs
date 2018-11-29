using System.Linq;
using DFEngine.Logging;
using DFEngine.Network;
using DFEngine.Network.Protocols;

namespace WorldManagerServer.Handlers
{
	internal static class CharacterHandlers
	{
		internal static void NC_CHAR_LOGIN_REQ(NetworkMessage message, NetworkConnection connection)
		{
			// we must get character map-zone here, and use for Id
			var zoneId = 0;

			var zoneConfig = ServerMain.NetConfig.ZoneNetworkConfigs.FirstOrDefault(z => z.ZoneID == zoneId);
			if (zoneConfig == null)
			{
				SocketLog.Write(SocketLogLevel.Exception, "Zone ID {zoneId} config not found!");
				return;
			}
			var zoneIp = zoneConfig.ExternalIP == zoneConfig.ListenIP ? zoneConfig.ListenIP : zoneConfig.ExternalIP;

			new PROTO_NC_CHAR_LOGIN_ACK(zoneIp, (ushort)zoneConfig.ListenPort).Send(connection);
		}
	}
}
