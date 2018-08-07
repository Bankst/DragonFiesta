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
        protected static SecureCollection<CollectCardDropRate> CollectCardDropRateSC;
        protected static SecureCollection<CollectCardMobGroup> CollectCardMobGroupSC;
        protected static SecureCollection<CollectCardReward> CollectCardRewardSC;
        protected static SecureCollection<CollectCardStarRate> CollectCardStarRateSC;
        protected static SecureCollection<CollectCardTitle> CollectCardTitleSC;

        [InitializerMethod]
        public static bool Initialize()
        {
            LoadCollectCard();
            LoadCollectCardDropRate();
            LoadCollectCardMobGroup();
            LoadCollectCardReward();
            LoadCollectStarRate();
            LoadCollectTitle();
            return true;
        }

        public static void LoadCollectCard()
        {
            var watch = Stopwatch.StartNew();
            CollectCardSC = new SecureCollection<CollectCard>();
            CollectCardByID = new ConcurrentDictionary<ushort, CollectCard>();

            var pResult = SHNManager.Load(SHNType.CollectCard);
            DataLog.WriteProgressBar(">> Load CollectCard");
            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new CollectCard(pResult, i);

                    if (!CollectCardByID.TryAdd(info.CardID, info))
                    {
                        DataLog.Write(DataLogLevel.Warning, $"Duplicate CollectCard ID found. ID: {info.CardID}");
                        continue;
                    }
                    CollectCardSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {CollectCardSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }

        public static void LoadCollectCardDropRate()
        {
            var watch = Stopwatch.StartNew();
            CollectCardDropRateSC = new SecureCollection<CollectCardDropRate>();

            var pResult = SHNManager.Load(SHNType.CollectCardDropRate);
            DataLog.WriteProgressBar(">> Load CollectCardDropRate");

            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new CollectCardDropRate(pResult, i);
                    CollectCardDropRateSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {CollectCardDropRateSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }

        public static void LoadCollectCardMobGroup()
        {
            var watch = Stopwatch.StartNew();
            CollectCardMobGroupSC = new SecureCollection<CollectCardMobGroup>();

            var pResult = SHNManager.Load(SHNType.CollectCardMobGroup);
            DataLog.WriteProgressBar(">> Load CollectCardMobGroup");

            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new CollectCardMobGroup(pResult, i);
                    CollectCardMobGroupSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {CollectCardMobGroupSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }

        public static void LoadCollectCardReward()
        {
            var watch = Stopwatch.StartNew();
            CollectCardRewardSC = new SecureCollection<CollectCardReward>();

            var pResult = SHNManager.Load(SHNType.CollectCardReward);
            DataLog.WriteProgressBar(">> Load CollectCardReward");

            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new CollectCardReward(pResult, i);
                    CollectCardRewardSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {CollectCardRewardSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }

        public static void LoadCollectStarRate()
        {
            var watch = Stopwatch.StartNew();
            CollectCardStarRateSC = new SecureCollection<CollectCardStarRate>();

            var pResult = SHNManager.Load(SHNType.CollectCardStarRate);
            DataLog.WriteProgressBar(">> Load CollectCardStarRate");

            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new CollectCardStarRate(pResult, i);
                    CollectCardStarRateSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {CollectCardStarRateSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }

        public static void LoadCollectTitle()
        {
            var watch = Stopwatch.StartNew();
            CollectCardTitleSC = new SecureCollection<CollectCardTitle>();

            var pResult = SHNManager.Load(SHNType.CollectCardTitle);
            DataLog.WriteProgressBar(">> Load CollectCardTitle");

            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = new CollectCardTitle(pResult, i);
                    CollectCardTitleSC.Add(info);
                    mBar.Step();
                }
                watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {CollectCardTitleSC.Count} rows from SHN in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }
    }
}
