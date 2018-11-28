using System;
using DFEngine.Logging;

namespace DFEngine.Config
{
	public class LoginConfiguration : Configuration<LoginConfiguration>
	{
		public bool CheckVersion { get; protected set; } = false;
		public int MaxPasswordLength { get; protected set; } = 32;
		public string ClientVersion { get; protected set; } = "";
		public int ClientRegion { get; protected set; } = 1; // NA

		public static LoginConfiguration Instance { get; set; }

		public static bool Load(out string message)
		{
			Instance = Initialize(out message);
			return message == string.Empty;
		}
	}
}
