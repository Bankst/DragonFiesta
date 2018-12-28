using System;
using DFEngine.Content.Game;
using DFEngine.Utils;

namespace DFEngine.Threading
{
	public interface IServerTask : IDisposable
	{
		ServerTaskTimes Interval { get; }

		GameTime LastUpdate { get; set; }

		bool Update(GameTime gameTime);
	}
}
