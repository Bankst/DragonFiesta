#region

using System;

#endregion

namespace DragonFiesta.Game.Characters.Event
{
    public class CharacterEventArgs<pCharacterType> : CharacterEventArgs
           where pCharacterType : CharacterBase
    {
        public new pCharacterType Character { get; private set; }

        public CharacterEventArgs(pCharacterType Character)
            : base(Character)
        {
            this.Character = Character;
        }

        ~CharacterEventArgs()
        {
            Character = default(pCharacterType);
        }
    }

    public class CharacterEventArgs : EventArgs
    {
        public CharacterBase Character { get; private set; }

        public CharacterEventArgs(CharacterBase Character)
        {
            this.Character = Character;
        }

        ~CharacterEventArgs()
        {
            Character = null;
        }
    }
}