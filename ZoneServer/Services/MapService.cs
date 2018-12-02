using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DFEngine;
using DFEngine.IO.Definitions.SHN;
using DFEngine.Logging;
using DFEngine.Worlds;

namespace ZoneServer.Services
{
	public class MapService
	{
		public static readonly Dictionary<string, List<Map>> Instances = new Dictionary<string, List<Map>>();

		public static bool TryInstantiate(MapInfo info, Field field, out Map map)
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();

			var instances = Instances.GetSafe(info.MapName);
			if (instances == null)
			{
				Instances.AddSafe(info.MapName, new List<Map>());
				instances = Instances.GetSafe(info.MapName);
			}

			map = null;
			var handle = 0;

			if (field.IsSubLimit && !field.SubHandles.TryDequeue(out handle))
			{
				stopwatch.Stop();
				EngineLog.Write(EngineLogLevel.Exception, $"Map instance overflow: no more handles available for another instance of {info.MapName}.");
				return false;
			}

			map = new Map(info, field) {Handle = handle};

			// load npc, mob, etc...

			instances.Add(map);

			stopwatch.Stop();
			EngineLog.Write(EngineLogLevel.Info, $"Started a map instance: {info.MapName} in {stopwatch.ElapsedMilliseconds}ms");
			return true;
		}

		public static bool TryFindInstance(string indxName, out Map map, int handle = 0)
		{
			map = null;
			var field = ZoneData.Field.GetSafe(indxName);
			if (field == null)
			{
				EngineLog.Write(EngineLogLevel.Exception, $"Could not find {indxName} because the Field is null.");
				return false;
			}

			var instances = Instances.GetSafe(indxName);
			if (instances == null)
			{
				EngineLog.Write(EngineLogLevel.Exception, "Tried to find an instance for a map that was never initialized.");
				return false;
			}

			if (field.IsSubLimit)
			{
				map = instances.First(instance => instance.Handle == handle);
				return true;
			}

			map = instances.First(instance => instance.Handle == 0);
			return true;
		}
	}
}
