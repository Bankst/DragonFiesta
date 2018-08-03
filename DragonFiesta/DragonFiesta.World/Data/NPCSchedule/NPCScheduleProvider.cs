using DragonFiesta.Utils.IO.SHN;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace DragonFiesta.World.Data.NPCSchedule
{
    [GameServerModule(ServerType.World, GameInitalStage.NPCSchedule)]
    public class NPCScheduleProvider
    {
        protected static ConcurrentDictionary<string, NPCSchedule> NPCScheduleByInx;
        protected static SecureCollection<NPCSchedule> NPCScheduleSC;

        [InitializerMethod]
        public static bool Initialize()
        {
            LoadAttendReward();
            return true;
        }

        public static void LoadAttendReward()
        {
            var watch = Stopwatch.StartNew();
            NPCScheduleSC = new SecureCollection<NPCSchedule>();
            NPCScheduleByInx = new ConcurrentDictionary<string, NPCSchedule>();

            var pResult = SHNManager.Load(SHNType.NpcSchedule);
            DataLog.WriteProgressBar(">> Load NPCSchedule");
            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new NPCSchedule(pResult, i);

                    if (!NPCScheduleByInx.TryAdd(info.Mob_Inx, info))
                    {
                        DataLog.Write(DataLogLevel.Warning, $"Duplicate MobInx found. MobInx: {info.Mob_Inx}");
                        continue;
                    }
                    NPCScheduleSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {NPCScheduleSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }
    }
}
