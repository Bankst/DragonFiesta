using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonFiesta.Messages.Zone.Maps
{
    [Serializable]
    public class StartMap_Response : IMessage
    {
        public MapStartResult Result { get; set; }

        public ushort MapId { get; set; }

        public ushort InstanceId { get; set; }

        public Guid Id { get; set; }
    }
}
