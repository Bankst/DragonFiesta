using System;
using System.Collections.Generic;
using System.Text;
using DFEngine.Utils.ServerConsole;

namespace ZoneServer.Util.Console
{
	public class ZoneConsoleTitle : ConsoleTitle
	{
		private const string ZoneConstString = "DragonFiesta.ZoneServer [{0}] Players [{1}/{2}] Remotes [{3}]";

		private static int ZoneId => ZoneServer.ZoneId;

		private static int PlayerCount => ZoneServer.ClientServer.Connections.Count;

		private static int MaxConnection => ZoneServer.ZoneNetConfig != null ? ZoneServer.ZoneNetConfig.MaxClientConnections : 0;

		private static int RemoteZoneCount => ZoneServer.NetConfig != null ? ZoneServer.NetConfig.ZoneNetworkConfigs.Length : 0;

		public void Update()=> base.Update(ZoneConstString, ZoneId, PlayerCount, MaxConnection, RemoteZoneCount);
	}
}
