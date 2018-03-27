using System;
using System.Collections.Generic;

namespace DragonFiesta.Messages.Zone.Maps
{
    [Serializable]
    public class StartMapList_Response : IMessage
    {
        public Guid Id { get; set; }

        public List<StartMap_Response> MapList;
    }
}