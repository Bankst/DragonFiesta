using DFEngine.Content.Game;

namespace DFEngine.Content.GameObjects
{
	public class GameObject
	{
		public ushort Handle { get; set; }
		public byte Level { get; set; }
		public Position Position { get; set; }
		public Stats Stats { get; set; }
		public GameObjectType Type { get; set; }

		public GameObject()
		{
			Position = new Position();
			Stats = new Stats();
		}
	}
}
