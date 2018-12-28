using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DFEngine.Logging;

namespace DFEngine.Config
{
	public class ZoneConfiguration : Configuration<ZoneConfiguration>
	{
		public SingleZoneConfiguration[] Zones { get; protected set; } =
		{
			new SingleZoneConfiguration(),
		};
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

	public class SingleZoneConfiguration
	{
		public byte ZoneID { get; protected set; } = 0;
		public string ShinePath { get; protected set; } = "ShineData";
		public bool CheckSHNHash { get; protected set; } = false;
	}
}
