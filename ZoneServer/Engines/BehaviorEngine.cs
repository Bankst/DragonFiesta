using System.Threading.Tasks;

using DFEngine.Content.Game.Engines;
using DFEngine.Content.GameObjects;

namespace ZoneServer.Engines
{
	internal class BehaviorEngine : IEngine
	{
		public void Main(long now)
		{
			Parallel.ForEach(GameObject.Objects, obj =>
			{
				obj.Behavior?.Update(now);
			});
		}
	}
}
