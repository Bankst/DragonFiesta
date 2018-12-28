namespace DFEngine.Content.GameObjects.Movers
{
	public class MoverHunger
	{
		public string FeedType { get; set; }

		public ushort RestoreAmount { get; set; }

		public ushort MaxHunger { get; set; }

		public ushort CreateHunger { get; set; }

		public ushort Tick { get; set; }

		public ushort HungerConsumption { get; set; }
	}
}
