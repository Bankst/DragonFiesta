using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonFiesta.Messages.Zone.Transfer
{
    [Serializable]
    public class ZoneTransferMessage_Response : IMessage
    {
        public Guid Id { get; set; }

        public ZoneTransferMessage Request { get; set; }

        public ZoneTransferResult AddResult { get; set; }
    }
}
