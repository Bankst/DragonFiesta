using System;
using System.Collections.Generic;
using System.Text;
using DFEngine.Utils.ServerConsole;

namespace WorldManagerServer.Util.Console
{
	public class WorldConsoleTitle : ConsoleTitle
	{
		private const string TitleString = "DragonFiesta.WorldManagerServer ID [{0}] Zones [{1}/{2}] Players [{3}/{4}]";

		private static int PlayerCount => ServerMain.ClientServer.Connections.Count;

		private static int WorldId => ServerMain.NetConfig != null ? ServerMain.NetConfig.WorldNetConfig.WorldID : -1;

		private static int MaxPlayers => ServerMain.NetConfig != null ? ServerMain.NetConfig.WorldNetConfig.MaxClientConnections : 0;

		private static int ActiveZone => ServerMain.ZoneServer.Connections.Count;

		private static int MaxZones => ServerMain.NetConfig != null ? ServerMain.NetConfig.WorldNetConfig.MaxZoneConnections : 0;

		public void Update() => base.Update(TitleString, WorldId, ActiveZone, MaxZones, PlayerCount, MaxPlayers);
	}
}
