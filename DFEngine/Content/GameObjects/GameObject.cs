using DFEngine.Content.Game;

namespace DFEngine.Content.GameObjects
{
	public abstract class GameObject
	{
		public ushort Handle { get; set; }

		public byte Level { get; set; }
		public GameObjectType Type { get; set; }
		public Stats Stats { get; set; }
		public Position Position { get; set; }

		protected GameObject()
		{
			Stats = new Stats(this);
			Position = new Position();
		}
	}
}
