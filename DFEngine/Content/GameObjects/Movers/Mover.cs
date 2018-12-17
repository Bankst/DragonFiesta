using DFEngine.Content.Items;

namespace DFEngine.Content.GameObjects.Movers
{
	public class Mover
	{
		public uint ID { get; set; }

		public Item Item { get; set; }

		public string IndxName { get; set; }

		public int CastTime { get; set; }

		public int CoolTime { get; set; }

		public int RunSpeed { get; set; }

		public int WalkSpeed { get; set; }

		public ushort Duration { get; set; }

		public byte MaxCharSlot { get; set; }

		public MoverHunger Hunger { get; set; }
	}
}
