using DragonFiesta.Game.Characters;
using DragonFiesta.Game.Characters.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

