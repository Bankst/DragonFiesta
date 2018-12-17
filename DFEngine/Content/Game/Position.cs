using System.Collections.Generic;

using DFEngine.Content.GameObjects;
using DFEngine.Worlds;

namespace DFEngine.Content.Game
{
	public class Position : Vector2
	{
		public double D { get; set; }
		public byte DByte => D.ToDirectionByte();

		public Map Map { get; set; }

		public Position() : base(0, 0)
		{
			D = 0;
		}

		public Position(int x, int y, short d) : base(x, y)
		{
			D = d;
		}

		public Position(Map map, double x, double y, double d) : base(x, y)
		{
			D = d;
			Map = map;
		}

		public List<GameObject> GetSurroundingObjects(int radius)
		{
			return new List<GameObject>(Map.Objects).Filter(obj => obj.Position != this && obj.Position.IsInCircle(X, Y, radius));
		}
	}
}
