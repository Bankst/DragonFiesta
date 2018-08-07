using DragonFiesta.Utils.IO.SHN;
using System.Collections.Concurrent;
using System.Diagnostics;

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
            var watch = Stopwatch.StartNew();
            FaceInfosByID = new ConcurrentDictionary<byte, FaceInfo>();

            SHNResult pResult = SHNManager.Load(SHNType.FaceInfo);
            DataLog.WriteProgressBar(">> Load FaceInfos");

            using (ProgressBar mBar = new ProgressBar(pResult.Count))
            {
                for (int i = 0; i < pResult.Count; i++)
                {
                    var info = new FaceInfo(pResult, i);

                    if (!FaceInfosByID.TryAdd(info.ID, info))
                    {
                        DataLog.Write(DataLogLevel.Warning, $"Duplicate face id found. ID: {info.ID}");
                    }
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {FaceInfosByID.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }

        private static void LoadHairInfos()
        {
            var watch = Stopwatch.StartNew();
            HairInfosByID = new ConcurrentDictionary<byte, HairInfo>();

            SHNResult pResult = SHNManager.Load(SHNType.HairInfo);
            DataLog.WriteProgressBar(">> Load HairInfos");

            using (ProgressBar mBar = new ProgressBar(pResult.Count))
            {
                for (int i = 0; i < pResult.Count; i++)
                {
                    var info = new HairInfo(pResult, i);

                    if (!HairInfosByID.TryAdd(info.ID, info))
                    {
                        DataLog.Write(DataLogLevel.Warning, $"Duplicate hair id found. ID: {info.ID}");
                    }
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {HairInfosByID.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }

        private static void LoadHairColorsInfos()
        {
            var watch = Stopwatch.StartNew();
            HairColorInfosByID = new ConcurrentDictionary<byte, HairColorInfo>();

            SHNResult pResult = SHNManager.Load(SHNType.HairColorInfo);
            DataLog.WriteProgressBar(">> Load HairColorInfos");

            using (ProgressBar mBar = new ProgressBar(pResult.Count))
            {
                for (int i = 0; i < pResult.Count; i++)
                {
                    var info = new HairColorInfo(pResult, i);

                    if (!HairColorInfosByID.TryAdd(info.ID, info))
                    {
                        DataLog.Write(DataLogLevel.Warning, $"Duplicate hair color id found. ID: {info.ID}");
                    }
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {HairColorInfosByID.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
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