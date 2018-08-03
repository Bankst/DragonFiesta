using DragonFiesta.Utils.IO.SHN;
using System.Diagnostics;

namespace DragonFiesta.World.Data.GuildAcademy
{
    [GameServerModule(ServerType.World, GameInitalStage.GuildAcademy)]
    public class GuildAcademyRankProvider
    {
        protected static SecureCollection<GuildAcademyRank> GuildAcademyRankSC;
        protected static SecureCollection<GuildAcademyLevelUp> GuildAcademyLevelUpSC;
        protected static SecureCollection<GuildAcademy> GuildAcademySC;

        [InitializerMethod]
        public static bool Initialize()
        {
            LoadGuildAcademyRank();
            LoadGuildAcademyLevelUp();
            LoadGuildAcademy();
            return true;
        }

        public static void LoadGuildAcademyRank()
        {
            var watch = Stopwatch.StartNew();
            GuildAcademyRankSC = new SecureCollection<GuildAcademyRank>();

            var pResult = SHNManager.Load(SHNType.GuildAcademyRank);
            DatabaseLog.WriteProgressBar(">> Load GuildAcademyRank");

            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new GuildAcademyRank(pResult, i);
                    GuildAcademyRankSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DatabaseLog.WriteProgressBar($">> Loaded {GuildAcademyRankSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }

        public static void LoadGuildAcademyLevelUp()
        {
            var watch = Stopwatch.StartNew();
            GuildAcademyLevelUpSC = new SecureCollection<GuildAcademyLevelUp>();

            var pResult = SHNManager.Load(SHNType.GuildAcademyLevelUp);
            DatabaseLog.WriteProgressBar(">> Load GuildAcademyLevelUp");

            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new GuildAcademyLevelUp(pResult, i);
                    GuildAcademyLevelUpSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DatabaseLog.WriteProgressBar($">> Loaded {GuildAcademyLevelUpSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }

        public static void LoadGuildAcademy()
        {
            var watch = Stopwatch.StartNew();
            GuildAcademySC = new SecureCollection<GuildAcademy>();

            var pResult = SHNManager.Load(SHNType.GuildAcademy);
            DatabaseLog.WriteProgressBar(">> Load GuildAcademy");

            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new GuildAcademy(pResult, i);
                    GuildAcademySC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DatabaseLog.WriteProgressBar($">> Loaded {GuildAcademySC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }
    }
}
