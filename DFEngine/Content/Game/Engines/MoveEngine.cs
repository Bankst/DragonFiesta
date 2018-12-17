using System;
using System.Collections.Generic;
using System.Text;
using DFEngine.Content.Game.Controllers;

namespace DFEngine.Content.Game.Engines
{
	public class MoveEngine : IEngine
	{
		public static volatile List<MoveController> MoveControllers = new List<MoveController>();
		private static long _lastMoveCall;

		public void Main(long now)
		{
			for (var upperBound = MoveControllers.GetUpperBound(); upperBound >= 0; --upperBound)
			{
				MoveControllers[upperBound].Main((now - _lastMoveCall) / 1000.0);
			}
			_lastMoveCall = now;
		}
	}
}
