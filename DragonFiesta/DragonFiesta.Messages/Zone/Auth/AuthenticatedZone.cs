using DragonFiesta.Utils.Config.Section.Network;
using System;

namespace DragonFiesta.Messages.Zone
{
    [Serializable]
    public class AuthenticatedZone : ExpectAnswer
    {
        public byte ZoneId { get; set; }

        public ExternServerInfo NetInfo { get; set; }

        public AuthenticatedZone() :
            base((int)ServerTaskTimes.SERVER_AUTH_ZONE)
        {
        }
    }
}