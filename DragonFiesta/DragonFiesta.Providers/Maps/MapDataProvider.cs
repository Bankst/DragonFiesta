using DragonFiesta.Utils.Config;
using DragonFiesta.Utils.IO.SHN;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DragonFiesta.Database.SQL;

namespace DragonFiesta.Providers.Maps
{
    [GameServerModule(ServerType.World, GameInitalStage.Map)]
    public class MapDataProvider
    {
        public static SecureCollection<FieldInfo> FieldInfos { get; private set; }
        public static SecureCollection<MapInfo> MapInfos { get; private set; }
        private static ConcurrentDictionary<ushort, MapInfo> _mapInfosByID;
        private static ConcurrentDictionary<string, MapInfo> _mapInfosByIndex;
        private static ConcurrentDictionary<ushort, FieldInfo> _fieldInfosByMapID;
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
			var watch = Stopwatch.StartNew();
            var count = 0;
			var pResult = SHNManager.Load(SHNType.FieldLvCondition);
            DataLog.WriteProgressBar(">> Load Level Condition");
            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    mBar.Step();
                    var mapId = pResult.Read<string>(i, "MapIndex");
	                var mMap = MapInfos.First(x => x.Index == mapId);
	                mMap.LevelCondition = new FieldLvCondition(pResult, i);
	                count++;
                }
				watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {count} Level Condition from SHN in {(double)watch.ElapsedMilliseconds / 1000}");
            }
        }

        private static void LoadMapInfos()
        {
	        var watch = Stopwatch.StartNew();
            MapInfos = new SecureCollection<MapInfo>();
            _mapInfosByID = new ConcurrentDictionary<ushort, MapInfo>();
            _mapInfosByIndex = new ConcurrentDictionary<string, MapInfo>();
            var pResult = SHNManager.Load(SHNType.MapInfo);
            DataLog.WriteProgressBar(">> Load MapInfos");
            using (var mBar = new ProgressBar(pResult.Count))
            {
	            for (var i = 0; i < pResult.Count; i++)
	            {
		            var info = new MapInfo(pResult, i);
		            if (!_mapInfosByID.TryAdd(info.ID, info))
		            {
                        DataLog.Write(DataLogLevel.Warning, $"Duplicate map id found: {info.ID}");
			            continue;
		            }

		            if (!_mapInfosByIndex.TryAdd(info.Index, info))
		            {
                        DataLog.Write(DataLogLevel.Warning, $"Duplicate map index found: {info.Index}");
			            //reset
			            _mapInfosByID.TryRemove(info.ID, out info);
			            continue;
		            }

		            MapInfos.Add(info);
		            mBar.Step();
	            }
	            watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {MapInfos.Count} Map Infos from SHN in {(double) watch.ElapsedMilliseconds / 1000}s");
            }

            if (!GetMapInfoByID(GameConfiguration.Instance.DefaultSpawnMapId, out var defaultMap))
                throw new KeyNotFoundException("Can't find default spawn map {GameConfiguration.Instance.DefaultSpawnMapId}");

            if (!GetMapInfoByID(GameConfiguration.Instance.TutorialMap, out var tutorialMap))
                throw new KeyNotFoundException($"Can't find TutorialMap {GameConfiguration.Instance.TutorialMap}");

            DefaultMapInfo = defaultMap;
            TutorialMapInfo = tutorialMap;
        }

        private static void LoadFieldInfos()
        {
	        var watch = Stopwatch.StartNew();
            FieldInfos = new SecureCollection<FieldInfo>();
            _fieldInfosByMapID = new ConcurrentDictionary<ushort, FieldInfo>();

            var pResult = DB.Select(DatabaseType.Data, "SELECT * FROM FieldInfos");
            DataLog.WriteProgressBar(">> Load Field Infos");
            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    mBar.Step();

                    var mapClientId = pResult.Read<ushort>(i, "MapID");
                    //load map
                    if (!_mapInfosByID.TryGetValue(mapClientId, out var map))
                    {
                        DataLog.Write(DataLogLevel.Warning, "Can't find map field info. Map ID: '{0}'", mapClientId);
                        continue;
                    }
                    var info = new FieldInfo(pResult, i)
                    {
                        MapInfo = map,
                    };
                    //load regen map
                    if (!_mapInfosByID.TryGetValue(pResult.Read<ushort>(i, "RegenCity"), out var regenMap))
                    {
                        DataLog.Write(DataLogLevel.Warning, "Can't find regen map. Map ID: '{0}'", mapClientId);
                        continue;
                    }

                    info.RegenMap = regenMap;

                    if (!_fieldInfosByMapID.TryAdd(info.MapInfo.ID, info))
                    {
                        DataLog.Write(DataLogLevel.Warning, "Duplicate field found. Map ID: {0}", mapClientId);
                        continue;
                    }

                    FieldInfos.Add(info);
                }

	            watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {FieldInfos.Count} Field Infos from Database in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }

        public static bool GetMapInfoByID(ushort mapID, out MapInfo mapInfo)
        {
            return _mapInfosByID.TryGetValue(mapID, out mapInfo);
        }

        public static bool GetMapInfoByIndex(string mapIndex, out MapInfo mapInfo)
        {
            return _mapInfosByIndex.TryGetValue(mapIndex, out mapInfo);
        }

        public static bool GetFieldInfosByMapID(ushort serial, out FieldInfo fieldInfo)
        {
            return _fieldInfosByMapID.TryGetValue(serial, out fieldInfo);
        }
    }
}