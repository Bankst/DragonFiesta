using DFEngine;
using DFEngine.Content.Game.Engines;
using ZoneServer.Services;

namespace ZoneServer.Engines
{
	public class AIEngine : IEngine
	{
		public static bool IsAIEnabled = true;

		public void Main(long now)
		{
			for (var upperBound = CharacterService.OnlineCharacters.GetUpperBound(); upperBound >= 0; --upperBound)
				CharacterService.OnlineCharacters[upperBound].AI?.Main(now);
			if (!AIEngine.IsAIEnabled)
				return;
//			for (int upperBound = MobService.ActiveMobs.UpperBound; upperBound >= 0; --upperBound)
//			{
//				MobInstance activeMob = MobService.ActiveMobs[upperBound];
//				if (activeMob.IsAIEnabled)
//				{
//					if (activeMob.Regen == null)
//						activeMob.AI?.Main(now);
//					else if (activeMob.Regen.IsAIEnabled)
//						activeMob.AI?.Main(now);
//				}
//			}
		}
	}
}
