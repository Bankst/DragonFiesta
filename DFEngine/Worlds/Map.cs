using System.Collections.Generic;
using System.Runtime.Serialization;
using DFEngine.IO.Definitions.SHN;

namespace DFEngine.Worlds
{
	public class Map
	{
		public int Handle { get; set; }
		//public List<GameObjects> Objects { get; set; }
		public MapInfo Info { get; set; }
		public Field Field { get; set; }

		public Map()
		{
//			Objects = new List<GameObject>();
		}
	}
}
