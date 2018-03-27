using DragonFiesta.Utils.Config.Section.Network;
using System;

namespace DragonFiesta.Messages.Zone
{
    [Serializable]
    public class AuthenticatetZone : ExpectAnswer
    {
        public byte ZoneId { get; set; }

        public ServerInfo NetInfo { get; set; }

        public AuthenticatetZone() :
            base((int)ServerTaskTimes.SERVER_AUTH_ZONE)
        {
        }
    }
}