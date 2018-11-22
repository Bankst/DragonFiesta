using System.Security;

namespace DFEngine.Config
{
	public class DatabaseConfiguration
	{
		public string Host { get; protected set; }
		public string Username { get; protected set; }
		public SecureString Password { get; protected set; }
		public string Database { get; protected set; }
		public int ClientLifeTime { get; protected set; }
		public int MinPoolSize { get; protected set; }
		public int MaxPoolSize { get; protected set; }

		public static DatabaseConfiguration Instance { get; set; }
	}
}
