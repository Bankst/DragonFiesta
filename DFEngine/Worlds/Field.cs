using System.Collections.Concurrent;
using DFEngine.Content.Game;

namespace DFEngine.Worlds
{
	public class Field
	{
		public string MapIDClient { get; set; }
		public ConcurrentQueue<int> SubHandles { get; set; }
		public string MapName { get; set; }
		public FieldMapType Type { get; set; }
		public short ImmortalSeconds { get; set; }
		public string ScriptName { get; set; }
		public bool CanAttackEnemyGuild { get; set; }
		public bool NeedParty { get; set; }
		public bool IsPKKQ { get; set; }
		public bool IsPVP { get; set; }
		public bool IsPartyBattle { get; set; }
		public bool LinkIN { get; set; }
		public bool LinkOUT { get; set; }
		public bool IsSystemMap { get; set; }
		public bool RegenCity { get; set; }
		public bool CanRestart { get; set; }
		public bool CanTrade { get; set; }
		public bool CanMinihouse { get; set; }
		public bool CanUseItem { get; set; }
		public bool CanUseSkill { get; set; }
		public bool CanChat { get; set; }
		public bool CanShout { get; set; }
		public bool CanBooth { get; set; }
		public bool CanProduce { get; set; }
		public bool CanRide { get; set; }
		public bool CanStone { get; set; }
		public bool CanParty { get; set; }
		public ushort ExpLossByMob { get; set; }
		public ushort ExpLossByPlayer { get; set; }
		public byte ZoneNo { get; set; }

		public bool IsSubLimit { get; set; }
		public int SubFrom { get; set; }
		public int SubTo { get; set; }

		public ConcurrentBag<Vector2> Regens { get; set; }
	}
}
