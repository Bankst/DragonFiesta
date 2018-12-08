using System.Collections.Generic;

namespace DFEngine.Content.GameObjects
{
	public class CharacterCommon
	{
		public int RunSpeed { get; set; }
		public int WalkSpeed { get; set; }
		public int AttackSpeed { get; set; }
		public int ShoutLevel { get; set; }
		public int ShoutDelay { get; set; }
		public int LevelLimit { get; set; }
		public int MaxExpBonus { get; set; }
		public int RestExpRate { get; set; }
		public int MinNeedTime { get; set; }
		public int DefaultBonusTime { get; set; }
		public int IntervalTime { get; set; }
		public int AddBuffTime { get; set; }
		public int MaxBuffTime { get; set; }
		public int LostExpLevel { get; set; }
		public int MaxFreeStat { get; set; }
		public Dictionary<int, long> NextEXP { get; set; }
		public Dictionary<int, long> EXPLostAtPVP { get; set; }

		public CharacterCommon()
		{
			NextEXP = new Dictionary<int, long>();
			EXPLostAtPVP = new Dictionary<int, long>();
		}

		public long GetNextEXP(int level)
		{
			return NextEXP.ContainsKey(level) ? NextEXP[level] : 0;
		}

		public long GetEXPLostAtPVP(int level)
		{
			return EXPLostAtPVP.ContainsKey(level)? EXPLostAtPVP[level] : 0;
		}
	}
}
