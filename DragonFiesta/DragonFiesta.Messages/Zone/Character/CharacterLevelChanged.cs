using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonFiesta.Messages.Zone.Character
{
    [Serializable]
    public class CharacterLevelChanged : IMessage
    {
        public Guid Id { get; set; }

        public int CharacterId { get; set; }

        public byte NewLevel { get; set; }

        public ushort MobId { get; set; }
    }
}
