#region

using DragonFiesta.Providers.Characters;

#endregion

namespace DragonFiesta.Game.Characters.Event
{
    public class CharacterClassChangeEventArgs<TCharacter> : CharacterEventArgs<TCharacter>
         where TCharacter : CharacterBase
    {
        public ClassId NewClass { get; private set; }
        public CharacterClassChangeEventArgs(TCharacter Character, ClassId NewClass)
            : base(Character)
        {
            this.NewClass = NewClass;
        }
    }
}
