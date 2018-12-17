using System;
using System.Collections.Generic;
using System.Text;
using DFEngine.Content.GameObjects;
using ZoneServer.Other;

namespace ZoneServer.Services
{
	internal static class AIService
	{
		internal static IAI CreateAI(GameObject Object)
		{
			if (Object.Type == GameObjectType.CHARACTER)
				return new CharacterAI();
			if (Object.Type != GameObjectType.MOB)
				return null;
			// TODO: MOBAI
			return (IAI)new MobAI();
		}

		internal static void InitAI(GameObject Object)
		{
			if (Object.AI != null)
				return;
			Object.AI = CreateAI(Object);
			Object.AI.Init(Object);
		}
	}
}
