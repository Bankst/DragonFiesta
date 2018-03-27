using DragonFiesta.Game.Characters;
using DragonFiesta.Game.Characters.Event;

public class CharacterNameChangedEventArgs<pCharacterType> : CharacterNameChangedEventArgs
      where pCharacterType : CharacterBase
{
    public new pCharacterType Character { get; private set; }

    public CharacterNameChangedEventArgs(pCharacterType Character, string OldName)
        : base(Character, OldName)
    {
        this.Character = Character;
    }

    ~CharacterNameChangedEventArgs()
    {
        Character = default(pCharacterType);
    }
}

public class CharacterNameChangedEventArgs : CharacterEventArgs
{
    public string OldName { get; private set; }

    public CharacterNameChangedEventArgs(CharacterBase Character, string OldName)
        : base(Character)
    {
        this.OldName = OldName;
    }

    ~CharacterNameChangedEventArgs()
    {
        OldName = null;
    }
}