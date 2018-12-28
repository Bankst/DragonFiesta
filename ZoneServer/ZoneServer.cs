using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

using DFEngine;
using DFEngine.Config;
using DFEngine.Database;
using DFEngine.Logging;
using DFEngine.Network;
using DFEngine.Server;
using DFEngine.Threading;

using ZoneServer.Handlers;
using ZoneServer.Util.Console;

namespace ZoneServer
{
	public class ZoneServer : BaseApplication
	{
		public static byte ZoneId { get; private set; }

		// Global Objects
		internal static List<NetworkTransfer> Transfers = new List<NetworkTransfer>();
		internal static ZoneConsoleTitle Title { get; set; }

		// Configuration
		internal static ZoneNetworkConfiguration ZoneNetConfig;
		internal static SingleZoneConfiguration ZoneConfig;

		// Database
		internal static SqlConnection CharDb;

		// Networking
		internal static NetworkServer ClientServer = new NetworkServer(NetworkConnectionType.NCT_CLIENT);
		internal static NetworkConnection WorldServer = new NetworkConnection(NetworkConnectionType.NCT_WORLDMANAGER);
		internal static NetworkConnection GameLogServer = new NetworkConnection(NetworkConnectionType.NCT_DB_GAMELOG);

		public static void Run(byte zoneId = 0)
		{
			ZoneId = zoneId;
			InternalInstance = new ZoneServer();
			InternalInstance.Initialize(ServerType.Zone);
		}

		protected override void Start() // Default to zone 0
		{
			Title = new ZoneConsoleTitle();
			Title.Update();

			EngineLog.Write(EngineLogLevel.Startup, $"Starting Zone0{ZoneId}Server");

			// Configuration
			if ((ZoneNetConfig = NetConfig.ZoneNetworkConfigs.FirstOrDefault(z => z.ZoneID == ZoneId)) == null)
			{
				throw new StartupException("ZoneID Not found in config!");
			}

			if (!ZoneConfiguration.Load(out var zoneConfigMsg))
			{
				throw new StartupException(zoneConfigMsg);
			}
			var tempZoneConfig = ZoneConfiguration.Instance;

			if ((ZoneConfig = tempZoneConfig.Zones.FirstOrDefault(z => z.ZoneID == ZoneId)) == null)
			{
				throw new StartupException("ZoneID Not found in config!");
			}

			// Database
			if (!DB.AddManager(DatabaseType.Character, DbConfig))
			{
				throw new StartupException("Database connection failure! See above error.");
			}
			CharDb = DB.GetDatabaseClient(DatabaseType.Character).Connection;
			
			// Handlers
			StoreHandlers();

			// Data
			if (!ZoneData.CalculateSHNChecksums())
			{
				throw new StartupException("Failed to calculate SHN Checksums! See above error.");
			}

			if (!ZoneData.LoadSHNs())
			{
				throw new StartupException("Failed to load SHNs! See above error.");
			}

			if (!ZoneData.LoadShineTables())
			{
				throw new StartupException("Failed to load Shine Tables! See above error.");
			}

			if (!ZoneData.LoadMaps())
			{
				throw new StartupException("Failed to load Maps! See above error.");
			}
			
			// Networking
			WorldServer.Connect(NetConfig.WorldNetConfig.S2SListenIP, (ushort) NetConfig.WorldNetConfig.S2SListenPort);
			ClientServer.Listen(ZoneNetConfig.ListenIP, (ushort) ZoneNetConfig.ListenPort);
			// TODO: GameLogServer
			// GameLogServer.Connect(NetConfig.GameLogNetConfig.S2SListenIP, (ushort)NetConfig.GameLogNetConfig.S2SListenPort);
		}

		private static void StoreHandlers()
		{
			NetworkMessageHandler.Store(NetworkCommand.NC_MISC_SEED_ACK, MiscHandlers.NC_MISC_SEED_ACK);
			NetworkMessageHandler.Store(NetworkCommand.NC_MISC_S2SCONNECTION_RDY, MiscHandlers.NC_MISC_S2SCONNECTION_RDY);
			NetworkMessageHandler.Store(NetworkCommand.NC_MISC_S2SCONNECTION_ACK, MiscHandlers.NC_MISC_S2SCONNECTION_ACK);

			NetworkMessageHandler.Store(NetworkCommand.NC_USER_NORMALLOGOUT_CMD, UserHandlers.NC_USER_NORMALLOGOUT_CMD);

			NetworkMessageHandler.Store(NetworkCommand.NC_CHAR_LOGOUTREADY_CMD, CharacterHandlers.NC_CHAR_LOGOUTREADY_CMD);
			NetworkMessageHandler.Store(NetworkCommand.NC_CHAR_LOGOUTCANCEL_CMD, CharacterHandlers.NC_CHAR_LOGOUTCANCEL_CMD);

			NetworkMessageHandler.Store(NetworkCommand.NC_MAP_LOGIN_REQ, MapHandlers.NC_MAP_LOGIN_REQ);
			NetworkMessageHandler.Store(NetworkCommand.NC_MAP_LOGINCOMPLETE_CMD, MapHandlers.NC_MAP_LOGINCOMPLETE_CMD);

			NetworkMessageHandler.Store(NetworkCommand.NC_ACT_MOVERUN_CMD, ActionHandlers.NC_ACT_MOVERUN_CMD);
			NetworkMessageHandler.Store(NetworkCommand.NC_ACT_MOVEWALK_CMD, ActionHandlers.NC_ACT_MOVEWALK_CMD);
			NetworkMessageHandler.Store(NetworkCommand.NC_ACT_STOP_REQ, ActionHandlers.NC_ACT_STOP_REQ);
			NetworkMessageHandler.Store(NetworkCommand.NC_ACT_CHAT_REQ, ActionHandlers.NC_ACT_CHAT_REQ);
		}

		protected override void Update(long now)
		{
			var mapTimeouts = Transfers.Filter(t => now - t.CreateTime >= (int)MessageRequestTimeOuts.ZONE_TRANSFER_MAP);
			mapTimeouts.ForBackwards(Transfers.RemoveSafe);

			// Update console title
			Title.Update();
		}
	}
}
