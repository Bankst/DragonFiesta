using DragonFiesta.Providers.Maps;
using DragonFiesta.Zone.Config;
using DragonFiesta.Zone.Game.Zone;
using System.Collections.Concurrent;
using DragonFiesta.Utils.Logging;
using DragonFiesta.Zone.Game.Maps.Types;

namespace DragonFiesta.Zone.Game.Maps.Object
{
    [ServerModule(ServerType.Zone, InitializationStage.Logic)]
    public class MapManager
    {
        #region Properties

        private static ConcurrentDictionary<ushort, NormalMap> NormalMapsByID;
        private static ConcurrentDictionary<int, InstanceMap> InstanceMapsByID;

        private static ConcurrentDictionary<ushort, RemoteMap> RemoteMapsByID;
        private static ConcurrentDictionary<int, RemoteInstanceMap> RemoteInstanceMapByID;


        [InitializerMethod]
        public static bool InitialMapManager()
        {
            NormalMapsByID = new ConcurrentDictionary<ushort, NormalMap>();
            InstanceMapsByID = new ConcurrentDictionary<int, InstanceMap>();
            RemoteMapsByID = new ConcurrentDictionary<ushort, RemoteMap>();
            RemoteInstanceMapByID = new ConcurrentDictionary<int, RemoteInstanceMap>();

            return true;
        }


        #endregion Properties


        public static void Dispose()
        {
            //Clear All Maps
            NormalMapsByID.Clear();
            InstanceMapsByID.Clear();
            RemoteMapsByID.Clear();
            RemoteInstanceMapByID.Clear();
        }

  

        #region GetMethods
        public static bool GetMap(ushort MapId, ushort InstanceId, out ZoneServerMap Map)
        {
            Map = null;

            if (!MapDataProvider.GetFieldInfosByMapID(MapId, out FieldInfo Info))
            {
                return false;
            }

            return GetMap(Info, InstanceId, out Map);
        }
        public static bool GetMap(FieldInfo Info, ushort InstanceId, out ZoneServerMap Map)
        {
            if (Info.ZoneID == ZoneConfiguration.Instance.ZoneID)
                return GetLocalMap(Info, out Map, InstanceId);
            else
                return GetRemoteMap(Info, out Map, InstanceId);
        }

        private static bool GetRemoteMap(FieldInfo Info, out ZoneServerMap Map, ushort InstanceId)
        {
            Map = null;

            if (Info.MapInfo.Type != MapType.Normal)
            {
                if (RemoteInstanceMapByID.TryGetValue(Info.MapInfo.ID, out RemoteInstanceMap RemoteInstance)
                    && RemoteInstance.MapId == Info.MapInfo.ID)
                {
                    Map = RemoteInstance;
                    return true;
                }

                return false;
            }
            else
            {
                if (RemoteMapsByID.TryGetValue(Info.MapInfo.ID, out RemoteMap RemoteMap))
                {
                    Map = RemoteMap;
                    return true;
                }
                return false;
            }
        }


        private static bool GetLocalMap(FieldInfo Info, out ZoneServerMap Map, ushort InstanceId = 0)
        {
            Map = null;

            if (Info.MapInfo.Type != MapType.Normal)
            {
                if (InstanceMapsByID.TryGetValue(InstanceId, out InstanceMap Instance)
                    && Instance.MapId == Info.MapInfo.ID)
                {
                    Map = Instance;
                    return true;
                }
            }
            else
            {
                if (NormalMapsByID.TryGetValue(Info.MapInfo.ID, out NormalMap Normal))
                {
                    Map = Normal;
                    return true;
                }
            }

            return false;
        }

     
        #endregion

        #region StartMethods
        public static bool StartNewMap(FieldInfo mInfo, ushort Instance = 0)
        {
            if (ZoneConfiguration.Instance.ZoneID == mInfo.ZoneID)
            {
                return StartLocalMap(mInfo, Instance);
            }
            else
            {
                return StartRemoteMap(mInfo, Instance);
            }
        }


