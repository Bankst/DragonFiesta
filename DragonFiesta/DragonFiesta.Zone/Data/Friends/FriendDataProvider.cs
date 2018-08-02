using System;
using System.Diagnostics;
using System.Linq;
using DragonFiesta.Database.SQL;

namespace DragonFiesta.Zone.Data.Friends
{
    [GameServerModule(ServerType.Zone, GameInitalStage.Friend)]
    public class FriendDataProvider
    {

        private static SecureCollection<FriendPointReward> FriendPointRewards { get; set; }

        [InitializerMethod]
        public static bool InitialFriendDataProvider()
        {
            LoadFriendPointsRewards();
            return true;
        }

        private static void LoadFriendPointsRewards()
        {
            var watch = Stopwatch.StartNew();
            FriendPointRewards = new SecureCollection<FriendPointReward>();

            SQLResult pResult = DB.Select(DatabaseType.Data, "SELECT * FROM FriendPointRewards");
            DatabaseLog.WriteProgressBar(">> Load FriendPointRewards");
            using (ProgressBar mBar = new ProgressBar(pResult.Count))
            {
                for (int i = 0; i < pResult.Count; i++)
                {

                    mBar.Step();

                    if (!FriendPointRewards.Add(new FriendPointReward(pResult, i)))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate Reward Found!");
                        continue;
                    }
                }
                watch.Stop();
                DatabaseLog.WriteProgressBar($">> Loaded {FriendPointRewards.Count} rows from database in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }

        public static bool GetReward(out FriendPointReward Reward)
        {
            //thanks for calculation baker :)

            var TotalWeight = FriendPointRewards.Sum(x => x.Rate);

            var RandomNum = new Random().Next(0, TotalWeight);

            foreach (var R in FriendPointRewards)
            {
                if (RandomNum < R.Rate)
                {
                    Reward = R;
                    return true;
                }
                RandomNum -= R.Rate;

            }

            Reward = null;
            return false;
        }
    }
}