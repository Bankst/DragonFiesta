using DFEngine.Content.GameObjects;
using DFEngine.Content.GameObjects.Movers;
using DFEngine.IO.Definitions.SHN;

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
		public ItemSlot Slot { get; set; }
		/// <summary>
		/// The upgrade tier of the item.
		/// </summary>
		public byte Upgrades { get; set; }

		// ?
		public Mover Mover { get; set; }

		public bool CanEquip => Slot > 0U;

		public Stats Stats { get; set; }
	}
}
