using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonFiesta.Messages.Zone.Character
{
    [Serializable]
   public class SetCharacterMoney : IMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public int CharacterId { get; set; }

        public ulong NewMoney { get; set; }
    }
}
