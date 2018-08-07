using DragonFiesta.Providers.Maps;
using System;
using System.Collections.Concurrent;
using System.IO;
using DragonFiesta.Database.SQL;

namespace DragonFiesta.Zone.Data.Maps
{
    [ServerModule(ServerType.Zone, InitializationStage.Data)]
    public class ZoneMapDataProvider : MapDataProvider
    {
        private static ConcurrentDictionary<ushort, BlockInfo> BlockInfosByMapID;
        private static ConcurrentDictionary<byte, TownPortalInfo> TownPortalInfosByID;

        [InitializerMethod]
        public static new bool Initialize()
        {
            if (!MapDataProvider.Initialize())
                return false;

            LoadBlockInfos();
            LoadTownPortalInfos();

            return true;
        }

        private static void LoadTownPortalInfos()
        {
            TownPortalInfosByID = new ConcurrentDictionary<byte, TownPortalInfo>();
            SQLResult pResult = DB.Select(DatabaseType.Data, "SELECT * FROM TownPortals");
            DatabaseLog.WriteProgressBar(">> Load town portal infos");
            using (ProgressBar mBar = new ProgressBar(pResult.Count))
            {
                for (int i = 0; i < pResult.Count; i++)
                {
                    var info = new TownPortalInfo(pResult, i);

                    mBar.Step();

                    if (!TownPortalInfosByID.TryAdd(info.Index, info))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate town portal index found: " + info.Index);
                        continue;
                    }
                }

                DatabaseLog.WriteProgressBar(">> Loaded {0} town portal infos", TownPortalInfosByID.Count);
            }
        }

        private static void LoadBlockInfos()
        {
            BlockInfosByMapID = new ConcurrentDictionary<ushort, BlockInfo>();
            SQLResult pResult = DB.Select(DatabaseType.Data, "SELECT * FROM BlockInfos");
            int filecounter = 0;
            DatabaseLog.WriteProgressBar(">> Load BlockInfos ");
            using (ProgressBar mBar = new ProgressBar(pResult.Count))
            {
                for (int i = 0; i < pResult.Count; i++)
                {
                    bool LoadBlock = pResult.Read<bool>(i, "LoadBlock");
                    ushort MapID = pResult.Read<ushort>(i, "MapID");
                    BlockInfo blockInfo = null;

                    MapInfo inf;
                    if (!GetMapInfoByID(MapID, out inf))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Can't find block info  from mapid '{0}'", MapID);
                        continue;
                    }

                    if (LoadBlock)
                    {
                        string filename = pResult.Read<string>(i, "BlockFileName");
                        var blockName = String.Format("BlockInfo\\{0}.shbd", filename);
                        if (!File.Exists(blockName))
                        {
                            DatabaseLog.Write(DatabaseLogLevel.Warning, "Can't find block info for map '{0}'. Map will NOT be loaded.", inf.Index);
                            MapInfos.Remove(inf);
                            continue;
                        }

                        blockInfo = new BlockInfo(blockName);
                        filecounter++;
                    }
                    else
                    {
                        blockInfo = new BlockInfo(pResult, i);
                    }

                    mBar.Step();
                    if (!BlockInfosByMapID.TryAdd(inf.ID, blockInfo))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate block info found. Map ID: {0}", inf.ID);
                        continue;
                    }
                }
                foreach (var m in MapInfos)
                {
                    if (!BlockInfosByMapID.ContainsKey(m.ID))
                    {
                        MapInfos.Remove(m);
                        DatabaseLog.Write(DatabaseLogLevel.Warning, " map '{0} has not Blockinfo '. Map will NOT be loaded.", m.ID);
                    }
                }

                DatabaseLog.WriteProgressBar(">>  Loaded {0} blocks with {1} files ", BlockInfosByMapID.Count, filecounter);
            }
        }

        public static bool GetBlockInfoByMapID(ushort MapID, out BlockInfo BlockInfo)
        {
            return BlockInfosByMapID.TryGetValue(MapID, out BlockInfo);
        }

        public static bool GetTownPortalInfoByID(byte Index, out TownPortalInfo TownPortalInfo)
        {
            return TownPortalInfosByID.TryGetValue(Index, out TownPortalInfo);
        }
    }
}