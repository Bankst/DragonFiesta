using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonFiesta.Messages.Zone.Maps
{
    [Serializable]
    public class StopMap : IMessage
    {
        public Guid Id { get; set; }

        public ushort MapId { get; set; }

        public ushort InstanceId { get; set; }
    }
}
