using DragonFiesta.Providers.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
