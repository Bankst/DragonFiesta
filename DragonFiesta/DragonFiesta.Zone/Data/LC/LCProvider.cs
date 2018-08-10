using System.Diagnostics;
using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Zone.Data.LC
{
    [GameServerModule(ServerType.Zone, GameInitalStage.LC)]
    public class LCProvider
    {
        protected static SecureCollection<LCGroupRate> LCGroupRateSC;
        protected static SecureCollection<LCReward> LCRewardSC;

        [InitializerMethod]
        public static bool Initialize()
        {
            LoadLCReward();
            LoadLCGroupRate();
            return true;
        }


        public static void LoadLCReward()
        {
            var watch = Stopwatch.StartNew();
            LCRewardSC = new SecureCollection<LCReward>();

            var pResult = SHNManager.Load(SHNType.LCReward);
            DataLog.WriteProgressBar(">> Load LCReward");
            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new LCReward(pResult, i);
                    LCRewardSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {LCRewardSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }

        public static void LoadLCGroupRate()
        {
            var watch = Stopwatch.StartNew();
            LCGroupRateSC = new SecureCollection<LCGroupRate>();

            var pResult = SHNManager.Load(SHNType.LCGroupRate);
            DataLog.WriteProgressBar(">> Load LCGroupRate");
            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new LCGroupRate(pResult, i);
                    LCGroupRateSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {LCGroupRateSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }
    }
}
