using DragonFiesta.Game.Zone;
using System;

namespace DragonFiesta.Messages.Zone
{
    [Serializable]
    public class NewZoneStarted : IMessage
    {
        public Guid Id { get; set; }

        public IZone ZoneServer { get; set; }
    }
}