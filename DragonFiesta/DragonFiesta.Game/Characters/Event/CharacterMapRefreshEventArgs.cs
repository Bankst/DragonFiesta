namespace DragonFiesta.Game.Characters.Event
{
    public class CharacterMapRefreshEventArgs<TCharacter> : CharacterEventArgs<TCharacter>
        where TCharacter : CharacterBase
    {
        public CharacterMapRefreshEventArgs(TCharacter Character) : base(Character)
        {

        }
    }
}
