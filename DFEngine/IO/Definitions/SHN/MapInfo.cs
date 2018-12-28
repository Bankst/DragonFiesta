using DFEngine.Worlds;

namespace DFEngine.IO.Definitions.SHN
{
	[Definition]
	public class MapInfo
	{
		[Identity]
		public ushort ID { get; set; }
		public string MapName { get; set; }
		public string Name { get; set; }
		public int IsWMLink { get; set; }
		public bool KingdomMap { get; set; }
		public uint RegenX { get; set; }
		public uint RegenY { get; set; }
		public string MapFolderName { get; set; }
		public bool InSide { get; set; }
		public uint Sight { get; set; }
	}
}
