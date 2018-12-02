using System.Collections.Generic;
using DFEngine.Content.GameObjects;
using DFEngine.IO.Definitions.SHN;

namespace DFEngine.Worlds
{
	public class Map
	{

		public int Handle { get; set; }
		public List<GameObject> Objects { get; set; }
		public MapInfo Info { get; set; }
		public Field Field { get; set; }

		public Map()
		{
			Objects = new List<GameObject>();
		}

		public Map(MapInfo info, Field field)
		{
			Info = info;
			Field = field;
		}
	}
}
