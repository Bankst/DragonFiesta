using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;

using DFEngine;
using DFEngine.Config;
using DFEngine.Database;
using DFEngine.Logging;
using DFEngine.Network;
using DFEngine.Server;
using DFEngine.Threading;
using DFEngine.Utils;

using WorldManagerServer.Handlers;
using WorldManagerServer.Util.Console;

namespace WorldManagerServer
{
	public class WorldManagerServer : ServerMainBase
	{
		public new static WorldManagerServer InternalInstance { get; private set; }

		// Global objects
		internal static List<NetworkTransfer> Transfers = new List<NetworkTransfer>();
		internal static WorldConsoleTitle Title { get; set; }
		internal static List<Zone> Zones = new List<Zone>();

		// Configuration
		internal static NetworkConfiguration NetConfig;
		internal static DatabaseConfiguration DbConfig;
		internal static WorldConfiguration WorldConfig;

		// Database
		internal static SqlConnection CharDb;

		// Networking
		internal static NetworkServer ClientServer = new NetworkServer(NetworkConnectionType.NCT_CLIENT);
		internal static NetworkServer ZoneServer = new NetworkServer(NetworkConnectionType.NCT_ZONE);
		internal static NetworkConnection LoginServer = new NetworkConnection(NetworkConnectionType.NCT_LOGIN);
		internal static NetworkConnection GameLogServer = new NetworkConnection(NetworkConnectionType.NCT_DB_GAMELOG);

		public WorldManagerServer() : base(ServerType.World)
		{
			Title = new WorldConsoleTitle();
			Title.Update();
		}

		public static void Initialize()
		{
			InternalInstance = new WorldManagerServer();
			InternalInstance.WriteConsoleLogo();

			EngineLog.Write(EngineLogLevel.Startup, "Starting WorldManagerServer");

			var stopwatch = new Stopwatch();
			stopwatch.Start();

			// Configuration
			if (!NetworkConfiguration.Load(out var netConfigMsg))
			{
				throw new StartupException(netConfigMsg);
			}
			NetConfig = NetworkConfiguration.Instance;

			if (!DatabaseConfiguration.Load(out var dbConfigMsg))
			{
				throw new StartupException(dbConfigMsg);
			}
			DbConfig = DatabaseConfiguration.Instance;

			if (!WorldConfiguration.Load(out var loginConfigMsg))
			{
				throw new StartupException(loginConfigMsg);
			}
			WorldConfig = WorldConfiguration.Instance;

			// Database
			if (!DB.AddManager(DatabaseType.Character, DatabaseConfiguration.Instance))
			{
				throw new StartupException("Database connection failure! See above error.");
			}

			CharDb = DB.GetDatabaseClient(DatabaseType.Character).Connection;

			// Handlers
			StoreHandlers();

			// Data
			WorldData.LoadSHNs();
			WorldData.LoadScripts();

			// Networking
			LoginServer.Connect(NetConfig.LoginNetConfig.S2SListenIP, (ushort) NetConfig.LoginNetConfig.S2SListenPort);
			ZoneServer.Listen(NetConfig.WorldNetConfig.S2SListenIP, (ushort) NetConfig.WorldNetConfig.S2SListenPort);
			ClientServer.Listen(NetConfig.WorldNetConfig.ListenIP, (ushort) NetConfig.WorldNetConfig.ListenPort);
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

			// Console commands?
		}

		private static void StoreHandlers()
		{
			NetworkMessageHandler.Store(NetworkCommand.NC_MISC_SEED_ACK, MiscHandlers.NC_MISC_SEED_ACK);
			NetworkMessageHandler.Store(NetworkCommand.NC_MISC_S2SCONNECTION_RDY, MiscHandlers.NC_MISC_S2SCONNECTION_RDY);
			NetworkMessageHandler.Store(NetworkCommand.NC_MISC_S2SCONNECTION_REQ, MiscHandlers.NC_MISC_S2SCONNECTION_REQ);
			NetworkMessageHandler.Store(NetworkCommand.NC_MISC_S2SCONNECTION_ACK, MiscHandlers.NC_MISC_S2SCONNECTION_ACK);
			NetworkMessageHandler.Store(NetworkCommand.NC_MISC_GAMETIME_REQ, MiscHandlers.NC_MISC_GAMETIME_REQ);

			NetworkMessageHandler.Store(NetworkCommand.NC_USER_WILLLOGIN_REQ, UserHandlers.NC_USER_WILLLOGIN_REQ);
			NetworkMessageHandler.Store(NetworkCommand.NC_USER_LOGINWORLD_REQ, UserHandlers.NC_USER_LOGINWORLD_REQ);
			NetworkMessageHandler.Store(NetworkCommand.NC_USER_NORMALLOGOUT_CMD, UserHandlers.NC_USER_NORMALLOGOUT_CMD);
			NetworkMessageHandler.Store(NetworkCommand.NC_USER_WILL_WORLD_SELECT_REQ, UserHandlers.NC_USER_WILL_WORLD_SELECT_REQ);

			NetworkMessageHandler.Store(NetworkCommand.NC_AVATAR_CREATE_REQ, AvatarHandlers.NC_AVATAR_CREATE_REQ);
			NetworkMessageHandler.Store(NetworkCommand.NC_AVATAR_ERASE_REQ, AvatarHandlers.NC_AVATAR_ERASE_REQ);

			NetworkMessageHandler.Store(NetworkCommand.NC_CHAR_LOGIN_REQ, CharacterHandlers.NC_CHAR_LOGIN_REQ);

			NetworkMessageHandler.Store(NetworkCommand.NC_CHAR_OPTION_GET_SHORTCUTSIZE_REQ, CharOptionHandlers.NC_CHAR_OPTION_GET_SHORTCUTSIZE_REQ);
			NetworkMessageHandler.Store(NetworkCommand.NC_CHAR_OPTION_GET_WINDOWPOS_REQ, CharOptionHandlers.NC_CHAR_OPTION_GET_WINDOWPOS_REQ);

			NetworkMessageHandler.Store(NetworkCommand.NC_PRISON_GET_REQ, PrisonHandlers.NC_PRISON_GET_REQ);
		}

		private static void Update(long now)
		{
			// Remove map transfers that have been waiting for more than allowed timeout.
			var mapTimeouts = Transfers.Filter(t => now - t.CreateTime >= (int)MessageRequestTimeOuts.ZONE_TRANSFER_MAP);
			mapTimeouts.ForBackwards(Transfers.RemoveSafe);

			// Update console title
			Title.Update();
		}
	}
}
