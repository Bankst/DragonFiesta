using DragonFiesta.Providers.Characters;
using DragonFiesta.Utils.Config;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonFiesta.Providers.Characters
{
    public class CharacterDataProviderBase
    {
        private static ConcurrentDictionary<byte, ulong> ExpTable;

 

        protected static void LoadExpTable()
        {
            ExpTable = new ConcurrentDictionary<byte, ulong>();

            DatabaseLog.WriteProgressBar(">> Load ExpTable");

            SQLResult Result = DB.Select(DatabaseType.Data, "SELECT * FROM ExpTable");

            using (ProgressBar mBar = new ProgressBar(Result.Count))
            {
                for (int i = 0; i < Result.Count; i++)
                {
                    byte level = Result.Read<byte>(i, "Level");
                    if (!ExpTable.TryAdd(level, Result.Read<ulong>(i, "NextExp")))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Dublicate Exp for Level {0} Found!!", level);
                    }
                    mBar.Step();
                }
                DatabaseLog.WriteProgressBar(">> Loaded {0} Exps", ExpTable.Count);
            }
        }

     
        public static ulong GetEXPForNextLevel(byte CurrentLevel)
        {
            var nextLevel = (byte)(CurrentLevel + 1);

            ulong exp;
            if (nextLevel >= GameConfiguration.Instance.LimitSetting.LevelLimit
                || !ExpTable.TryGetValue(nextLevel, out exp))
            {
                exp = 0;
            }
            return exp;
        }

    }
}
