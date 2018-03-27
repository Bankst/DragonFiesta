using DragonFiesta.Messages.Zone.Maps;
using DragonFiesta.Providers.Maps;
using DragonFiesta.World.Game.Maps;
using DragonFiesta.World.Game.Zone;
using System;

namespace DragonFiesta.World.InternNetwork.InternHandler.Server.Maps
{
    public static class MapMethods
    {

        public static void SendStartNewMap(MapInfo Info, out ushort InstanceId)
        {
            InstanceId = 0;

            if (Info.Type != MapType.Normal)
            {
                if (!MapManager.AvailablInstanceIds.TryDequeue(out InstanceId))
                {
                    InstanceId = 0;
                }
            }

            StartMap mStart = new StartMap
            {
                Id = Guid.NewGuid(),
                InstanceId = InstanceId,
                MapId = Info.ID,
            };

            ZoneManager.Broadcast(mStart);
        }
    
        public static void SendStopMap(ushort MapId, ushort InstanceId = 0,ZoneServer Server = null)
        {
            StopMap mStop = new StopMap
            {
                Id = Guid.NewGuid(),
                InstanceId = InstanceId,
                MapId = MapId,
            };
            if (Server == null)
                ZoneManager.Broadcast(mStop);
            else
                Server.Send(mStop);
        }
    }
}