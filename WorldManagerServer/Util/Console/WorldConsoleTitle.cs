using System;
using System.Collections.Generic;
using System.Text;
using DFEngine.Utils.ServerConsole;

namespace WorldManagerServer.Util.Console
{
	public class WorldConsoleTitle : ConsoleTitle
	{
		private const string TitleString = "DragonFiesta.WorldManagerServer ID [{0}] Zones [{1}/{2}] Players [{3}/{4}]";

		private static int PlayerCount => WorldManagerServer.ClientServer.Connections.Count;

		private static int WorldId => WorldManagerServer.NetConfig != null ? WorldManagerServer.NetConfig.WorldNetConfig.WorldID : -1;

		private static int MaxPlayers => WorldManagerServer.NetConfig != null ? WorldManagerServer.NetConfig.WorldNetConfig.MaxClientConnections : 0;

		private static int ActiveZone => WorldManagerServer.ZoneServer.Connections.Count;

		private static int MaxZones => WorldManagerServer.NetConfig != null ? WorldManagerServer.NetConfig.WorldNetConfig.MaxZoneConnections : 0;

		public void Update() => base.Update(TitleString, WorldId, ActiveZone, MaxZones, PlayerCount, MaxPlayers);
	}
}
