using DragonFiesta.Messages.Zone.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonFiesta.Messages.Zone.Character
{
    [Serializable]
    public class CharacterChangeMap : IMessage
    {
        public int CharacterId { get; set; }

        public ushort MapId { get; set; }

        public ushort InstanceId { get; set; }

        public Position Position { get; set; }

        public Guid Id { get; set; }
    }
}
