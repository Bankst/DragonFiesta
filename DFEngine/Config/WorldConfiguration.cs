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

		public static bool Load(out string message)
		{
			Instance = Initialize(out message);
			return message == string.Empty;
		}
	}
}
