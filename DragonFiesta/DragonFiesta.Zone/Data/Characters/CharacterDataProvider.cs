using DragonFiesta.Providers.Characters;
using DragonFiesta.Zone.Data.Stats;
using System.Collections.Concurrent;

namespace DragonFiesta.Zone.Data.Character
{
    [GameServerModule(ServerType.World, GameInitalStage.CharacterData)]
    [GameServerModule(ServerType.Zone,GameInitalStage.CharacterData)]
    public class CharacterDataProvider : CharacterDataProviderBase
    {
        private static ConcurrentDictionary<ClassId, ConcurrentDictionary<byte, CharacterLevelParameter>> CharacterStatParameters;

        private static ConcurrentDictionary<byte, FreeStatData> CharacterFreeStatsByLevel;

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
            CharacterFreeStatsByLevel = new ConcurrentDictionary<byte, FreeStatData>();

            SQLResult Result = DB.Select(DatabaseType.Data, "SELECT * FROM FreeStates");

            DatabaseLog.WriteProgressBar(">> Load FreeStats");

            using (ProgressBar mBar = new ProgressBar(Result.Count))
            {
                for (int i = 0; i < Result.Count; i++)
                {
                    FreeStatData Data = new FreeStatData(Result, i);

                    if (!CharacterFreeStatsByLevel.TryAdd(Data.Level, Data))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate FreeStatdata found Level {0}", Data.Level);
                    }
                    mBar.Step();
                }
                DatabaseLog.WriteProgressBar(">> Loaded {0} FreeStats", CharacterFreeStatsByLevel.Count);
            }
        }

        private static void LoadCharacterParameters()
        {
            CharacterStatParameters = new ConcurrentDictionary<ClassId, ConcurrentDictionary<byte, CharacterLevelParameter>>();

            DatabaseLog.WriteProgressBar(">> Load ChaacterParameters");

            SQLResult Result = DB.Select(DatabaseType.Data, "SELECT * FROM CharacterParameters");

            int StatsCounter = 0;

            using (ProgressBar mBar = new ProgressBar(Result.Count))
            {
                for (int i = 0; i < Result.Count; i++)
                {
                    byte Class = Result.Read<byte>(i, "Class");

                    mBar.Step();

                    if (!CharacterClass.IsValidClass(Class))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Invalid ClassId {0} Found!! in CharacterParameters", Class);
                        continue;
                    }


                    CharacterLevelParameter LevelParams = new CharacterLevelParameter(Result, i);

                    //Create new CLass :P
                    if (!CharacterStatParameters.TryGetValue((ClassId)Class, out ConcurrentDictionary<byte, CharacterLevelParameter> LevelParms))
                        CharacterStatParameters.TryAdd((ClassId)Class, new ConcurrentDictionary<byte, CharacterLevelParameter>());

                    if(!GetFreeStat(LevelParams.Level,out FreeStatData Data))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "No FreeStat Data for Level {0} found!", LevelParams.Level);
                        continue;
                    }

                    LevelParams.FreeStats = Data;

                    if (!CharacterStatParameters[(ClassId)Class].TryAdd(LevelParams.Level, LevelParams))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate CharacterParameters Found!! Class {0} Level {1}", Class, LevelParams.Level);
                        continue;
                    }
                    StatsCounter++;

                }

                DatabaseLog.WriteProgressBar(">> Loaded {0} CharacterParameters", StatsCounter);
            }

        }



        public static bool GetFreeStat(byte Level, out FreeStatData Data) => CharacterFreeStatsByLevel.TryGetValue(Level, out Data);
        public static bool GetLevelParameters(ClassId Class, byte Level, out CharacterLevelParameter Params)
        {
            Params = null;
            if (!CharacterStatParameters.TryGetValue(Class, out ConcurrentDictionary<byte, CharacterLevelParameter> ClassParams))
                return false;


            if (!ClassParams.TryGetValue(Level, out Params))
                return false;


            return true;
        }
    }
}
