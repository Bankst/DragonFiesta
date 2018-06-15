using DragonFiesta.Messages.Zone.Maps;
using DragonFiesta.Providers.Maps;
using DragonFiesta.World.Game.Maps;
using DragonFiesta.World.Game.Zone;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonFiesta.World.InternNetwork.InternHandler.Server.Maps
{
    public static class MapListMethods
    {
        public static void SendMapStartZoneList(byte Id)
        {
            var MapList = MapDataProvider.FieldInfos.ToList().FindAll(mInfo => mInfo.ZoneID == Id && mInfo.MapInfo.Type == MapType.Normal);

            if (MapList != null)
            {
                SendStartMapList(MapList);
            }
        }

        public static void SendMapLists(InternZoneSession mSession)
        {
            StartMapList Message = new StartMapList()
            {
                Id = Guid.NewGuid(),
                MapsList = new List<StartMap>(),
                
            };

            foreach (var mInstanceMaps in MapManager.InstanceMapsByID.Values)
            {
                Message.MapsList.Add(new StartMap
                {
                   
                    InstanceId = mInstanceMaps.InstanceId,
                    MapId = mInstanceMaps.MapId,
                });
            }
            foreach (var NormalMap in MapManager.NormalMapsByID.Values)
            {
                Message.MapsList.Add(new StartMap
                {
                    MapId = NormalMap.MapId,
                });
            }

            mSession.SendMessage(Message);
        }

        public static void SendStartMapList(List<FieldInfo> MapList)
        {
            StartMapList Message = new StartMapList()
            {
                Id = Guid.NewGuid(),
                MapsList = new List<StartMap>(),
                  
            };

            foreach (var FieldInfo in MapList)
            {
                if (!ZoneManager.GetZoneByID(FieldInfo.ZoneID, out ZoneServer mZone))
                {
                    continue;
                }

                if (!mZone.IsConnected)
                {
                    continue;
                }



                if (FieldInfo.MapInfo.Type != MapType.Normal)
                {
                    if (!MapManager.AvailablInstanceIds.TryDequeue(out ushort InstanceId))
                    {
                        GameLog.Write(GameLogLevel.Warning, "Failed To Start Maplist Instanceids overlow!");
                        continue;
                    }
                    Message.MapsList.Add(new StartMap
                    {
                        InstanceId = InstanceId,
                        MapId = FieldInfo.MapInfo.ID,
                    });
                }
                else
                {
                    Message.MapsList.Add(new StartMap
                    {
                        MapId = FieldInfo.MapInfo.ID,
                    });
                }
            }
            ZoneManager.Broadcast(Message);
        }
    }
}