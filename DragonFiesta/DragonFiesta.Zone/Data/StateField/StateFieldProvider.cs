using DragonFiesta.Utils.IO.SHN;
using System.Diagnostics;

namespace DragonFiesta.Zone.Data.StateField
{
    [GameServerModule(ServerType.Zone, GameInitalStage.StateField)]
    public class StateFieldProvider
    {
        protected static SecureCollection<StateField> StateFieldSC;

        [InitializerMethod]
        public static bool Initialize()
        {
            LoadStateField();
            return true;
        }

        public static void LoadStateField()
        {
            var watch = Stopwatch.StartNew();
            StateFieldSC = new SecureCollection<StateField>();

            var pResult = SHNManager.Load(SHNType.StateField);
            DataLog.WriteProgressBar(">> Load StateField");

            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new StateField(pResult, i);
                    StateFieldSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {StateFieldSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }
    }
}
