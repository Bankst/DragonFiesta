using System;
using System.Collections.Generic;
using System.Text;
using DFEngine.Content.Other;

namespace DFEngine.Content.GameObjects
{
	/// <summary>
	/// Class that represents a character's default data.
	/// </summary>
	public class DefaultCharacterData
	{
		/// <summary>
		/// The class that this data is for.
		/// </summary>
		public CharacterClass Class { get; set; }
		/// <summary>
		/// The map the character starts in.
		/// </summary>
		public string MapIndx { get; set; }
		/// <summary>
		/// The X-coordinate of the character.
		/// </summary>
		public int PosX { get; set; }
		/// <summary>
		/// The Y-coordinate of the character.
		/// </summary>
		public int PosY { get; set; }
		/// <summary>
		/// The character's starting HP.
		/// </summary>
		public int HP { get; set; }
		/// <summary>
		/// The character's starting SP.
		/// </summary>
		public int SP { get; set; }
		/// <summary>
		/// The character's starting HP stone count.
		/// </summary>
		public int HPStone { get; set; }
		/// <summary>
		/// The character's starting SP stone count.
		/// </summary>
		public int SPStone { get; set; }
		/// <summary>
		/// The character's starting Cen amount.
		/// </summary>
		public long Cen { get; set; }
		/// <summary>
		/// The character's starting level.
		/// </summary>
		public byte Level { get; set; }
		/// <summary>
		/// The character's starting EXP.
		/// </summary>
		public long EXP { get; set; }
		/// <summary>
		/// The character's starting items.
		/// </summary>
		public List<Tuple<ushort, byte>> Items { get; set; }
		/// <summary>
		/// The character's starting skills.
		/// </summary>
		public List<ushort> Skills { get; set; }
		/// <summary>
		/// The character's starting quests.
		/// </summary>
		public List<int> Quests { get; set; }
		/// <summary>
		/// The character's starting shortcuts.
		/// </summary>
		public List<Shortcut> Shortcuts { get; set; }

		/// <summary>
		/// Creates a new instance of the <see cref="DefaultCharacterData"/> class.
		/// </summary>
		public DefaultCharacterData()
		{
			Items = new List<Tuple<ushort, byte>>();
			Skills = new List<ushort>();
			Quests = new List<int>();
			Shortcuts = new List<Shortcut>();
		}
	}
}
