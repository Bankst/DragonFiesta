using DFEngine.Network;
using DFEngine.Network.Protocols;

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

		internal static void NC_MISC_S2SCONNECTION_RDY(NetworkMessage message, NetworkConnection connection)
		{
			new PROTO_NC_MISC_S2SCONNECTION_REQ(
				NetworkConnectionType.NCT_WORLDMANAGER, // From 
				NetworkConnectionType.NCT_LOGIN, // To
				ServerMain.NetConfig.WorldNetConfig.WorldID, // World ID
				"Isya", // World Name TODO: Add this to WorldConfiguration/NetworkConfiguration
				0, // ZoneNo TODO: do this programatically?
				ServerMain.NetConfig.WorldNetConfig.ListenIP, // Client Listen IP
				(ushort)ServerMain.NetConfig.WorldNetConfig.ListenPort // Client Listen Port
			).Send(connection);
		}

		internal static void NC_MISC_S2SCONNECTION_ACK(NetworkMessage message, NetworkConnection connection)
		{
			Data.LoadSHNs();
			Data.LoadShineTables();
		}
	}
}
