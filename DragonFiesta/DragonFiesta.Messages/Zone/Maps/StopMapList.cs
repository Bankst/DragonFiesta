using System;
using System.Collections.Generic;

namespace DragonFiesta.Messages.Zone.Maps
{
    [Serializable]
    public class StopMapList : IMessage
    {
        public List<StopMap> MapsList;

        public Guid Id { get; set; } = Guid.NewGuid();
    }
}