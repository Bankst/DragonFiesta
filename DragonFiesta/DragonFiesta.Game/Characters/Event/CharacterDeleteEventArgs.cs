using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
