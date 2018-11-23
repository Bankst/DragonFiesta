using System;
using System.ComponentModel;
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
		
		public static bool Initialize(out string message)
		{
			message = "";
			try
			{
				Instance = ReadJson();
				if (Instance != null)
				{
					EngineLog.Write(EngineLogLevel.Startup, "Successfully read Network config.");
					return true;
				}

				if (!Write(out var pConfig))
				{
					message = "Failed to create default NetworkConfiguration.";
					return false;
				}
				pConfig.WriteJson();

				EngineLog.Write(EngineLogLevel.Startup, "Successfully created Network config.");
				message = "No NetworkConfiguration found! Please edit generated config.";
				return false;
			}
			catch (Exception ex)
			{
				EngineLog.Write(EngineLogLevel.Exception, "Failed to load Network config:\n {0}", ex);
				message = $"Failed to load NetworkConfiguration: \n {ex.StackTrace}";
				return false;
			}
		}

		private static bool Write(out NetworkConfiguration pConfig)
		{
			pConfig = null;
			try
			{
				pConfig = new NetworkConfiguration();
				return true;
			}
			catch
			{
				return false;
			}
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
		public int MaxConnections { get; protected set; } = 25;
		public string S2SListenIP { get; protected set; } = "127.0.0.1";
		public int S2SListenPort { get; protected set; } = 9111;
		public byte WorldID { get; protected set; } = 0;
	}

	public class ZoneNetworkConfiguration
	{
		public string ListenIP { get; protected set; } = "127.0.0.1";
		public string ExternalIP { get; protected set; } = "127.0.0.1";
		public int ListenPort { get; protected set; } = 9210;
		public int MaxConnections { get; protected set; } = 100;
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
