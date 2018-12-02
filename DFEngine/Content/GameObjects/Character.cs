using DFEngine.Content.Game;

namespace DFEngine.Content.GameObjects
{
	/// <summary>
	/// Class that contains character data.
	/// </summary>
	public class Character : GameObject
	{
		public int CharNo { get; set; }
		public int UserNo { get; set; }
		public string Name { get; set; }
		public byte Slot { get; set; }
		public byte AdminLevel { get; set; }
		public ushort PrisonMinutes { get; set; }

		public long EXP { get; set; }
		public long Cen { get; set; }
		public int Fame { get; set; }

		public byte StatPoints { get; set; }
		public byte SkillPoints { get; set; }
		public int KillPoints { get; set; }

		public CharacterShape Shape { get; set; }

		public Character()
		{
			Type = GameObjectType.CHARACTER;
			Shape = new CharacterShape();
		}
	}
}
