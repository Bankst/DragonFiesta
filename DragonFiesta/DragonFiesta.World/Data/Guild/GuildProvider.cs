using DragonFiesta.Utils.IO.SHN;
using System.Diagnostics;

namespace DragonFiesta.World.Data.Guild
{
    [GameServerModule(ServerType.World, GameInitalStage.Guild)]
    public class GuildProvider
    {
        protected static SecureCollection<GuildLevelScoreData> GuildLevelScoreDataSC;
        protected static SecureCollection<GuildGradeScoreData> GuildGradeScoreDataSC;
        protected static SecureCollection<GuildGradeData> GuildGradeDataSC;

        [InitializerMethod]
        public static bool Initialize()
        {
            LoadGuildLevelScoreData();
            LoadGuildGradeScoreData();
            LoadGuildGradeData();
            return true;
        }

        public static void LoadGuildLevelScoreData()
        {
            var watch = Stopwatch.StartNew();
            GuildLevelScoreDataSC = new SecureCollection<GuildLevelScoreData>();

            var pResult = SHNManager.Load(SHNType.GuildLevelScoreData);
            DataLog.WriteProgressBar(">> Load GuildLevelScoreData");

            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new GuildLevelScoreData(pResult, i);
                    GuildLevelScoreDataSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {GuildLevelScoreDataSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }

        public static void LoadGuildGradeScoreData()
        {
            var watch = Stopwatch.StartNew();
            GuildGradeScoreDataSC = new SecureCollection<GuildGradeScoreData>();

            var pResult = SHNManager.Load(SHNType.GuildGradeScoreData);
            DataLog.WriteProgressBar(">> Load GuildGradeScoreData");

            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new GuildGradeScoreData(pResult, i);
                    GuildGradeScoreDataSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {GuildGradeScoreDataSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }

        public static void LoadGuildGradeData()
        {
            var watch = Stopwatch.StartNew();
            GuildGradeDataSC = new SecureCollection<GuildGradeData>();

            var pResult = SHNManager.Load(SHNType.GuildGradeData);
            DataLog.WriteProgressBar(">> Load GuildGradeData");

            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new GuildGradeData(pResult, i);
                    GuildGradeDataSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {GuildGradeDataSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }
    }
}
