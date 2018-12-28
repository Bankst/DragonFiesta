using DFEngine.Content.Items;

namespace DFEngine.IO.Definitions.SHN
{
	[Definition]
	public class ItemInfoServer
	{
		[Identity]
		public uint ID { get; set; }
		public string InxName { get; set; }
		public string MarketIndex { get; set; }
		public bool City { get; set; }
		public string DropGroupA { get; set; }
		public string DropGroupB { get; set; }
		public string RandomOptionDropGroup { get; set; }
		public uint Vanish { get; set; }
		public uint looting { get; set; }
		public ISEType ISET_Index { get; set; }
		public string ItemSort_Index { get; set; }
		public bool KQItem { get; set; }
	}
}
