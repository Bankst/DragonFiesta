using System;
using System.Security;
using DFEngine.Logging;

namespace DFEngine.Config
{
	public class DatabaseConfiguration : Configuration<DatabaseConfiguration>
	{
		public string Host { get; protected set; } = "127.0.0.1";
		public string Username { get; protected set; } = "sa";
		public string Password { get; protected set; } = "password";

		public string AccountDatabase { get; protected set; } = "Account";
		public string AccountLogDatabase { get; protected set; } = "AccountLog";
		public string CharacterDatabase { get; protected set; } = "Character";
		public string GameLogDatabase { get; protected set; } = "GameLog";

		public int ClientLifeTime { get; protected set; } = 10;
		public int MinPoolSize { get; protected set; } = 5;
		public int MaxPoolSize { get; protected set; } = 10;

		public static DatabaseConfiguration Instance { get; set; }

		public static bool Initialize(out string message)
		{
			message = "";
			try
			{
				Instance = ReadJson();
				if (Instance != null)
				{
					EngineLog.Write(EngineLogLevel.Startup, "Successfully read Database config.");
					return true;
				}

				if (!Write(out var pConfig))
				{
					message = "Failed to create default DatabaseConfiguration.";
					return false;
				}
				pConfig.WriteJson();

				EngineLog.Write(EngineLogLevel.Startup, "Successfully created Database config.");
				message = "No DatabaseConfiguration found! Please edit generated config.";
				return false;
			}
			catch (Exception ex)
			{
				EngineLog.Write(EngineLogLevel.Exception, "Failed to load Database config:\n {0}", ex);
				message = $"Failed to load DatabaseConfiguration: \n {ex.StackTrace}";
				return false;
			}
		}

		private static bool Write(out DatabaseConfiguration pConfig)
		{
			pConfig = null;
			try
			{
				pConfig = new DatabaseConfiguration();
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
