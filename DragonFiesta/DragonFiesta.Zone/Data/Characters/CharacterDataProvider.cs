using DragonFiesta.Providers.Characters;
using DragonFiesta.Zone.Data.Stats;
using System.Collections.Concurrent;
using DragonFiesta.Database.SQL;
using DragonFiesta.Zone.Data.Characters;
using System.Diagnostics;

namespace DragonFiesta.Zone.Data.Character
{
    [GameServerModule(ServerType.World, GameInitalStage.CharacterData)]
    [GameServerModule(ServerType.Zone,GameInitalStage.CharacterData)]
    public class CharacterDataProvider : CharacterDataProviderBase
    {
        private static ConcurrentDictionary<ClassId, ConcurrentDictionary<byte, CharacterLevelParameter>> _characterStatParameters;
        private static ConcurrentDictionary<byte, FreeStatData> _characterFreeStatsByLevel;

        [InitializerMethod]
        public static bool InitialCharacterData()
        {
            LoadCharacterFreeStats();
            LoadCharacterParameters();
            LoadExpTable();
            return true;
        }

        private static void LoadCharacterFreeStats()
        {
            var watch = Stopwatch.StartNew();
            _characterFreeStatsByLevel = new ConcurrentDictionary<byte, FreeStatData>();

            var result = DB.Select(DatabaseType.Data, "SELECT * FROM FreeStates");

            DatabaseLog.WriteProgressBar(">> Load FreeStats");

            using (var mBar = new ProgressBar(result.Count))
            {
                for (var i = 0; i < result.Count; i++)
                {
                    var data = new FreeStatData(result, i);

                    if (!_characterFreeStatsByLevel.TryAdd(data.Level, data))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate FreeStatdata found Level {0}", data.Level);
                    }
                    mBar.Step();
                }
                watch.Stop();
                DatabaseLog.WriteProgressBar($">> Loaded {_characterFreeStatsByLevel.Count} rows from database in in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }

        private static void LoadCharacterParameters()
        {
            var watch = Stopwatch.StartNew();
            _characterStatParameters = new ConcurrentDictionary<ClassId, ConcurrentDictionary<byte, CharacterLevelParameter>>();

            DatabaseLog.WriteProgressBar(">> Load CharacterParameters");

            var result = DB.Select(DatabaseType.Data, "SELECT * FROM CharacterParameters");

            var statsCounter = 0;

            using (var mBar = new ProgressBar(result.Count))
            {
                for (var i = 0; i < result.Count; i++)
                {
                    var Class = result.Read<byte>(i, "Class");

                    mBar.Step();

                    if (!CharacterClass.IsValidClass(Class))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Invalid ClassId {0} Found!! in CharacterParameters", Class);
                        continue;
                    }


                    var levelParams = new CharacterLevelParameter(result, i);

                    //Create new CLass :P
                    if (!_characterStatParameters.TryGetValue((ClassId)Class, out var levelParms))
                        _characterStatParameters.TryAdd((ClassId)Class, new ConcurrentDictionary<byte, CharacterLevelParameter>());

                    if(!GetFreeStat(levelParams.Level,out var data))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "No FreeStat Data for Level {0} found!", levelParams.Level);
                        continue;
                    }

                    levelParams.FreeStats = data;

                    if (!_characterStatParameters[(ClassId)Class].TryAdd(levelParams.Level, levelParams))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate CharacterParameters Found!! Class {0} Level {1}", Class, levelParams.Level);
                        continue;
                    }
                    statsCounter++;

                }
                watch.Stop();
                DatabaseLog.WriteProgressBar($">> Loaded {statsCounter} rows from database in in {(double)watch.ElapsedMilliseconds / 1000}s");
            }

        }

        public static bool GetFreeStat(byte level, out FreeStatData data) => _characterFreeStatsByLevel.TryGetValue(level, out data);
        public static bool GetLevelParameters(ClassId Class, byte level, out CharacterLevelParameter Params)
        {
            Params = null;
            return _characterStatParameters.TryGetValue(Class, out var classParams) && classParams.TryGetValue(level, out Params);
        }
    }
}
