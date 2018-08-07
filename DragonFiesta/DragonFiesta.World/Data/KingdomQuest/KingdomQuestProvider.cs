using DragonFiesta.Utils.IO.SHN;
using System.Diagnostics;

namespace DragonFiesta.World.Data.KingdomQuest
{
    [GameServerModule(ServerType.World, GameInitalStage.KingdomQuest)]
    public class KingdomQuestProvider
    {
        protected static SecureCollection<KingdomQuestMap> KingdomQuestMapSC;
        protected static SecureCollection<KQIsVote> KQIsVoteSC;
        protected static SecureCollection<KQTeam> KQTeamSC;
        protected static SecureCollection<KQVoteMajorityRate> KQVoteMajorityRateSC;
        protected static SecureCollection<KingdomQuest> KingdomQuestSC;

        [InitializerMethod]
        public static bool Initialize()
        {
            LoadKingdomQuestMap();
            LoadKQTeam();
            LoadKQIsVote();
            LoadKQVoteMajorityRate();
            LoadKingdomQuest();
            return true;
        }

        public static void LoadKingdomQuestMap()
        {
            var watch = Stopwatch.StartNew();
            KingdomQuestMapSC = new SecureCollection<KingdomQuestMap>();

            var pResult = SHNManager.Load(SHNType.KingdomQuestMap);
            DataLog.WriteProgressBar(">> Load KingdomQuestMap");

            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new KingdomQuestMap(pResult, i);
                    KingdomQuestMapSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {KingdomQuestMapSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }

        public static void LoadKQTeam()
        {
            var watch = Stopwatch.StartNew();
            KQTeamSC = new SecureCollection<KQTeam>();

            var pResult = SHNManager.Load(SHNType.KQTeam);
            DataLog.WriteProgressBar(">> Load KQTeam");

            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new KQTeam(pResult, i);
                    KQTeamSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {KQTeamSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }

        public static void LoadKQIsVote()
        {
            var watch = Stopwatch.StartNew();
            KQIsVoteSC = new SecureCollection<KQIsVote>();

            var pResult = SHNManager.Load(SHNType.KQIsVote);
            DataLog.WriteProgressBar(">> Load KQIsVote");

            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new KQIsVote(pResult, i);
                    KQIsVoteSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {KQIsVoteSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }

        public static void LoadKQVoteMajorityRate()
        {
            var watch = Stopwatch.StartNew();
            KQVoteMajorityRateSC = new SecureCollection<KQVoteMajorityRate>();

            var pResult = SHNManager.Load(SHNType.KQVoteMajorityRate);
            DataLog.WriteProgressBar(">> Load KQVoteMajorityRate");

            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new KQVoteMajorityRate(pResult, i);
                    KQVoteMajorityRateSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {KQVoteMajorityRateSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }

        public static void LoadKingdomQuest()
        {
            var watch = Stopwatch.StartNew();
            KingdomQuestSC = new SecureCollection<KingdomQuest>();

            var pResult = SHNManager.Load(SHNType.KingdomQuest);
            DataLog.WriteProgressBar(">> Load KingdomQuest");

            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new KingdomQuest(pResult, i);
                    KingdomQuestSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {KingdomQuestSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }
    }
}
