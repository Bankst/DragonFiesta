using DragonFiesta.Game.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonFiesta.Messages.Zone.Transfer
{
    [Serializable]
    public class ZoneTransferMessage : ExpectAnswer
    {
        public int CharacterId { get; set; }

        public ushort MapId { get; set; }

        public ushort InstanceId { get; set; }

        public Position SpawnPosition { get; set; }
        public ushort WorldSessionId { get; set; }

        public byte RoleId { get; set; }

        public ZoneTransferMessage()
            : base(5000)
        {
        }
    }
}
