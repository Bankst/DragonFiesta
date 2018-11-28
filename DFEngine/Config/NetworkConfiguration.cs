using System;
using System.ComponentModel;
using System.Linq;
using DFEngine.Logging;

namespace DFEngine.Config
{
	public class NetworkConfiguration : Configuration<NetworkConfiguration>
	{
		public LoginNetworkConfiguration LoginNetConfig { get; protected set; } = new LoginNetworkConfiguration();
		public WorldNetworkConfiguration WorldNetConfig { get; protected set; } = new WorldNetworkConfiguration();

		// TODO: Better method for multiple zones?
		// getting a single ZoneNetworkConfiguration would be done with LINQ
		// i.e.   ZoneNetworkConfiguration ZoneNetworkConfig = ZoneNetworkConfigs.First(x => x.ZoneID == 0);
		public ZoneNetworkConfiguration[] ZoneNetworkConfigs { get; protected set; } = {
			new ZoneNetworkConfiguration()
		};

		public GameLogNetworkConfiguration GameLogNetConfig { get; protected set; } = new GameLogNetworkConfiguration();

		public static NetworkConfiguration Instance { get; set; }

		public static bool Load(out string message)
		{
			Instance = Initialize(out message);
			return message == string.Empty;
		}
	}

	public class LoginNetworkConfiguration
	{
		public string ListenIP { get; protected set; } = "127.0.0.1";
		public string ExternalIP { get; protected set; } = "127.0.0.1";
		public int ListenPort { get; protected set; } = 9010;
		public int MaxClientConnections { get; protected set; } = 25;
		public int MaxWorldConnections { get; protected set; } = 5;
		public string S2SListenIP { get; protected set; } = "127.0.0.1";
		public int S2SListenPort { get; protected set; } = 9011;
	}

	public class WorldNetworkConfiguration
	{
		public string ListenIP { get; protected set; } = "127.0.0.1";
		public string ExternalIP { get; protected set; } = "127.0.0.1";
		public int ListenPort { get; protected set; } = 9110;
		public int MaxClientConnections { get; protected set; } = 25;
		public int MaxZoneConnections { get; protected set; } = 5;
		public string S2SListenIP { get; protected set; } = "127.0.0.1";
		public int S2SListenPort { get; protected set; } = 9111;
		public byte WorldID { get; protected set; } = 0;
	}

	public class ZoneNetworkConfiguration
	{
		public string ListenIP { get; protected set; } = "127.0.0.1";
		public string ExternalIP { get; protected set; } = "127.0.0.1";
		public int ListenPort { get; protected set; } = 9210;
		public int MaxClientConnections { get; protected set; } = 200;
		public string S2SListenIP { get; protected set; } = "127.0.0.1";
		public int S2SListenPort { get; protected set; } = 9211;
		public byte ZoneID { get; protected set; } = 0;
	}

	public class GameLogNetworkConfiguration
	{
		public string S2SListenIP { get; protected set; } = "127.0.0.1";
		public int S2SListenPort { get; protected set; } = 9311;
	}
}
