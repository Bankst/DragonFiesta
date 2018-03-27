using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonFiesta.Messages.Zone.Character
{
    [Serializable]
    public class CharacterLoggedIn : IMessage
    {
        public Guid Id { get; set; }

        public int CharacterId { get; set; }

        public ushort MapId { get; set; }

        public ushort Instance { get; set; }
    }
}
