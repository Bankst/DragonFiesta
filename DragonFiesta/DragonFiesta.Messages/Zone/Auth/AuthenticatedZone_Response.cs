using DragonFiesta.Game.Zone;
using System;
using System.Collections.Generic;

namespace DragonFiesta.Messages.Zone
{
    [Serializable]
    public class AuthenticatedZone_Response : IMessage
    {
        public Guid Id { get; set; }

        public ClientRegion Region { get; set; }

        public List<IZone> RemoteZoneList { get; set; }

        public InternZoneAuthesult Result { get; set; }
    }
}