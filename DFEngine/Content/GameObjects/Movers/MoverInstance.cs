namespace DFEngine.Content.GameObjects.Movers
{
	public class MoverInstance : GameObject
	{
		public Mover Mover { get; set; }

		public int CurrentHP { get; set; }

		public byte Grade { get; set; }

		public byte Slot { get; set; }

		public byte InventorySlot { get; set; }

		public long LastTick { get; set; }

		public Character Owner { get; set; }

		public Character Passenger { get; set; }

		public MoverInstance(Mover mover)
		{
			CurrentHP = mover.Hunger.CreateHunger;
			Mover = mover;
			Type = GameObjectType.MOVER;
		}
	}
}
