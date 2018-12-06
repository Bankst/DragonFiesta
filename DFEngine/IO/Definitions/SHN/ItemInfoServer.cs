namespace DFEngine.IO.Definitions.SHN
{
	[Definition]
	public class ItemInfoServer
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
		public string ItemSort_Index { get; private set; }
	}
}
