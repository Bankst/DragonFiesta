using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
