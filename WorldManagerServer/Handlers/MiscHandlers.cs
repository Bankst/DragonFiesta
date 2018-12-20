using DFEngine.Logging;
using DFEngine.Network;
using DFEngine.Network.Protocols;
using DFEngine.Server;

namespace WorldManagerServer.Handlers
{
	internal static class MiscHandlers
	{
		internal static void NC_MISC_SEED_ACK(NetworkMessage message, NetworkConnection connection)
		{
			connection.SetSeed(message.ReadUInt16());
		}

		internal static void NC_MISC_GAMETIME_REQ(NetworkMessage message, NetworkConnection connection)
		{
			new PROTO_NC_MISC_GAMETIME_ACK().Send(connection);
		}

        internal static void NC_MISC_SERVER_TIME_NOTIFY_CMD(NetworkMessage message, NetworkConnection connection)
        {
            new PROTO_NC_MISC_SERVER_TIME_NOTIFY_CMD().Send(connection);
        }


        internal static void NC_MISC_S2SCONNECTION_RDY(NetworkMessage message, NetworkConnection connection)
		{
			new PROTO_NC_MISC_S2SCONNECTION_REQ(
				NetworkConnectionType.NCT_WORLDMANAGER, // From 
				connection.Type, // To
				WorldManagerServer.NetConfig.WorldNetConfig.WorldID, // World ID
				"ISYA", // World Name TODO: Add this to WorldConfiguration/NetworkConfiguration
				0, // ZoneNo TODO: do this programatically? Or ignore it for Login?
				WorldManagerServer.NetConfig.WorldNetConfig.ListenIP, // Client Listen IP
				(ushort) WorldManagerServer.NetConfig.WorldNetConfig.ListenPort // Client Listen Port
			).Send(connection);
		}

		internal static void NC_MISC_S2SCONNECTION_REQ(NetworkMessage message, NetworkConnection connection)
		{
			var from = message.ReadByte();
			var to = message.ReadByte();
			var worldNumber = message.ReadByte();
			var worldName = message.ReadString(20);
			var zoneNumber = message.ReadByte();
			var ip = message.ReadString(16);
			var port = message.ReadUInt16();
			var key = message.ReadUInt16();

			if (key != from + to)
			{
				SocketLog.Write(SocketLogLevel.Warning, "Invalid key used with S2S connection.");
				connection.Disconnect();
				return;
			}

			if ((NetworkConnectionType)from != NetworkConnectionType.NCT_ZONE) return;
			if (WorldManagerServer.Zones.Exists(zone => zone.Connection == connection || zone.Number == zoneNumber))
			{
				return;
			}
			WorldManagerServer.Zones.Add(new Zone(connection, zoneNumber, ip, port));
			new PROTO_NC_MISC_S2SCONNECTION_ACK(0, 0).Send(connection);
		}

		internal static void NC_MISC_S2SCONNECTION_ACK(NetworkMessage message, NetworkConnection connection)
		{
			var echoData = message.ReadByte();
			var error = message.ReadUInt16();
		}
	}
}
