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

		public static bool Initialize(out string message)
		{
			message = "";
			try
			{
				Instance = ReadJson();
				if (Instance != null)
				{
					EngineLog.Write(EngineLogLevel.Startup, "Successfully read Login config.");
					return true;
				}

				if (!Write(out var pConfig))
				{
					message = "Failed to create default LoginConfiguration.";
					return false;
				}
				pConfig.WriteJson();

				EngineLog.Write(EngineLogLevel.Startup, "Successfully created Login config.");
				message = "No LoginConfiguration found! Please edit generated config.";
				return false;
			}
			catch (Exception ex)
			{
				EngineLog.Write(EngineLogLevel.Exception, "Failed to load Login config:\n {0}", ex);
				message = $"Failed to load LoginConfiguration: \n {ex.StackTrace}";
				return false;
			}
		}

		private static bool Write(out LoginConfiguration pConfig)
		{
			pConfig = null;
			try
			{
				pConfig = new LoginConfiguration();
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
