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
	public class WorldManagerServer : BaseApplication
	{
		public static byte WorldId { get; private set; }

		// Global objects
		internal static WorldConsoleTitle Title { get; set; }
		internal static List<NetworkTransfer> Transfers = new List<NetworkTransfer>();
		internal static List<Zone> Zones = new List<Zone>();

		// Configuration
		internal static WorldConfiguration WorldConfig;

		// Database
		internal static SqlConnection CharDb;

		// Networking
		internal static NetworkServer ClientServer = new NetworkServer(NetworkConnectionType.NCT_CLIENT);
		internal static NetworkServer ZoneServer = new NetworkServer(NetworkConnectionType.NCT_ZONE);
		internal static NetworkConnection LoginServer = new NetworkConnection(NetworkConnectionType.NCT_LOGIN);
		internal static NetworkConnection GameLogServer = new NetworkConnection(NetworkConnectionType.NCT_DB_GAMELOG);


		public static void Run(byte worldId = 0)
		{
			WorldId = worldId;
			InternalInstance = new WorldManagerServer();
			InternalInstance.Initialize(ServerType.World);
		}

		protected override void Start()
		{
			Title = new WorldConsoleTitle();
			Title.Update();

			// Configuration
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
			if (!WorldData.LoadSHNs())
			{
				throw new StartupException("Failed to load SHNs! See above error.");
			}

			if (!WorldData.LoadScripts())
			{
				throw new StartupException("Failed to load Scripts! See above error.");
			}

			// Networking
			LoginServer.Connect(NetConfig.LoginNetConfig.S2SListenIP, (ushort) NetConfig.LoginNetConfig.S2SListenPort);
			ZoneServer.Listen(NetConfig.WorldNetConfig.S2SListenIP, (ushort) NetConfig.WorldNetConfig.S2SListenPort);
			ClientServer.Listen(NetConfig.WorldNetConfig.ListenIP, (ushort) NetConfig.WorldNetConfig.ListenPort);
			// TODO: GameLogServer
			// GameLogServer.Connect(NetConfig.GameLogNetConfig.S2SListenIP, (ushort)NetConfig.GameLogNetConfig.S2SListenPort);
		}

		private static void StoreHandlers()
		{
			NetworkMessageHandler.Store(NetworkCommand.NC_MISC_SEED_ACK, MiscHandlers.NC_MISC_SEED_ACK);
			NetworkMessageHandler.Store(NetworkCommand.NC_MISC_S2SCONNECTION_RDY, MiscHandlers.NC_MISC_S2SCONNECTION_RDY);
			NetworkMessageHandler.Store(NetworkCommand.NC_MISC_S2SCONNECTION_REQ, MiscHandlers.NC_MISC_S2SCONNECTION_REQ);
			NetworkMessageHandler.Store(NetworkCommand.NC_MISC_S2SCONNECTION_ACK, MiscHandlers.NC_MISC_S2SCONNECTION_ACK);
			NetworkMessageHandler.Store(NetworkCommand.NC_MISC_GAMETIME_REQ, MiscHandlers.NC_MISC_GAMETIME_REQ);
            NetworkMessageHandler.Store(NetworkCommand.NC_MISC_SERVER_TIME_NOTIFY_CMD, MiscHandlers.NC_MISC_SERVER_TIME_NOTIFY_CMD);

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

			NetworkMessageHandler.Store(NetworkCommand.NC_MAP_LINKEND_CMD, MapHandlers.NC_MAP_LINKEND_CMD);
		}

		protected override void Update(long now)
		{
			// Remove map transfers that have been waiting for more than allowed timeout.
			var mapTimeouts = Transfers.Filter(t => now - t.CreateTime >= (int)MessageRequestTimeOuts.ZONE_TRANSFER_MAP);
			mapTimeouts.ForBackwards(Transfers.RemoveSafe);

			// Update console title
			Title.Update();
		}
	}
}
