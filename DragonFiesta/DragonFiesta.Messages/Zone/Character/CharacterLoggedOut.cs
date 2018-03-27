using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonFiesta.Messages.Zone.Character
{
    [Serializable]
    public class CharacterLoggedOut : IMessage
    {
        public bool SendCharacterList { get; set; }
        public int CharacterId { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
