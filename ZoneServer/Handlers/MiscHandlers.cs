using DFEngine.Network;
using DFEngine.Network.Protocols;

namespace ZoneServer.Handlers
{
	internal static class MiscHandlers
	{
		internal static void NC_MISC_SEED_ACK(NetworkMessage message, NetworkConnection connection)
		{
			connection.SetSeed(message.ReadUInt16());
		}

		internal static void NC_MISC_S2SCONNECTION_RDY(NetworkMessage message, NetworkConnection connection)
		{
			// send correct client IP
			var clientIP = ZoneServer.ZoneNetConfig.ListenIP != ZoneServer.ZoneNetConfig.ExternalIP
				? ZoneServer.ZoneNetConfig.ExternalIP
				: ZoneServer.ZoneNetConfig.ListenIP;
			new PROTO_NC_MISC_S2SCONNECTION_REQ(
				NetworkConnectionType.NCT_ZONE, // From 
				connection.Type, // To
				ZoneServer.NetConfig.WorldNetConfig.WorldID,
				"ISYA", // World Name TODO: Add this to WorldConfiguration/NetworkConfiguration
				ZoneServer.ZoneId, // ZoneNo
				clientIP, // Client Listen IP
				(ushort)ZoneServer.ZoneNetConfig.ListenPort // Client Listen Port
				).Send(connection);
		}

		internal static void NC_MISC_S2SCONNECTION_ACK(NetworkMessage message, NetworkConnection connection)
		{
			var echoData = message.ReadByte();
			var error = message.ReadUInt16();
		}
	}
}
