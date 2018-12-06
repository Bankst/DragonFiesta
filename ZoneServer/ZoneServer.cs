using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using DFEngine;
using DFEngine.Config;
using DFEngine.Database;
using DFEngine.Logging;
using DFEngine.Network;
using DFEngine.Server;
using DFEngine.Threading;
using DFEngine.Utils;
using ZoneServer.Handlers;
using ZoneServer.Util.Console;

namespace ZoneServer
{
	public class ZoneServer : ServerMainBase
	{
		public new static ZoneServer InternalInstance { get; private set; }
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
		internal static NetworkConnection WorldServer = new NetworkConnection(NetworkConnectionType.NCT_WORLDMANAGER);
		internal static NetworkConnection GameLogServer = new NetworkConnection(NetworkConnectionType.NCT_DB_GAMELOG);
		

		public ZoneServer() : base(ServerType.Zone)
		{
			Title = new ZoneConsoleTitle();
			Title.Update();
		}

		public static void Initialize(byte zoneId = 0) // Default to zone 0
		{
			ZoneId = zoneId;

			InternalInstance = new ZoneServer();
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


			// Database
			if (!DB.AddManager(DatabaseType.Character, DbConfig))
			{
				throw new StartupException("Database connection failure! See above error.");
			}
			CharDb = DB.GetDatabaseClient(DatabaseType.Character).Connection;


			// Handlers
			StoreHandlers();

			// Data
			ZoneData.LoadShineTables();

			// Networking
			WorldServer.Connect(NetConfig.WorldNetConfig.S2SListenIP, (ushort) NetConfig.WorldNetConfig.S2SListenPort);
			ClientServer.Listen(ZoneNetConfig.ListenIP, (ushort) ZoneNetConfig.ListenPort);
			// TODO: GameLogServer
			// GameLogServer.Connect(NetConfig.GameLogNetConfig.S2SListenIP, (ushort)NetConfig.GameLogNetConfig.S2SListenPort);

			stopwatch.Stop();
			EngineLog.Write(EngineLogLevel.Startup, $"Time taken to start: {stopwatch.ElapsedMilliseconds}ms");

			// Main server loop
			new Thread(() =>
			{
				while (true)
				{
					Update(Time.Milliseconds);
					Thread.Sleep(10);
				}

			}).Start();
		}

		private static void StoreHandlers()
		{
			NetworkMessageHandler.Store(NetworkCommand.NC_MISC_SEED_ACK, MiscHandlers.NC_MISC_SEED_ACK);
			NetworkMessageHandler.Store(NetworkCommand.NC_MISC_S2SCONNECTION_RDY, MiscHandlers.NC_MISC_S2SCONNECTION_RDY);;
			NetworkMessageHandler.Store(NetworkCommand.NC_MISC_S2SCONNECTION_ACK, MiscHandlers.NC_MISC_S2SCONNECTION_ACK);
		}

		private static void Update(long now)
		{
			var mapTimeouts = Transfers.Filter(t => now - t.CreateTime >= (int)MessageRequestTimeOuts.ZONE_TRANSFER_MAP);
			mapTimeouts.ForBackwards(Transfers.RemoveSafe);

			// Update console title
			Title.Update();
		}
	}
}
