using DragonFiesta.Messages.Zone;
using DragonFiesta.Messages.Zone.Zone;
using DragonFiesta.World.Game.Zone;
using System;

namespace DragonFiesta.World.InternNetwork.InternHandler.Server.Zone
{
    public static class ZoneMethods
    {
        public static void SendZoneStopt(byte StoppedId)
        {
            ZoneStopped StoppedZone = new ZoneStopped
            {
                Id = Guid.NewGuid(),
                ZoneId = StoppedId,
            };

            ZoneManager.Broadcast(StoppedZone, StoppedZone.ZoneId);
        }

        public static void BroadCastServerShutdown()
        {
            ServerShutdown Shutdown = new ServerShutdown
            {
                Id = Guid.NewGuid(),
            };

            ZoneManager.Broadcast(Shutdown);
        }
    }
}
