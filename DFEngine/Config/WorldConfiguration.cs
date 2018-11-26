using System;
using System.Collections.Generic;
using System.Text;
using DFEngine.Logging;

namespace DFEngine.Config
{
	public class WorldConfiguration : Configuration<WorldConfiguration>
	{
		public string ShinePath { get; protected set; } = "ShineData";
		public int TutorialMap { get; protected set; } = 150;
		public bool UseTutorial { get; protected set; } = false;
		public int MaxCharacterPerWorld { get; protected set; } = 6;
		public bool ShowWeaponsAtCharacterSelect { get; protected set; } = false;
		public int DefaultSpawnMapId { get; protected set; } = 124;
		public int RestExp_Rate { get; protected set; } = 1100;
		public int RestExp_MinNeedTime { get; protected set; } = 21600;
		public int RestExp_BonusTime { get; protected set; } = 600;
		public int RestExp_IntervalTime { get; protected set; } = 3600;
		public int RunSpeed { get; protected set; } = 125;
		public int WalkSpeed { get; protected set; } = 33;
		public int LevelLimit { get; protected set; } = 130;

		public static WorldConfiguration Instance { get; set; }

		public static bool Initialize(out string message)
		{
			message = "";
			try
			{
				Instance = ReadJson();
				if (Instance != null)
				{
					EngineLog.Write(EngineLogLevel.Startup, "Successfully read World config.");
					return true;
				}

				if (!Write(out var pConfig))
				{
					message = "Failed to create default WorldConfiguration.";
					return false;
				}
				pConfig.WriteJson();

				EngineLog.Write(EngineLogLevel.Startup, "Successfully created World config.");
				message = "No WorldConfiguration found! Please edit generated config.";
				return false;
			}
			catch (Exception ex)
			{
				EngineLog.Write(EngineLogLevel.Exception, "Failed to load World config:\n {0}", ex);
				message = $"Failed to load WorldConfiguration:\n {ex.StackTrace}";
				return false;
			}
		}

		private static bool Write(out WorldConfiguration pConfig)
		{
			pConfig = null;
			try
			{
				pConfig = new WorldConfiguration();
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
