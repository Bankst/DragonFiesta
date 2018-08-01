using DragonFiesta.Utils.IO.SHN;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace DragonFiesta.Zone.Data.CollectCard
{
    [GameServerModule(ServerType.Zone, GameInitalStage.CollectCard)]
    public class CollectCardProvider
    {
        protected static ConcurrentDictionary<ushort, CollectCard> CollectCardByID;
        protected static SecureCollection<CollectCard> CollectCardSC;

        [InitializerMethod]
        public static bool Initialize()
        {
            LoadCollectCard();
            return true;
        }

        public static void LoadCollectCard()
        {
            var watch = Stopwatch.StartNew();
            CollectCardSC = new SecureCollection<CollectCard>();
            CollectCardByID = new ConcurrentDictionary<ushort, CollectCard>();

            var pResult = SHNManager.Load(SHNType.CollectCard);
            DatabaseLog.WriteProgressBar(">> Load CollectCard");
            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new CollectCard(pResult, i);

                    if (!CollectCardByID.TryAdd(info.CardID, info))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, $"Duplicate CollectCard ID found. ID: {info.CardID}");
                        continue;
                    }
                    CollectCardSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DatabaseLog.WriteProgressBar($">> Loaded {CollectCardSC.Count} rows in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }
    }
}
