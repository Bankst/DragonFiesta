using DragonFiesta.Utils.IO.SHN;
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
        public static bool OnStart()
        {
            LoadFaceInfos();
            LoadHairColorsInfos();
            LoadHairInfos();
            return true;
        }

        private static void LoadFaceInfos()
        {
            FaceInfosByID = new ConcurrentDictionary<byte, FaceInfo>();

            SHNResult pResult = SHNManager.Load(SHNType.FaceInfo);
            DatabaseLog.WriteProgressBar(">> Load FaceInfos");

            using (ProgressBar mBar = new ProgressBar(pResult.Count))
            {
                for (int i = 0; i < pResult.Count; i++)
                {
                    var info = new FaceInfo(pResult, i);

                    if (!FaceInfosByID.TryAdd(info.ID, info))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, $"Duplicate face id found. ID: {info.ID}");
                    }
                    mBar.Step();
                }
                DatabaseLog.WriteProgressBar($">> Loaded {FaceInfosByID.Count} FaceInfos");
            }
        }

        private static void LoadHairInfos()
        {
            HairInfosByID = new ConcurrentDictionary<byte, HairInfo>();

            SHNResult pResult = SHNManager.Load(SHNType.HairInfo);

            DatabaseLog.WriteProgressBar(">> Load HairInfos");

            using (ProgressBar mBar = new ProgressBar(pResult.Count))
            {
                for (int i = 0; i < pResult.Count; i++)
                {
                    var info = new HairInfo(pResult, i);

                    if (!HairInfosByID.TryAdd(info.ID, info))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, $"Duplicate hair id found. ID: {info.ID}");
                    }
                    mBar.Step();
                }
                DatabaseLog.WriteProgressBar($">> Loaded {HairInfosByID.Count} HairInfos");
            }
        }

        private static void LoadHairColorsInfos()
        {
            HairColorInfosByID = new ConcurrentDictionary<byte, HairColorInfo>();

            SHNResult pResult = SHNManager.Load(SHNType.HairColorInfo);

            DatabaseLog.WriteProgressBar(">> Load HairColorInfos");

            using (ProgressBar mBar = new ProgressBar(pResult.Count))
            {
                for (int i = 0; i < pResult.Count; i++)
                {
                    var info = new HairColorInfo(pResult, i);

                    if (!HairColorInfosByID.TryAdd(info.ID, info))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, $"Duplicate hair color id found. ID: {info.ID}");
                    }
                    mBar.Step();
                }

                DatabaseLog.WriteProgressBar($">> Loaded {HairColorInfosByID.Count} HairColorInfos");
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