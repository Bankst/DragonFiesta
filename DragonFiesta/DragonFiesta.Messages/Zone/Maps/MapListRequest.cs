using System;

namespace DragonFiesta.Messages.Zone.Maps
{
    [Serializable]
    public class MapListRequest : ExpectAnswer
    {
        public MapListRequest() : base(10000)
        {
        }
    }
}