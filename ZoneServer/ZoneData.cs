using System;
using System.Collections.Generic;
using System.Text;
using DFEngine.IO;
using DFEngine.IO.Definitions.SHN;
using DFEngine.Worlds;

namespace ZoneServer
{
	public class ZoneData
	{
		public static Dictionary<string, Field> Field = new Dictionary<string, Field>();
		public static ObjectCollection<MapInfo> MapInfo = new ObjectCollection<MapInfo>();
	}
}
