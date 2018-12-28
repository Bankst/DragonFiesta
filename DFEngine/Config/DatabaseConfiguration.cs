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

		public static bool Load(out string message)
		{
			Instance = Initialize(out message);
			return message == string.Empty;
		}
	}
}
