using System;
using System.Collections.Generic;
using System.Text;
using DFEngine;
using DFEngine.Network;

namespace WorldManagerServer.Handlers
{
	public static class MapHandlers
	{
		internal static void NC_MAP_LINKEND_CMD(NetworkMessage message, NetworkConnection client)
		{
			var handle = message.ReadUInt16();
			var mapIndx = message.ReadString(12);

			var charClient = WorldManagerServer.ClientServer.Connections.First(con => con.Handle == handle);
			if (charClient == null)
			{
				return;
			}

			if (!WorldData.MapZones.TryGetValue(mapIndx, out var zoneNo))
			{
				charClient.Disconnect();
				return;
			}

			var zone = WorldManagerServer.Zones.First(z => z.Number == zoneNo);
			if (zone == null)
			{
				charClient.Disconnect();
				return;
			}

			charClient.Avatar.FinalizeLogin(mapIndx, zone);
		}
	}
}
