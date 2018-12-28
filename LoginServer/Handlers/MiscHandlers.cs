using DFEngine.Logging;
using DFEngine.Network;
using DFEngine.Network.Protocols;
using DFEngine.Server;

namespace LoginServer.Handlers
{
	internal static class MiscHandlers
	{
		internal static void NC_MISC_SEED_ACK(NetworkMessage message, NetworkConnection connection)
		{
			connection.SetSeed(message.ReadUInt16());
		}

		internal static void NC_MISC_S2SCONNECTION_REQ(NetworkMessage message, NetworkConnection connection)
		{
			var from = message.ReadByte();
			var to = message.ReadByte();
			var worldNumber = message.ReadByte();
			var worldName = message.ReadString(20);
			var unused = message.ReadByte();
			var ip = message.ReadString(16);
			var port = message.ReadUInt16();
			var key = message.ReadUInt16();

			if (key != from + to)
			{
				SocketLog.Write(SocketLogLevel.Warning, "Invalid key used with S2S connection.");
				connection.Disconnect();
				return;
			}

			if ((NetworkConnectionType) from != NetworkConnectionType.NCT_WORLDMANAGER) return;
			if (LoginServer.Worlds.Exists(world => world.Connection == connection || world.Number == worldNumber))
			{
				return;
			}

			LoginServer.Worlds.Add(new World(connection, worldName, worldNumber, ip, port));
			new PROTO_NC_MISC_S2SCONNECTION_ACK(0, 0).Send(connection);
		}

		internal static void NC_MISC_S2SCONNECTION_RDY(NetworkMessage message, NetworkConnection connection)
		{

		}
	}
}
