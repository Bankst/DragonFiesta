using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonFiesta.Messages.Zone.Zone
{
    [Serializable]
    public class ServerShutdown : IMessage
    {
        public Guid Id { get; set; }
    }
}
