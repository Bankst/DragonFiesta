using DragonFiesta.Utils.IO.SHN;
using System.Diagnostics;

namespace DragonFiesta.Zone.Data.Gather
{
    [GameServerModule(ServerType.Zone, GameInitalStage.Gather)]
    public class GatherProvider
    {
        protected static SecureCollection<Gather> GatherSC;

        [InitializerMethod]
        public static bool Initialize()
        {
            LoadGather();
            return true;
        }

        public static void LoadGather()
        {
            var watch = Stopwatch.StartNew();
            GatherSC = new SecureCollection<Gather>();

            var pResult = SHNManager.Load(SHNType.Gather);
            DataLog.WriteProgressBar(">> Load Gather");

            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new Gather(pResult, i);
                    GatherSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {GatherSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }
    }
}
