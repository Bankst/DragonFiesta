using DragonFiesta.Utils.IO.SHN;
using System.Diagnostics;

namespace DragonFiesta.World.Data.GBHouse
{
    [GameServerModule(ServerType.World, GameInitalStage.GBHouse)]
    public class GBHouseProvider
    {
        protected static SecureCollection<GBHouse> GBHouseSC;

        [InitializerMethod]
        public static bool Initialize()
        {
            LoadGBHouse();
            return true;
        }

        public static void LoadGBHouse()
        {
            var watch = Stopwatch.StartNew();
            GBHouseSC = new SecureCollection<GBHouse>();

            var pResult = SHNManager.Load(SHNType.GBHouse);
            DataLog.WriteProgressBar(">> Load GBHouse");

            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new GBHouse(pResult, i);
                    GBHouseSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {GBHouseSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }
    }
}
