using DragonFiesta.Utils.IO.SHN;
using System.Diagnostics;

namespace DragonFiesta.World.Data.GuildTournament
{
    [GameServerModule(ServerType.World, GameInitalStage.GuildTournament)]
    public class GuildTournamentProvider
    {
        protected static SecureCollection<GuildTournamentRequire> GuildTournamentRequireSC;
        protected static SecureCollection<GuildTournamentReward> GuildTournamentRewardSC;
        protected static SecureCollection<GuildTournament> GuildTournamentSC;

        [InitializerMethod]
        public static bool Initialize()
        {
            LoadGuildTournamentRequire();
            LoadGuildTournamentReward();
            LoadGuildTournament();
            return true;
        }

        public static void LoadGuildTournamentRequire()
        {
            var watch = Stopwatch.StartNew();
            GuildTournamentRequireSC = new SecureCollection<GuildTournamentRequire>();

            var pResult = SHNManager.Load(SHNType.GuildTournamentRequire);
            DataLog.WriteProgressBar(">> Load GuildTournamentRequire");

            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new GuildTournamentRequire(pResult, i);
                    GuildTournamentRequireSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {GuildTournamentRequireSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }

        public static void LoadGuildTournamentReward()
        {
            var watch = Stopwatch.StartNew();
            GuildTournamentRewardSC = new SecureCollection<GuildTournamentReward>();

            var pResult = SHNManager.Load(SHNType.GuildTournamentReward);
            DataLog.WriteProgressBar(">> Load GuildTournamentReward");

            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new GuildTournamentReward(pResult, i);
                    GuildTournamentRewardSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {GuildTournamentRewardSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }

        public static void LoadGuildTournament()
        {
            var watch = Stopwatch.StartNew();
            GuildTournamentSC = new SecureCollection<GuildTournament>();

            var pResult = SHNManager.Load(SHNType.GuildTournament);
            DataLog.WriteProgressBar(">> Load GuildTournament");

            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new GuildTournament(pResult, i);
                    GuildTournamentSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {GuildTournamentSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }
    }
}
