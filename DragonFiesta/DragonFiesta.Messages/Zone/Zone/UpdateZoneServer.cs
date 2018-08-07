using System;

namespace DragonFiesta.Messages.Zone.Zone
{
    [Serializable]
    public class UpdateZoneServer : IMessage
    {
        public Guid Id { get; set; }

        public byte ZoneId { get; set; }

        public int CurrentConnection { get; set; }
    }
}