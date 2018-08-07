using DragonFiesta.Utils.IO.SHN;
using System.Diagnostics;

namespace DragonFiesta.World.Data.MsgWorldManager
{
    [GameServerModule(ServerType.World, GameInitalStage.MsgWorldManager)]
    public class MsgWorldManagerProvider
    {
        protected static SecureCollection<MsgWorldManager> MsgWorldManagerSC;

        [InitializerMethod]
        public static bool Initialize()
        {
            LoadMsgWorldManager();
            return true;
        }

        public static void LoadMsgWorldManager()
        {
            var watch = Stopwatch.StartNew();
            MsgWorldManagerSC = new SecureCollection<MsgWorldManager>();

            var pResult = SHNManager.Load(SHNType.MsgWorldManager);
            DataLog.WriteProgressBar(">> Load MsgWorldManager");

            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new MsgWorldManager(pResult, i);
                    MsgWorldManagerSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {MsgWorldManagerSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }
    }
}
