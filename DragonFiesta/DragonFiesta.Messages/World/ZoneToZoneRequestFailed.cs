using System;

namespace DragonFiesta.Messages.World
{
    public class ZoneToZoneRequestFailed : IMessage
    {
        public Guid Id { get; set; }

        public Guid RequestId { get; set; }
    }
}