namespace DragonFiesta.Game.Characters.Event
{
    public class CharacterDeleteEventArgs<TCharacter> : CharacterEventArgs<TCharacter>
         where TCharacter : CharacterBase
    {
        public CharacterDeleteEventArgs(TCharacter character)
            : base(character)
        {

        }
    }
}
