using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonFiesta.Messages.World.Character
{
    [Serializable]
    public class CharacterDeleted : IMessage
    {
        public Guid Id { get; set; }

        public int CharacterId { get; set; }
    }
}
