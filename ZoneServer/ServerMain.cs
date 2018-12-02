using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DFEngine;
using DFEngine.Config;
using DFEngine.Database;
using DFEngine.Logging;
using DFEngine.Network;
using DFEngine.Server;
using ZoneServer.Util.Console;

namespace ZoneServer
{
	public class ServerMain : ServerMainBase
	{
		public new static ServerMain InternalInstance { get; private set; }
		public static byte ZoneId { get; private set; }

		// Global Objects
		internal static List<NetworkTransfer> Transfers = new List<NetworkTransfer>();
		internal static ZoneConsoleTitle Title { get; set; }

		// Configuration
		internal static NetworkConfiguration NetConfig;
		internal static DatabaseConfiguration DbConfig;
		internal static ZoneNetworkConfiguration ZoneNetConfig;
		internal static ZoneConfiguration ZoneConfig;

		// Database
		internal static SqlConnection CharDb;

		// Networking
		internal static NetworkServer ClientServer = new NetworkServer(NetworkConnectionType.NCT_CLIENT);


		public ServerMain() : base(ServerType.Zone)
		{
			Title = new ZoneConsoleTitle();
			Title.Update();
		}

		public static void Initialize(byte zoneId = 0) // Default to zone 0
		{
			ZoneId = zoneId;

			InternalInstance = new ServerMain();
			InternalInstance.WriteConsoleLogo();

			EngineLog.Write(EngineLogLevel.Startup, $"Starting Zone0{ZoneId}Server");

			var stopwatch = new Stopwatch();
			stopwatch.Start();
			
			// Configuration
			if (!NetworkConfiguration.Load(out var netConfigMsg))
			{
				throw new StartupException(netConfigMsg);
			}
			NetConfig = NetworkConfiguration.Instance;

			if ((ZoneNetConfig = NetConfig.ZoneNetworkConfigs.FirstOrDefault(z => z.ZoneID == zoneId)) == null)
			{
				throw new StartupException("ZoneID Not found in config!");
			}

			if (!DatabaseConfiguration.Load(out var dbConfigMsg))
			{
				throw new StartupException(dbConfigMsg);
			}
			DbConfig = DatabaseConfiguration.Instance;

			if (!ZoneConfiguration.Load(out var zoneConfigMsg))
			{
				throw new StartupException(zoneConfigMsg);
			}
			ZoneConfig = ZoneConfiguration.Instance;

			if (!DB.AddManager(DatabaseType.Character, DbConfig))
			{
				throw new StartupException("Database connection failure! See above error.");
			}
			CharDb = DB.GetDatabaseClient(DatabaseType.Character).Connection;
		}
	}
}
