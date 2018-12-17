using DFEngine.Content.Items;

namespace DFEngine.Content.GameObjects.MiniHouse
{
	public class MiniHouse
	{
		public ushort Handle { get; set; }

		public Item Item { get; set; }

		public long KeepTimeHour { get; set; }

		public long HPTick { get; set; }

		public long SPTick { get; set; }

		public int SlotCount { get; set; }

		public int HPRecoveryAmount { get; set; }

		public int SPRecoveryAmount { get; set; }

		public long CastTime { get; set; }
	}
}
