using DFEngine.Definitions.SHN;

namespace DFEngine.Content.Items
{
	/// <summary>
	/// Class that contains information for an instance of an item.
	/// </summary>
	public class Item
	{
		/// <summary>
		/// The item's information from ItemInfo.shn.
		/// </summary>
		public ItemInfo Info { get; set; }
		/// <summary>
		/// The item's key.
		/// </summary>
		public long Key { get; set; }
		/// <summary>
		/// The item's position in the inventory.
		/// </summary>
		public byte Slot { get; set; }
		/// <summary>
		/// The upgrade tier of the item.
		/// </summary>
		public byte Upgrades { get; set; }
	}
}
