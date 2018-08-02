using DragonFiesta.Utils.Config;
using System.Collections.Concurrent;
using DragonFiesta.Database.SQL;
using System.Diagnostics;

namespace DragonFiesta.Providers.Characters
{
    public class CharacterDataProviderBase
    {
        private static ConcurrentDictionary<byte, ulong> _expTable;
		
        protected static void LoadExpTable()
        {
            var watch = Stopwatch.StartNew();
            _expTable = new ConcurrentDictionary<byte, ulong>();

            DatabaseLog.WriteProgressBar(">> Load ExpTable");

            var pResult = DB.Select(DatabaseType.Data, "SELECT * FROM ExpTable");

            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    var level = pResult.Read<byte>(i, "Level");
                    if (!_expTable.TryAdd(level, pResult.Read<ulong>(i, "NextExp")))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate Exp for Level {0} Found!!", level);
                    }
                    mBar.Step();
                }
                watch.Stop();
                DatabaseLog.WriteProgressBar($">> Loaded {_expTable.Count} Exps from Database in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }
		
        public static ulong GetEXPForNextLevel(byte currentLevel)
        {
            var nextLevel = (byte)(currentLevel + 1);

	        if (nextLevel >= GameConfiguration.Instance.LimitSetting.LevelLimit
                || !_expTable.TryGetValue(nextLevel, out var exp))
            {
                exp = 0;
            }
            return exp;
        }
	}
}
