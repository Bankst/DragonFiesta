using System;
using System.Collections.Generic;
using System.Text;
using DFEngine.Utils.ServerConsole;

namespace ZoneServer.Util.Console
{
	public class ZoneConsoleTitle : ConsoleTitle
	{
		private const string ZoneConstString = "DragonFiesta.ZoneServer [{0}] Players [{1}/{2}] Remotes [{3}]";

		private static int ZoneId => ServerMain.ZoneId;

		private static int PlayerCount => ServerMain.ClientServer.Connections.Count;

		private static int MaxConnection => ServerMain.ZoneNetConfig != null ? ServerMain.ZoneNetConfig.MaxClientConnections : 0;

		private static int RemoteZoneCount => ServerMain.NetConfig != null ? ServerMain.NetConfig.ZoneNetworkConfigs.Length : 0;

		public void Update()=> base.Update(ZoneConstString, ZoneId, PlayerCount, MaxConnection, RemoteZoneCount);
	}
}
