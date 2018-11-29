using System.Collections.Generic;
using DFEngine;
using DFEngine.IO.Definitions.SHN;
using DFEngine.Worlds;

namespace ZoneServer.Services
{
	public class MapService
	{
		public static readonly Dictionary<string, List<Map>> ActiveMaps = new Dictionary<string, List<Map>>();

		public static Map Find(string indxName, int handle, byte mode = 1)
		{
			if (!ActiveMaps.TryGetValue(indxName, out var instances))
			{
				ActiveMaps.Add(indxName, new List<Map>());
				instances = ActiveMaps[indxName];
			}

			var map = instances.First(i => i.Handle == handle);
			if (map == null)
			{
				var info = ZoneData.MapInfo[indxName];
				var field = ZoneData.Field[indxName];

				if (info != null && field != null)
				{
					return CreateInstance(info, field, handle, mode);
				}

			}

			return map;
		}

		public static Map CreateInstance(MapInfo info, Field field, int handle, byte mode)
		{
			return null;
		}

		public static void Load(MapInfo info, Field field)
		{
			// If the map is created using handles, don't load it initially
			if (field.IsSubLimit)
			{
				return;
			}

			var map = new Map()
			{
				Info = info,
				Field = field
			};



			// Load mobs, npcs, gates, etc...
		}
	}
}
