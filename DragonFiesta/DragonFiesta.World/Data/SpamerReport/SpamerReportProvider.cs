using DragonFiesta.Utils.IO.SHN;
using System.Diagnostics;

namespace DragonFiesta.World.Data.SpamerReport
{
    [GameServerModule(ServerType.World, GameInitalStage.SpamerReport)]
    public class SpamerReportProvider
    {
        protected static SecureCollection<SpamerReport> SpamerReportSC;

        [InitializerMethod]
        public static bool Initialize()
        {
            LoadSpamerReport();
            return true;
        }

        public static void LoadSpamerReport()
        {
            var watch = Stopwatch.StartNew();
            SpamerReportSC = new SecureCollection<SpamerReport>();

            var pResult = SHNManager.Load(SHNType.SpamerReport);
            DataLog.WriteProgressBar(">> Load SpamerReport");

            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new SpamerReport(pResult, i);
                    SpamerReportSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {SpamerReportSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }
    }
}