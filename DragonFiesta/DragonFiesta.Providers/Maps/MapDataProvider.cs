using DragonFiesta.Utils.Config;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DragonFiesta.Providers.Maps
{
    [GameServerModule(ServerType.World, GameInitalStage.Map)]
    public class MapDataProvider
    {
        public static SecureCollection<FieldInfo> FieldInfos { get; private set; }
        public static SecureCollection<MapInfo> MapInfos { get; private set; }
        private static ConcurrentDictionary<ushort, MapInfo> MapInfosByID;
        private static ConcurrentDictionary<string, MapInfo> MapInfosByIndex;
        private static ConcurrentDictionary<ushort, FieldInfo> FieldInfosByMapID;
        public static MapInfo DefaultMapInfo { get; private set; }
        public static MapInfo TutorialMapInfo { get; private set; }

        [InitializerMethod]
        public static bool Initialize()
        {
            LoadMapInfos();
            LoadFieldInfos();
            LoadLevelCondition();
            return true;
        }

        private static void LoadLevelCondition()
        {
            int Count = 0;
            SQLResult pResult = DB.Select(DatabaseType.Data, "SELECT * FROM FieldLvCondition");
            DatabaseLog.WriteProgressBar(">> Load Level Condition");
            using (ProgressBar mBar = new ProgressBar(pResult.Count))
            {
                for (int i = 0; i < pResult.Count; i++)
                {
                    mBar.Step();
                    ushort MapID = pResult.Read<ushort>(i, "MapID");
                    if (GetMapInfoByID(MapID, out MapInfo mMap))
                    {
                        mMap.LevelCondition = new FieldLvCondition(pResult, i);
                        Count++;
                    }
                }
                DatabaseLog.WriteProgressBar(">> Loaded {0} Level Condition", Count);
            }
        }

        private static void LoadMapInfos()
        {
            MapInfos = new SecureCollection<MapInfo>();
            MapInfosByID = new ConcurrentDictionary<ushort, MapInfo>();
            MapInfosByIndex = new ConcurrentDictionary<string, MapInfo>();
            SQLResult pResult = DB.Select(DatabaseType.Data, "SELECT * FROM MapInfos");
            DatabaseLog.WriteProgressBar(">> Load Map Infos");
            using (ProgressBar mBar = new ProgressBar(pResult.Count))
            {
                for (int i = 0; i < pResult.Count; i++)
                {
                    var info = new MapInfo(pResult, i);
                    if (!MapInfosByID.TryAdd(info.ID, info))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate map id found: " + info.ID);
                        continue;
                    }
                    if (!MapInfosByIndex.TryAdd(info.Index, info))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate map index found: " + info.Index);
                        //reset
                        MapInfosByID.TryRemove(info.ID, out info);
                        continue;
                    }
                    MapInfos.Add(info);
                    mBar.Step();
                }
                DatabaseLog.WriteProgressBar(">> Loaded {0} Map Infos", MapInfos.Count);
            }

            if (!GetMapInfoByID(GameConfiguration.Instance.DefaultSpawnMapId, out MapInfo DefaultMap))
                throw new KeyNotFoundException("Cant find default spawn map " + GameConfiguration.Instance.DefaultSpawnMapId + "");

            if (!GetMapInfoByID(GameConfiguration.Instance.TutorialMap, out MapInfo TutorialMap))
                throw new KeyNotFoundException("Cant find TutorialMap  " + GameConfiguration.Instance.TutorialMap + "");

            DefaultMapInfo = DefaultMap;
            TutorialMapInfo = TutorialMap;
        }

        private static void LoadFieldInfos()
        {
            FieldInfos = new SecureCollection<FieldInfo>();
            FieldInfosByMapID = new ConcurrentDictionary<ushort, FieldInfo>();

            SQLResult pResult = DB.Select(DatabaseType.Data, "SELECT * FROM FieldInfos");
            DatabaseLog.WriteProgressBar(">> Load Field Infos");
            using (ProgressBar mBar = new ProgressBar(pResult.Count))
            {
                for (int i = 0; i < pResult.Count; i++)
                {
                    mBar.Step();

                    ushort MapClientID = pResult.Read<ushort>(i, "MapID");
                    //load map
                    if (!MapInfosByID.TryGetValue(MapClientID, out MapInfo map))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Can't find map  field info. Map ID: '{0}'.", MapClientID);
                        continue;
                    }
                    var info = new FieldInfo(pResult, i)
                    {
                        MapInfo = map,
                    };
                    //load regen map
                    if (!MapInfosByID.TryGetValue(pResult.Read<ushort>(i, "RegenCity"), out MapInfo regenMap))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Can't find regen map Map ID: '{0}'.", MapClientID);
                        continue;
                    }

                    info.RegenMap = regenMap;

                    if (!FieldInfosByMapID.TryAdd(info.MapInfo.ID, info))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate field  found. Map ID: {0}", MapClientID);
                        continue;
                    }

                    FieldInfos.Add(info);
                }
                DatabaseLog.WriteProgressBar(">> Loaded {0} Field Infos", FieldInfos.Count);
            }
        }

        public static bool GetMapInfoByID(ushort MapID, out MapInfo MapInfo)
        {
            return MapInfosByID.TryGetValue(MapID, out MapInfo);
        }

        public static bool GetMapInfoByIndex(string MapIndex, out MapInfo MapInfo)
        {
            return MapInfosByIndex.TryGetValue(MapIndex, out MapInfo);
        }

        public static bool GetFieldInfosByMapID(ushort Serial, out FieldInfo FieldInfo)
        {
            return FieldInfosByMapID.TryGetValue(Serial, out FieldInfo);
        }
    }
}