using System.Collections.Generic;
using System.Data.SqlClient;

using DFEngine;
using DFEngine.Accounts;
using DFEngine.Config;
using DFEngine.Database;
using DFEngine.Network;
using DFEngine.Network.Protocols;
using DFEngine.Server;
using DFEngine.Threading;

using LoginServer.Handlers;
using LoginServer.Util.Console;

namespace LoginServer
{
	public class LoginServer : BaseApplication
	{
		// Global Objects
		internal static Dictionary<string, Account> CachedAccounts = new Dictionary<string, Account>();
		internal static List<NetworkTransfer> Transfers = new List<NetworkTransfer>();
		internal static List<World> Worlds = new List<World>();
		internal static LoginConsoleTitle Title { get; set; }

		// Configuration
		internal static LoginConfiguration LoginConfig;

		// Database
		internal static SqlConnection AccountDb;

		// Networking
		internal static NetworkServer ClientServer = new NetworkServer(NetworkConnectionType.NCT_CLIENT);
		internal static NetworkServer WorldServer = new NetworkServer(NetworkConnectionType.NCT_WORLDMANAGER);
		internal static NetworkConnection GameLogServer = new NetworkConnection(NetworkConnectionType.NCT_DB_GAMELOG);

		public static void Run()
		{
			InternalInstance = new LoginServer();
			InternalInstance.Initialize(ServerType.Login);
		}

		protected override void Start()
		{
			Title = new LoginConsoleTitle();
			Title.Update();

			// Configuration
			if (!LoginConfiguration.Load(out var loginConfigMsg))
			{
				throw new StartupException(loginConfigMsg);
			}
			LoginConfig = LoginConfiguration.Instance;

			// Database
			if (!DB.AddManager(DatabaseType.Account, DatabaseConfiguration.Instance))
			{
				throw new StartupException("Database connection failure! See above error.");
			}
			AccountDb = DB.GetDatabaseClient(DatabaseType.Account).Connection;

			// Handlers
			StoreHandlers();

			// Networking
			WorldServer.Listen(NetConfig.LoginNetConfig.S2SListenIP, (ushort)NetConfig.LoginNetConfig.S2SListenPort);
			ClientServer.Listen(NetConfig.LoginNetConfig.ListenIP, (ushort)NetConfig.LoginNetConfig.ListenPort);
			// TODO: gamelogserver
			// GameLogServer.Connect(NetConfig.GameLogNetConfig.S2SListenIP, (ushort)NetConfig.GameLogNetConfig.S2SListenPort);
		}


		private static void StoreHandlers()
		{
			NetworkMessageHandler.Store(NetworkCommand.NC_MISC_SEED_ACK, MiscHandlers.NC_MISC_SEED_ACK);
			NetworkMessageHandler.Store(NetworkCommand.NC_MISC_S2SCONNECTION_RDY, MiscHandlers.NC_MISC_S2SCONNECTION_RDY);
			NetworkMessageHandler.Store(NetworkCommand.NC_MISC_S2SCONNECTION_REQ, MiscHandlers.NC_MISC_S2SCONNECTION_REQ);

			NetworkMessageHandler.Store(NetworkCommand.NC_USER_CLIENT_VERSION_CHECK_REQ, UserHandlers.NC_USER_CLIENT_VERSION_CHECK_REQ);
			NetworkMessageHandler.Store(NetworkCommand.NC_USER_US_LOGIN_REQ, UserHandlers.NC_USER_US_LOGIN_REQ);
			NetworkMessageHandler.Store(NetworkCommand.NC_USER_XTRAP_REQ, UserHandlers.NC_USER_XTRAP_REQ);
			NetworkMessageHandler.Store(NetworkCommand.NC_USER_WORLD_STATUS_REQ, UserHandlers.NC_USER_WORLD_STATUS_REQ);
			NetworkMessageHandler.Store(NetworkCommand.NC_USER_WORLDSELECT_REQ, UserHandlers.NC_USER_WORLDSELECT_REQ);
			NetworkMessageHandler.Store(NetworkCommand.NC_USER_NORMALLOGOUT_CMD, UserHandlers.NC_USER_NORMALLOGOUT_CMD);
			NetworkMessageHandler.Store(NetworkCommand.NC_USER_LOGIN_WITH_OTP_REQ, UserHandlers.NC_USER_LOGIN_WITH_OTP_REQ);
			NetworkMessageHandler.Store(NetworkCommand.NC_USER_WILLLOGIN_ACK, UserHandlers.NC_USER_WILLLOGIN_ACK);
		}

		protected override void Update(long now)
		{
			Worlds.FilteredAction(world => !world.Connection.IsConnected, world => { world.Status = 0x00; });

			// Remove transfers that have been waiting for more than allowed timeout.
			var timeouts = Transfers.Filter(t => now - t.CreateTime >= (int)MessageRequestTimeOuts.LOGIN_TRANSFER_ACCOUNT);
			timeouts.ForBackwards(transfer =>
			{
				new PROTO_NC_USER_LOGINFAIL_ACK(0x49).Send(transfer.Connection);
				Transfers.RemoveSafe(transfer);
			});

			Title.Update();
		}
	}
}
