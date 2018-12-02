using DFEngine.Worlds;

namespace DFEngine.Content.Game
{
	public class Position : Vector2
	{
		public double D { get; set; }
		public Map Map { get; set; }

		public Position() : base(0, 0)
		{
			D = 0;
		}

		public Position(int x, int y, short d) : base(x, y)
		{
			D = d;
		}
	}
}
