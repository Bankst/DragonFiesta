using DragonFiesta.Game.Characters;
using DragonFiesta.Game.Characters.Event;

public class CharacterMapEventArgs<TCharacter, TMap> : CharacterEventArgs<TCharacter>
    where TCharacter : CharacterBase
    where TMap : IMap
{

    public TMap Map { get; private set; }

    public CharacterMapEventArgs(TCharacter Character, TMap Map) : base(Character)
    {
        this.Map = Map;
    }
}

