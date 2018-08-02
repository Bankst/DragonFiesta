using DragonFiesta.Utils.IO.SHN;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace DragonFiesta.World.Data.AttendReward
{
    [GameServerModule(ServerType.World, GameInitalStage.AttendReward)]
    public class AttendRewardProvider
    {
        protected static ConcurrentDictionary<byte, AttendReward> AttendRewardByID;
        protected static SecureCollection<AttendReward> AttendRewardSC;
        protected static SecureCollection<AttendSchedule> AttendScheduleSC;


        [InitializerMethod]
        public static bool Initialize()
        {
            LoadAttendReward();
            LoadAttendSchedule();
            return true;
        }

        public static void LoadAttendReward()
        {
            var watch = Stopwatch.StartNew();
            AttendRewardSC = new SecureCollection<AttendReward>();
            AttendRewardByID = new ConcurrentDictionary<byte, AttendReward>();

            var pResult = SHNManager.Load(SHNType.AttendReward);
            DatabaseLog.WriteProgressBar(">> Load AttendReward");
            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new AttendReward(pResult, i);

                    if (!AttendRewardByID.TryAdd(info.AR_ID, info))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, $"Duplicate AR ID found. ID: {info.AR_ID}");
                        continue;
                    }
                    AttendRewardSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DatabaseLog.WriteProgressBar($">> Loaded {AttendRewardSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }

        public static void LoadAttendSchedule()
        {
            var watch = Stopwatch.StartNew();
            AttendScheduleSC = new SecureCollection<AttendSchedule>();

            var pResult = SHNManager.Load(SHNType.AttendSchedule);
            DatabaseLog.WriteProgressBar(">> Load AttendSchedule");

            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new AttendSchedule(pResult, i);
                    AttendScheduleSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DatabaseLog.WriteProgressBar($">> Loaded {AttendScheduleSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }
    }
}