        private static bool StartLocalMap(FieldInfo Info, ushort InstanceId = 0)
        {
            LocalMap mMap = null;

            switch (Info.MapInfo.Type)
            {
                case MapType.Normal:

                    mMap = new NormalMap(Info);

                    if (!NormalMapsByID.TryAdd(mMap.MapId, mMap as NormalMap))
                    {
                        return false;
                    }
                    break;
                case MapType.KingdomQuest:
                    mMap = new KingdomMap(InstanceId, Info);

                    if (!InstanceMapsByID.TryAdd(InstanceId, mMap as InstanceMap))
                    {
                        return false;
                    }

                    break;
                case MapType.Instance:

                    mMap = new InstanceMap(InstanceId, Info);

                    if (!InstanceMapsByID.TryAdd(InstanceId, mMap as InstanceMap))
                    {
                        return false;
                    }
                    break;
                default:
                    return false;
            }



            GameLog.Write(GameLogLevel.Debug, $"Start New LocalMap  ID : {mMap.MapId }");

            return mMap.Start();
        }
        private static bool StartRemoteMap(FieldInfo mInfo, ushort InstanceId = 0)
        {
            if (!ZoneManager.GetRemoteZoneByID(mInfo.ZoneID, out RemoteZone mZone))
            {
                return false;
            }

            RemoteMap mMap = null;

            if (mInfo.MapInfo.Type != MapType.Normal)
            {
                mMap = new RemoteInstanceMap(InstanceId, mZone, mInfo);

                if (!RemoteInstanceMapByID.TryAdd(InstanceId, mMap as RemoteInstanceMap))
                {
                    return false;
                }
            }
            else
            {
                mMap = new RemoteMap(mZone, mInfo);

                if (!RemoteMapsByID.TryAdd(mMap.MapId, mMap))
                {
                    return false;
                }
            }

            GameLog.Write(GameLogLevel.Debug, $"Start New RemoteMap  ID : {mInfo.MapInfo.ID }");

            return mMap.Start();
        }

        #endregion Start Maps Logics

        #region StopMethods

        public static bool StopMap(ushort MapId, int InstanceId = 0)
        {
            if (!MapDataProvider.GetFieldInfosByMapID(MapId, out FieldInfo Info))
            {
                return false;
            }

            if (ZoneConfiguration.Instance.ZoneID == Info.ZoneID)
            {
                return StopLocalMap(Info, InstanceId);
            }
            else
            {
                return StopRemoteMap(Info, InstanceId);
            }
        }



        public static bool StopLocalMap(FieldInfo Info, int InstanceId = 0)
        {
            if (Info.MapInfo.Type != MapType.Normal)
            {
                if (InstanceMapsByID.TryRemove(InstanceId, out InstanceMap mMap)) //local maps
                {
                    GameLog.Write(GameLogLevel.Debug, $"Stoped local InstanceMap : ID : { InstanceId } MapID : { mMap.MapId}");
                    return mMap.Stop();
                }
            }
            else
            {
                if (NormalMapsByID.TryRemove(Info.MapInfo.ID, out NormalMap mMap))
                {
                    GameLog.Write(GameLogLevel.Debug, $"Stoped Local NormalMap  MapID : {Info.MapInfo.ID }");
                    return mMap.Stop();
                }
            }

            return false;
        }

        public static bool StopRemoteMap(FieldInfo Info, int InstanceId = 0)
        {
            if (Info.MapInfo.Type != MapType.Normal)
            {
                if (RemoteInstanceMapByID.TryRemove(InstanceId, out RemoteInstanceMap RemoteMap))//remote
                {
                    GameLog.Write(GameLogLevel.Debug, $"Stoped RemoteInstance Map : ID : { InstanceId } MapID : {RemoteMap.MapId }");
                    return RemoteMap.Stop();
                }
            }
            else
            {
                if (RemoteMapsByID.TryRemove(Info.MapInfo.ID, out RemoteMap RemoteMap))
                {
                    GameLog.Write(GameLogLevel.Debug, $"Stoped remote NormalMap  MapID : {Info.MapInfo.ID }");
                    return RemoteMap.Stop();
                }
            }

            return false;
        }

        #endregion

    }
}