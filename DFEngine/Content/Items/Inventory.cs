using System.Collections.Generic;

namespace DFEngine.Content.Items
{
	/// <summary>
	/// Class that holds a collection of item instances.
	/// </summary>
	public class Inventory
	{
		/// <summary>
		/// The items inside of the inventory.
		/// </summary>
		public List<Item> Items { get; set; }
		/// <summary>
		/// The inventory's type.
		/// </summary>
		public InventoryType Type { get; set; }

		/// <summary>
		/// The maximum number of items this inventory can store.
		/// </summary>
		private int capacity;

		/// <summary>
		/// Creates a new instance of the <see cref="Inventory"/> class.
		/// </summary>
		/// <param name="type">The type of the inventory.</param>
		/// <param name="capacity">The maximum number of items the inventory can contain.</param>
		public Inventory(InventoryType type, int capacity)
		{
			Items = new List<Item>(capacity);
			Type = type;

			this.capacity = capacity;
		}

		/// <summary>
		/// Returns the item at the specified inventory position.
		/// </summary>
		/// <param name="slot">The item's position.</param>
		public Item this[byte slot] => Items.First(i => i.Slot == slot);
	}
}
