using System.Collections.Concurrent;
using System.Diagnostics;
using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Zone.Data.MiniHouse
{
    [GameServerModule(ServerType.Zone, GameInitalStage.MiniHouse)]
    public class MiniHouseProvider
    {
        protected static ConcurrentDictionary<ushort, MiniHouse> MiniHouseByHandle;
        protected static SecureCollection<MiniHouse> MiniHouseSC;
        protected static SecureCollection<MiniHouseEndure> MiniHouseEndureSC;
        protected static SecureCollection<MiniHouseObjAni> MiniHouseObjAniSC;

        [InitializerMethod]
        public static bool Initialize()
        {
            LoadMiniHouse();
            LoadMiniHouseEndure();
            LoadMiniHouseObjAni();
            return true;
        }

        public static void LoadMiniHouse()
        {
            var watch = Stopwatch.StartNew();
            MiniHouseSC = new SecureCollection<MiniHouse>();
            MiniHouseByHandle = new ConcurrentDictionary<ushort, MiniHouse>();

            var pResult = SHNManager.Load(SHNType.MiniHouse);
            DataLog.WriteProgressBar(">> Load MiniHouse");
            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new MiniHouse(pResult, i);

                    if (!MiniHouseByHandle.TryAdd(info.Handle, info))
                    {
                        DataLog.Write(DataLogLevel.Warning, $"Duplicate item id found. ID: {info.Handle}");
                        continue;
                    }
                    MiniHouseSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {MiniHouseSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }

        public static void LoadMiniHouseEndure()
        {
            var watch = Stopwatch.StartNew();
            MiniHouseEndureSC = new SecureCollection<MiniHouseEndure>();

            var pResult = SHNManager.Load(SHNType.MiniHouseEndure);
            DataLog.WriteProgressBar(">> Load MiniHouseEndure");
            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new MiniHouseEndure(pResult, i);
                    MiniHouseEndureSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {MiniHouseEndureSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }

        public static void LoadMiniHouseObjAni()
        {
            var watch = Stopwatch.StartNew();
            MiniHouseObjAniSC = new SecureCollection<MiniHouseObjAni>();

            var pResult = SHNManager.Load(SHNType.MiniHouseObjAni);
            DataLog.WriteProgressBar(">> Load MiniHouseObjAni");
            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new MiniHouseObjAni(pResult, i);
                    MiniHouseObjAniSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {MiniHouseObjAniSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }
    }
}
