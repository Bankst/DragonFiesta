using System.Collections.Concurrent;

namespace DragonFiesta.Providers.Characters
{
    [GameServerModule(ServerType.Zone, GameInitalStage.CharacterData)]
    [GameServerModule(ServerType.World, GameInitalStage.CharacterData)]
    public class CharacterLookProvider
    {
        private static ConcurrentDictionary<byte, FaceInfo> FaceInfosByID;
        private static ConcurrentDictionary<byte, HairInfo> HairInfosByID;
        private static ConcurrentDictionary<byte, HairColorInfo> HairColorInfosByID;

        [InitializerMethod]
        public static bool OneStart()
        {
            LoadFaceInfos();
            LoadHairColorsInfos();
            LoadHairInfos();
            return true;
        }

        private static void LoadFaceInfos()
        {
            FaceInfosByID = new ConcurrentDictionary<byte, FaceInfo>();

            SQLResult pResult = DB.Select(DatabaseType.Data, "SELECT * FROM FaceInfos");

            DatabaseLog.WriteProgressBar(">> Load FaceInfos");

            using (ProgressBar mBar = new ProgressBar(pResult.Count))
            {
                for (int i = 0; i < pResult.Count; i++)
                {
                    var info = new FaceInfo(pResult, i);

                    if (!FaceInfosByID.TryAdd(info.ID, info))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate face id found. ID: {0}", info.ID);
                    }
                    mBar.Step();
                }
                DatabaseLog.WriteProgressBar(">> Loaded {0} FaceInfos", FaceInfosByID.Count);
            }
        }

        private static void LoadHairInfos()
        {
            HairInfosByID = new ConcurrentDictionary<byte, HairInfo>();

            SQLResult pResult = DB.Select(DatabaseType.Data, "SELECT * FROM HairInfos");

            DatabaseLog.WriteProgressBar(">> Load HairInfos");

            using (ProgressBar mBar = new ProgressBar(pResult.Count))
            {
                for (int i = 0; i < pResult.Count; i++)
                {
                    var info = new HairInfo(pResult, i);

                    if (!HairInfosByID.TryAdd(info.ID, info))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate hair id found. ID: {0}", info.ID);
                    }
                    mBar.Step();
                }
                DatabaseLog.WriteProgressBar(">> Loaded {0} HairInfos", HairInfosByID.Count);
            }
        }

        private static void LoadHairColorsInfos()
        {
            HairColorInfosByID = new ConcurrentDictionary<byte, HairColorInfo>();

            SQLResult pResult = DB.Select(DatabaseType.Data, "SELECT * FROM HairColorInfos");

            DatabaseLog.WriteProgressBar(">> Load HairColorInfos");

            using (ProgressBar mBar = new ProgressBar(pResult.Count))
            {
                for (int i = 0; i < pResult.Count; i++)
                {
                    var info = new HairColorInfo(pResult, i);

                    if (!HairColorInfosByID.TryAdd(info.ID, info))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate hair color id found. ID: {0}", info.ID);
                    }
                    mBar.Step();
                }

                DatabaseLog.WriteProgressBar(">> Loaded {0} HairColorInfos", HairColorInfosByID.Count);
            }
        }

        public static bool GetFaceInfoByID(byte ID, out FaceInfo FaceInfo)
        {
            return FaceInfosByID.TryGetValue(ID, out FaceInfo);
        }

        public static bool GetHairInfoByID(byte ID, out HairInfo HairInfo)
        {
            return HairInfosByID.TryGetValue(ID, out HairInfo);
        }

        public static bool GetHairColorInfoByID(byte ID, out HairColorInfo HairColorInfo)
        {
            return HairColorInfosByID.TryGetValue(ID, out HairColorInfo);
        }
    }
}