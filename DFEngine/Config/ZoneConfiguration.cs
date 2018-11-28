using System;
using System.Collections.Generic;
using System.Text;
using DFEngine.Logging;

namespace DFEngine.Config
{
	public class ZoneConfiguration : Configuration<ZoneConfiguration>
	{
		public string ShinePath { get; protected set; } = "ShineData";
		// TODO:
		// What else does the zone need?
		// Maybe some things from WorldConfiguration should get moved?


		public static ZoneConfiguration Instance { get; set; }

		public static bool Load(out string message)
		{
			Instance = Initialize(out message);
			return message == string.Empty;
		}
	}
}
