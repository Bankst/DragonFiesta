namespace DFEngine.Content.Items
{
	/// <summary>
	/// Class that contains data for a character's equipment.
	/// </summary>
	public class Equipment : Inventory
	{
		/// <summary>
		/// The base maximum number of items this inventory can contain.
		/// </summary>
		public const int BaseCapacity = 29;

		/// <summary>
		/// Creates a new instance of the <see cref="Equipment"/> class.
		/// </summary>
		public Equipment() : base(InventoryType.IT_EQUIPPED, BaseCapacity)
		{
		}

		/// <summary>
		/// Returns the equipped item at the specified inventory position.
		/// </summary>
		/// <param name="equip">The item's equip type.</param>S
		public Item this[ItemEquip equip] => Items.First(i => i.Info.Equip == equip);
	}
}
