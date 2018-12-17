using DFEngine.Content.Game;
using DFEngine.Content.GameObjects;
using DFEngine.Content.Other;
using DFEngine.Logging;

namespace ZoneServer.Logic
{
	internal static class MoveLogic
	{
		internal static void Move(Character character, Vector2 from, Vector2 to, MoveState state)
		{
			//TODO: reset character emoticon
			if (character.IsDead)
			{
				GameLog.Write(GameLogLevel.Warning, $"A dead object attempted to move - handle: {character.Handle}");
			}
			else
			{
				if (character.IsTrading && Vector2.Distance(from, to) > 75.0)
				{
					//TODO: Cancel trade
				}

			}
		}
	}
}
