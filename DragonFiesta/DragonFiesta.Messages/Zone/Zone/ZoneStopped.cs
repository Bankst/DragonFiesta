using System;

namespace DragonFiesta.Messages.Zone
{
    //World -> To All Rest Zone for remote zone dc
    [Serializable]
    public class ZoneStopped : IMessage
    {
        public Guid Id { get; set; }

        public byte ZoneId { get; set; }
    }
}