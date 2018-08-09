using DragonFiesta.Utils.IO.SHN;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace DragonFiesta.Zone.Data.Riding
{
    [GameServerModule(ServerType.Zone, GameInitalStage.Riding)]
    public class RidingProvider
    {
        protected static ConcurrentDictionary<ushort, Riding> RidingByHandle;
        protected static SecureCollection<Riding> RidingSC;

        [InitializerMethod]
        public static bool Initialize()
        {
            LoadRiding();
            return true;
        }

        public static void LoadRiding()
        {
            var watch = Stopwatch.StartNew();
            RidingSC = new SecureCollection<Riding>();
            RidingByHandle = new ConcurrentDictionary<ushort, Riding>();

            var pResult = SHNManager.Load(SHNType.Riding);
            DataLog.WriteProgressBar(">> Load Riding");
            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new Riding(pResult, i);

                    if (!RidingByHandle.TryAdd(info.Handle, info))
                    {
                        DataLog.Write(DataLogLevel.Warning, $"Duplicate Handle ID found. ID: {info.Handle}");
                        continue;
                    }
                    RidingSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {RidingSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }
    }
}
