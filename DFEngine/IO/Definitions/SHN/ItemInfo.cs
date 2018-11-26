using DFEngine.Content.Items;
using DFEngine.IO;

namespace DFEngine.Definitions.SHN
{
	[Definition]
	public class ItemInfo
	{
		/// <summary>
		/// The item's ID.
		/// </summary>
		[Identity]
		public ushort ID { get; private set; }
		/// <summary>
		/// The item's index name.
		/// </summary>
		public string InxName { get; private set; }
		/// <summary>
		/// The item's full name.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// The item's equip type.
		/// </summary>
		public ItemEquip Equip { get; private set; }
	}
}
