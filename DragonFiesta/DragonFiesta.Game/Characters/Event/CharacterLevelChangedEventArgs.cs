using DragonFiesta.Game.Characters;
using DragonFiesta.Game.Characters.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CharacterLevelChangedEventArgs<TCharacter> :
    CharacterEventArgs<TCharacter> where TCharacter : CharacterBase
{
    public CharacterLevelChangedEventArgs(TCharacter Character) : base(Character)
    {
    }

    public byte NewLevel { get; set; }
    public CharacterLevelChangedEventArgs(TCharacter Character, byte NewLevel)
        : base(Character)
    {
        this.NewLevel = NewLevel;
    }
}