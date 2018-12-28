namespace DFEngine.Content.Items
{
	public class Socket
	{
		public ushort ID { get; set; }

		public byte Index { get; set; }

		public byte RestCount { get; set; }

		public Item Item { get; set; }

		public Socket(ushort id, byte indx, byte restCount, Item item)
		{
			ID = id;
			Index = indx;
			RestCount = restCount;
			Item = item;
		}
	}
}
