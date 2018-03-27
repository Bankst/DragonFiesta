using DragonFiesta.Messages.Zone.Maps;
using DragonFiesta.Providers.Maps;
using DragonFiesta.World.Game.Zone;
using DragonFiesta.World.InternNetwork.InternHandler.Server.Maps;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace DragonFiesta.World.Game.Maps
{
    [ServerModule(ServerType.World, InitializationStage.Logic)]
    public class MapManager
    {
        #region Property
        public static ConcurrentQueue<ushort> AvailablInstanceIds { get; private set; }

        public static ConcurrentDictionary<ushort, NormalMap> NormalMapsByID { get; private set; }
        public static ConcurrentDictionary<ushort, InstanceMap> InstanceMapsByID { get; private set; }

        [InitializerMethod]
        public static bool InitialMapManager()
        {
            NormalMapsByID = new ConcurrentDictionary<ushort, NormalMap>();
            InstanceMapsByID = new ConcurrentDictionary<ushort, InstanceMap>();
            AvailablInstanceIds = new ConcurrentQueue<ushort>();

            AvailablInstanceIds.Fill(1, ushort.MaxValue);

            return true;
        }
        #endregion

        #region StartMethods
        public static bool StartMap(ushort MapId, ushort InstanceId, out WorldServerMap Map)
        {
            Map = null;

            if (!MapDataProvider.GetFieldInfosByMapID(MapId, out FieldInfo Info))
            {
                return false;
            }

            if (!ZoneManager.GetZoneByID(Info.ZoneID, out ZoneServer mZone) &&
                !mZone.IsConnected)
            {
                return false;
            }

            switch (Info.MapInfo.Type)
            {
                case MapType.Normal:

                    Map = new NormalMap(mZone, Info);

                    if (NormalMapsByID.TryAdd(Map.MapId, Map as NormalMap)
                        & mZone.MapList.Add(Map)
                        & Map.Start())
                    {
                        return true;
                    }
                    break;
                case MapType.Instance:
                    Map = new InstanceMap(mZone, Info, InstanceId);

                    if (InstanceMapsByID.TryAdd(InstanceId, Map as InstanceMap)
                        & mZone.MapList.Add(Map)
                        & Map.Start())

                    {
                        return true;
                    }
                    break;
                case MapType.KingdomQuest:
                    Map = new KingdomMap(mZone, Info, InstanceId);

                    if (InstanceMapsByID.TryAdd(InstanceId, Map as InstanceMap)
                         & mZone.MapList.Add(Map)
                         & Map.Start())

                    {
                        return true;
                    }
                    break;
                default:
                    return false;
            }

            return false;
        }
        #endregion

        #region StopMethods
        public static bool StopMap(ushort Mapid, int InstanceId = 0)
        {
            if (!MapDataProvider.GetMapInfoByID(Mapid, out MapInfo Info))
            {
                return false;
            }

            if (Info.Type != MapType.Normal)
            {
                if (GetInstancesOfMapId(Mapid, out List<InstanceMap> mMaps))
                {
                    foreach (var InstanceMap in mMaps)
                    {
                        StopInstanceMap(InstanceMap.InstanceId);
                    }
                    return true;
                }
            }
            else
            {
                if (GetNormalMapById(Mapid, out NormalMap mMap))
                {
                    return StopNormalMap(Mapid);
                }
            }

            return false;
        }

        private static bool StopInstanceMap(ushort InstanceId, bool Sending = true)
        {
            if (InstanceMapsByID.TryRemove(InstanceId, out InstanceMap RemoteMap))//remote
            {

                RemoteMap.Stop();
                if (Sending)
                {
                    MapMethods.SendStopMap(RemoteMap.MapId, InstanceId);
                }

                GameLog.Write(GameLogLevel.Debug, $"Stop InstanceMap MapID : {RemoteMap.MapId  }: {RemoteMap.InstanceId}");
                return true;
            }

            return false;
        }

        private static bool StopNormalMap(ushort MapId, bool Sending = true)
        {
            if (NormalMapsByID.TryRemove(MapId, out NormalMap mMap))
            {
                if (Sending)
                {
                    MapMethods.SendStopMap(MapId);
                }

                mMap.Stop();

                GameLog.Write(GameLogLevel.Debug, $"Stop Normal MapID : {MapId }");

                return true;
            }
            return false;
        }



        public static void StopMapList(List<IMap> MapList)
        {
            StopMapList Stop = new StopMapList()
            {
                Id = Guid.NewGuid(),
                MapsList = new List<StopMap>(),
            };

            foreach (var map in MapList)
            {


                var mapStopped = new StopMap
                {
                    MapId = map.MapId,
                    InstanceId = (map is IInstanceMap) ? (map as IInstanceMap).InstanceId : (ushort)0,
                };

                StopMap(mapStopped.MapId, mapStopped.InstanceId);

            }

            ZoneManager.Broadcast(Stop);
        }

        public static void StopMapsByZoneId(byte ZoneId)
        {
            if (ZoneManager.GetZoneByID(ZoneId, out ZoneServer Zone))
            {
                if (Zone.IsReady)
                {
                    StopMapList(Zone.MapList.ToList<IMap>());
                }
            }
        }

        #endregion

        #region GetMethods

       
        public static bool GetMap(ushort MapId, ushort InstanceId, out WorldServerMap Map)
        {
            Map = null;

            if (!MapDataProvider.GetFieldInfosByMapID(MapId, out FieldInfo Info))
            {
                return false;
            }

            return GetMap(Info, InstanceId, out Map);
        }
        public static bool GetMap(FieldInfo Info, ushort InstanceId, out WorldServerMap Map)
        {
            Map = null;

            if (Info.MapInfo.Type != MapType.Normal)
            {
                if (GetInstanceMapById(InstanceId, out InstanceMap Instance))
                {
                    Map = Instance;
                    return true;
                }
                return false;
            }
            else
            {
                if (GetNormalMapById(Info.MapInfo.ID, out NormalMap Normal))
                {
                    Map = Normal;
                    return true;
                }
            }
            return false;
        }
        public static bool GetInstancesOfMapId(ushort MapId, out List<InstanceMap> Maps)
        {
            Maps = InstanceMapsByID?.Values?.ToList()?.FindAll(m => m.MapId == MapId);
            return Maps != null;
        }

        public static bool GetNormalMapById(ushort MapID, out NormalMap map) => NormalMapsByID.TryGetValue(MapID, out map);

        public static bool GetInstanceMapById(ushort InstanceId, out InstanceMap Map) => InstanceMapsByID.TryGetValue(InstanceId, out Map);

        #endregion

    }
}