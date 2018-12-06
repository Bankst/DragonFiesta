using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;

using DFEngine;
using DFEngine.Accounts;
using DFEngine.Config;
using DFEngine.Database;
using DFEngine.Logging;
using DFEngine.Network;
using DFEngine.Network.Protocols;
using DFEngine.Server;
using DFEngine.Threading;
using DFEngine.Utils;

using LoginServer.Handlers;
using LoginServer.Util.Console;

namespace LoginServer
{
	public class LoginServer : ServerMainBase
	{
		public new static LoginServer InternalInstance { get; private set; }

		// Global Objects
		internal static Dictionary<string, Account> CachedAccounts = new Dictionary<string, Account>();
		internal static List<NetworkTransfer> Transfers = new List<NetworkTransfer>();
		internal static List<World> Worlds = new List<World>();
		internal static LoginConsoleTitle Title { get; set; }

		// Configuration
		internal static NetworkConfiguration NetConfig;
		internal static DatabaseConfiguration DbConfig;
		internal static LoginConfiguration LoginConfig;

		// Database
		internal static SqlConnection AccountDb;

		// Networking
		internal static NetworkServer ClientServer = new NetworkServer(NetworkConnectionType.NCT_CLIENT);
		internal static NetworkServer WorldServer = new NetworkServer(NetworkConnectionType.NCT_WORLDMANAGER);
		internal static NetworkConnection GameLogServer = new NetworkConnection(NetworkConnectionType.NCT_DB_GAMELOG);


		public LoginServer() : base(ServerType.Login)
		{
			Title = new LoginConsoleTitle();
			Title.Update();
		}

		public static void Initialize()
		{
			InternalInstance = new LoginServer();
			InternalInstance.WriteConsoleLogo();

			EngineLog.Write(EngineLogLevel.Startup, "Starting LoginServer");

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

			NetworkMessageHandler.Store(NetworkCommand.NC_USER_CLIENT_VERSION_CHECK_REQ, UserHandlers.NC_USER_CLIENT_VERSION_CHECK_REQ);
			NetworkMessageHandler.Store(NetworkCommand.NC_USER_US_LOGIN_REQ, UserHandlers.NC_USER_US_LOGIN_REQ);
			NetworkMessageHandler.Store(NetworkCommand.NC_USER_XTRAP_REQ, UserHandlers.NC_USER_XTRAP_REQ);
			NetworkMessageHandler.Store(NetworkCommand.NC_USER_WORLD_STATUS_REQ, UserHandlers.NC_USER_WORLD_STATUS_REQ);
			NetworkMessageHandler.Store(NetworkCommand.NC_USER_WORLDSELECT_REQ, UserHandlers.NC_USER_WORLDSELECT_REQ);
			NetworkMessageHandler.Store(NetworkCommand.NC_USER_NORMALLOGOUT_CMD, UserHandlers.NC_USER_NORMALLOGOUT_CMD);
			NetworkMessageHandler.Store(NetworkCommand.NC_USER_LOGIN_WITH_OTP_REQ, UserHandlers.NC_USER_LOGIN_WITH_OTP_REQ);
			NetworkMessageHandler.Store(NetworkCommand.NC_USER_WILLLOGIN_ACK, UserHandlers.NC_USER_WILLLOGIN_ACK);
		}

		private static void Update(long now)
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
