using System.Collections.Generic;

namespace DFEngine.Content.Items
{
	public class ItemDropGroup
	{
		public string IndxName { get; set; }

		public int MinQty { get; set; }

		public int MaxQty { get; set; }

		public List<Item> Items { get; set; }

		public ItemDropGroup()
		{
			Items = new List<Item>();
		}
	}
}
