using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;

using DFEngine;
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
	public class ServerMain : ServerMainBase
	{
		public new static ServerMain InternalInstance { get; private set; }

		internal static Dictionary<string, Account> CachedAccounts = new Dictionary<string, Account>();
		internal static List<NetworkTransfer> Transfers = new List<NetworkTransfer>();
		internal static List<World> Worlds = new List<World>();
		internal static NetworkConfiguration NetConfig;
		internal static DatabaseConfiguration DbConfig;
		internal static LoginConfiguration LoginConfig;

		internal static NetworkServer ClientServer = new NetworkServer(NetworkConnectionType.NCT_CLIENT);
		internal static NetworkServer WorldServer = new NetworkServer(NetworkConnectionType.NCT_WORLDMANAGER);
		internal static NetworkConnection GameLogServer = new NetworkConnection(NetworkConnectionType.NCT_DB_GAMELOG);

		public LoginConsoleTitle Title { get; set; }

		public ServerMain() : base(ServerType.Login)
		{
			Title = new LoginConsoleTitle();
			Title.Update();
		}

		public static void Initialize()
		{
			var stopwatch = new System.Diagnostics.Stopwatch();
			stopwatch.Start();

			InternalInstance = new ServerMain();
			InternalInstance.WriteConsoleLogo();

			EngineLog.Write(EngineLogLevel.Startup, "Starting LoginServer");

			// Configuration
			if (!NetworkConfiguration.Initialize(out var netConfigMsg))
			{
				throw new StartupException(netConfigMsg);
			}
			NetConfig = NetworkConfiguration.Instance;

			if (!DatabaseConfiguration.Initialize(out var dbConfigMsg))
			{
				throw new StartupException(dbConfigMsg);
			}
			DbConfig = DatabaseConfiguration.Instance;

			if (!LoginConfiguration.Initialize(out var loginConfigMsg))
			{
				throw new StartupException(loginConfigMsg);
			}
			LoginConfig = LoginConfiguration.Instance;

			// Database
			if (!DB.AddManager(DatabaseType.Account, DatabaseConfiguration.Instance))
			{
				throw new StartupException("Database connection failure! See above error.");
			}

			// Handlers
			StoreHandlers();

			// Networking
			WorldServer.Listen(NetConfig.WorldNetConfig.S2SListenIP, (ushort)NetConfig.WorldNetConfig.S2SListenPort);
			ClientServer.Listen(NetConfig.LoginNetConfig.ListenIP, (ushort)NetConfig.LoginNetConfig.ListenPort);


			// Main server loop
			new Thread(() =>
			{
				while (true)
				{
					Update(Time.Milliseconds);
					Thread.Sleep(10);
				}

			}).Start();

			stopwatch.Stop();
			EngineLog.Write(EngineLogLevel.Startup, $"Time taken to start: {stopwatch.ElapsedMilliseconds}ms");

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

			// Remove transfers that have been waiting for more that 5 seconds.
			var timeouts = Transfers.Filter(t => now - t.CreateTime >= (int)MessageRequestTimeOuts.LOGIN_TRANSFER_ACCOUNT);
			timeouts.ForBackwards(transfer =>
			{
				new PROTO_NC_USER_LOGINFAIL_ACK(0x49).Send(transfer.Connection);
				Transfers.RemoveSafe(transfer);
			});
		}
	}
}
