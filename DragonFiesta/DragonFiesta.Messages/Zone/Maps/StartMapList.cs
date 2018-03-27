using System;
using System.Collections.Generic;

namespace DragonFiesta.Messages.Zone.Maps
{
    [Serializable]
    public class StartMapList : IMessage
    {
        public Guid Id { get; set; }

        public List<StartMap> MapsList;
    }
}