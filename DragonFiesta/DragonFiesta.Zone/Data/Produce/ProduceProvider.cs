using DragonFiesta.Utils.IO.SHN;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace DragonFiesta.Zone.Data.Produce
{
    [GameServerModule(ServerType.Zone, GameInitalStage.Produce)]
    public class ProduceProvider
    {
        protected static ConcurrentDictionary<ushort, Produce> ProduceByID;
        protected static SecureCollection<Produce> ProduceSC;

        [InitializerMethod]
        public static bool Initialize()
        {
            LoadProduce();
            return true;
        }

        public static void LoadProduce()
        {
            var watch = Stopwatch.StartNew();
            ProduceSC = new SecureCollection<Produce>();
            ProduceByID = new ConcurrentDictionary<ushort, Produce>();

            var pResult = SHNManager.Load(SHNType.Produce);
            DataLog.WriteProgressBar(">> Load Produce");
            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new Produce(pResult, i);

                    if (!ProduceByID.TryAdd(info.ProductID, info))
                    {
                        DataLog.Write(DataLogLevel.Warning, $"Duplicate Produce ID found. ID: {info.ProductID}");
                        continue;
                    }
                    ProduceSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {ProduceSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }
    }
}
