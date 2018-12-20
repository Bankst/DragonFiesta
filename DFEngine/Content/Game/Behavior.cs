using System.Collections.Generic;
using DFEngine.Content.Game;

namespace DFEngine.Content.GameObjects
{
	public abstract class Behavior
	{
		public GameObject Object { get; set; }

		public void OnInit(GameObject obj)
		{
			Object = obj;
		}

		public virtual void Update(long now)
		{
		}

		public virtual void MoveTo(Vector2 to, double space = 0, bool force = false)
		{
		}

		public virtual void MoveTo(Vector2 from, Vector2 to, double space = 0, bool force = false)
		{
		}

		public virtual void Stop(Vector2 at, bool force = false)
		{
		}

	}
}
