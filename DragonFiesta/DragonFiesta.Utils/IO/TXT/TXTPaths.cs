using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonFiesta.Utils.IO.TXT
{
	public static class TXTPaths
	{
		private static readonly Dictionary<TXTType, string> Paths = new Dictionary<TXTType, string>
		{
			{ TXTType.MobAttackSequence, "MobAttackSequence" },
			{ TXTType.MobRegen, "MobRegen" },
			{ TXTType.MobRegenInstant, "MobRegen/Instant" },
			{ TXTType.MobRegenKingdomQuest, "MobRegen/KingdomQuest" },
			{ TXTType.MobRoam, "MobRoam" },
			{ TXTType.MobSetting, "MobSetting/Action" },
			{ TXTType.NPCItemList, "NPCItemList" },
			{ TXTType.Script, "Script" },
			{ TXTType.World, "World" }
		};

		public static string GetPath(TXTType type, string shinePath)
		{
			return $"{shinePath}/{Paths.First(x => x.Key == type).Value}";
		}
	}
}