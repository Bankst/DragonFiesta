namespace DFEngine.Content.GameObjects
{
	/// <summary>
	/// Class that represents a character's appearance.
	/// </summary>
	public class CharacterShape
	{
		/// <summary>
		/// The character's class type.
		/// </summary>
		public CharacterClass Class { get; set; }
		/// <summary>
		/// The character's base class type.
		/// </summary>
		public CharacterClass BaseClass => GetBaseClass(Class);
		/// <summary>
		/// The character's face shape.
		/// </summary>
		public byte Face { get; set; }
		/// <summary>
		/// The character's gender.
		/// </summary>
		public Gender Gender { get; set; }
		/// <summary>
		/// The character's hair shape.
		/// </summary>
		public byte Hair { get; set; }
		/// <summary>
		/// The character's hair color.
		/// </summary>
		public byte HairColor { get; set; }
		/// <summary>
		/// The character's race.
		/// </summary>
		public byte Race { get; set; }

		/// <summary>
		/// Retrieves the class's base class (or the first 'step' of the class).
		/// </summary>
		/// <param name="characterClass">The current class.</param>
		/// <returns>The class's base class.</returns>
		public static CharacterClass GetBaseClass(CharacterClass characterClass)
		{
			switch (characterClass)
			{
				case CharacterClass.CC_FIGHTER:
				case CharacterClass.CC_CLEVERFIGHTER:
				case CharacterClass.CC_WARRIOR:
				case CharacterClass.CC_GLADIATOR:
				case CharacterClass.CC_KNIGHT:
					return CharacterClass.CC_FIGHTER;
				case CharacterClass.CC_CLERIC:
				case CharacterClass.CC_HIGHCLERIC:
				case CharacterClass.CC_PALADIN:
				case CharacterClass.CC_HOLYKNIGHT:
				case CharacterClass.CC_GUARDIAN:
					return CharacterClass.CC_CLERIC;
				case CharacterClass.CC_ARCHER:
				case CharacterClass.CC_HAWKARCHER:
				case CharacterClass.CC_SCOUT:
				case CharacterClass.CC_SHARPSHOOTER:
				case CharacterClass.CC_RANGER:
					return CharacterClass.CC_ARCHER;
				case CharacterClass.CC_MAGE:
				case CharacterClass.CC_WIZMAGE:
				case CharacterClass.CC_ENCHANTER:
				case CharacterClass.CC_WARLOCK:
				case CharacterClass.CC_WIZARD:
					return CharacterClass.CC_MAGE;
				case CharacterClass.CC_TRICKSTER:
				case CharacterClass.CC_GAMBIT:
				case CharacterClass.CC_RENEGADE:
				case CharacterClass.CC_SPECTRE:
				case CharacterClass.CC_REAPER:
					return CharacterClass.CC_TRICKSTER;
				case CharacterClass.CC_CRUSADER:
				case CharacterClass.CC_TEMPLAR:
					return CharacterClass.CC_CRUSADER;
				default:
					return CharacterClass.CC_NONE;
			}
		}
	}
}