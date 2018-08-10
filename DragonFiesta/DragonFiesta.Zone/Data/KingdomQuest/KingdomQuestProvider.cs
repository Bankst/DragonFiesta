using DragonFiesta.Utils.IO.SHN;
using System.Diagnostics;

namespace DragonFiesta.Zone.Data.KingdomQuest
{
    [GameServerModule(ServerType.Zone, GameInitalStage.KingdomQuest)]
    public class KingdomQuestProvider
    {
        protected static SecureCollection<KingdomQuestRew> KingdomQuestRewSC;

        [InitializerMethod]
        public static bool Initialize()
        {
            LoadKingdomQuestRew();
            return true;
        }

        public static void LoadKingdomQuestRew()
        {
            var watch = Stopwatch.StartNew();
            KingdomQuestRewSC = new SecureCollection<KingdomQuestRew>();

            var pResult = SHNManager.Load(SHNType.KingdomQuestRew);
            DataLog.WriteProgressBar(">> Load KingdomQuestRew");

            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new KingdomQuestRew(pResult, i);
                    KingdomQuestRewSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {KingdomQuestRewSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }
    }
}
