using DragonFiesta.Providers.Maps;
using System;
using System.Collections.Concurrent;

namespace DragonFiesta.World.Data.Characters
{
    [GameServerModule(ServerType.World, GameInitalStage.CharacterData)]
    public class DefaultCharacterDataProvider
    {
        private static ConcurrentDictionary<byte, DefaultCharacterInfo> CharacterInfoByClassID;

        [InitializerMethod]
        public static bool OnStart()
        {
            try
            {
                InitialCharacterData();
                return true;
            }
            catch (Exception ex)
            {
                EngineLog.Write(EngineLogLevel.Startup, "Failed to Start DefaultCharacterDataProvider {0}", ex);
                return false;
            }
        }

        public static void InitialCharacterData()
        {
            LoadDefaultCharacterInfo();
            //LoadDefaultCharacterItem();
            //LoadDefaultCharacterItemsOptions();
            //LoadDefaultCharacterSkills();
        }

        public static void LoadDefaultCharacterSkills()
        {
            SQLResult pResult = DB.Select(DatabaseType.Data, "SELECT * FROM DefaultCharacterSkills");
            DatabaseLog.WriteProgressBar(">> Load DefaultCharacter Skills");
            int Count = 0;
            using (ProgressBar mBar = new ProgressBar(pResult.Count))
            {
                for (int i = 0; i < pResult.Count; i++)
                {
                    byte ClassID = pResult.Read<byte>(i, "Class");
                    ushort SkillID = pResult.Read<ushort>(i, "SkillID");
                    mBar.Step();
                    if (!GetDefaultCharacterByID(ClassID, out DefaultCharacterInfo mInfo))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Can not Find Class {0} with Default Skill {1}", ClassID, SkillID);
                        continue;
                    }

                    /*
                    if (SkillDataProvider.GetActiveSkillInfoByID(SkillID, out ActiveSkillInfo activeSkill))
                    {
                        mInfo.DefaultCharacterActiveSkills.Add(new DefaultCharacterActiveSkillInfo(activeSkill));
                        Count++;
                    }
                    else
                    {
                        if (!BuffDataProvider.GetAbStateInfoByID(SkillID, out AbStateInfo pInfo))
                        {
                            DatabaseLog.Write(DatabaseLogLevel.Warning, "Cant not found Default Skill {0} Classs {1}", SkillID, ClassID);
                            continue;
                        }
                        mInfo.DefaultCharacterPassiveSkills.Add(new DefaultCharacterPassiveSkillInfo(pInfo));
                        Count++;
                    }
                    */
                }
                DatabaseLog.WriteProgressBar(">> Loaded {0} DefaultCharacter Skills", Count);
            }
        }

        public static void LoadDefaultCharacterItem()
        {
            /*
            SQLResult pResult = DB.Select(DatabaseType.Data, "SELECT * FROM  DefaultCharacterItems");

            DatabaseLog.WriteConsole(">> Load DefaultCharacter Items");
            int count = 0;
            using (ProgressBar mBar = new ProgressBar(pResult.Count))
            {
                for (int i = 0; i < pResult.Count; i++)
                {
                    byte ClassID = pResult.Read<byte>(i, "Class");
                    ushort itemID = pResult.Read<ushort>(i, "ItemID");

                    DefaultCharacterInfo pInfo;
                    if (!GetDefaultCharacterByID(ClassID, out pInfo))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Cant find CharacterClass {0} for Item {1}", ClassID, itemID);
                        continue;
                    }

                    ItemInfo itemInfo;
                    if (!WorldItemDataProvider.GetItemInfoByID(itemID, out itemInfo))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Cant find ItemInfo for {0} Item in DefaultClass {1}", itemID, ClassID);
                        continue;
                    }*/
            //count++;
            /*
            if (!pInfo.DefaultCharacterItems.TryAdd(itemInfo.ID, new DefaultCharacterItem(itemInfo, pResult, i)))
            {
                DatabaseLog.Write(DatabaseLogLevel.Warning, "Dublicate DefaultCharacterItem {0} found!!", itemID);
            }

            mBar.step();
        }
        DatabaseLog.WriteConsole(">> Loaded {0} DefaultCharacter Items", count);
    }*/
        }

        public static void LoadDefaultCharacterInfo()
        {
            CharacterInfoByClassID = new ConcurrentDictionary<byte, DefaultCharacterInfo>();

            SQLResult pResult = DB.Select(DatabaseType.Data, "SELECT * FROM DefaultCharacterInfos");
            DatabaseLog.WriteProgressBar(">> Load DefaultCharacter Data");

            using (ProgressBar mBar = new ProgressBar(pResult.Count))
            {
                for (int i = 0; i < pResult.Count; i++)
                {
                    byte ClassID = pResult.Read<byte>(i, "Class");

                    ushort startID = pResult.Read<ushort>(i, "MapID");

                    if (!MapDataProvider.GetMapInfoByID(startID, out MapInfo startMap))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Cant find Start Mapinfo {0} for class {1}", startID, ClassID);
                        continue;
                    }

                    if (!CharacterInfoByClassID.TryAdd(ClassID, new DefaultCharacterInfo(startMap, pResult, i)))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Dublicate CharacterInfo Found {0}", ClassID);
                    }
                    mBar.Step();
                }
                DatabaseLog.WriteProgressBar(">> Loaded {0} DefaultCharacter Data", CharacterInfoByClassID.Count);
            }
        }

        public static void LoadDefaultCharacterItemsOptions()
        {
            SQLResult pResult = DB.Select(DatabaseType.Data, "SELECT * FROM DefaultCharacterItemOptions");

            DatabaseLog.WriteProgressBar(">> Load DefaultCharacterItemOptions");

            using (ProgressBar mBar = new ProgressBar(pResult.Count))
            {
                int mLoadet = 0;
                for (int i = 0; i < pResult.Count; i++)
                {
                    DefaultCharacterItemOptions mRow = new DefaultCharacterItemOptions(pResult, i);

                    mBar.Step();

                    if (!GetDefaultCharacterByID(mRow.Class, out DefaultCharacterInfo pInfo))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Cant find DefaultCharacterInfo {0} for DefaultCharacterItemOptions {1} ", mRow.Class, mRow.ItemID);
                        continue;
                    }

                    if (!pInfo.DefaultCharacterItems.TryGetValue(mRow.ItemID, out DefaultCharacterItem pItem))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Cant find ItemOptionsValues ItemID {0} : Class {1} Options {2}", mRow.ItemID, mRow.index);
                        continue;
                    }
                    /*
                    //Todo Better Error Message..
                    if (!pItem.Options.TryAdd((ItemOptionIndex)mRow.index, mRow.Value))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Dublicate DefaultCharacterItemOption found ItemID {0} Class {1} Index {2}", mRow.ItemID, mRow.Class, mRow.index);
                        continue;
                    }*/
                    mLoadet++;
                }
                DatabaseLog.WriteProgressBar(">> Loaded {0} DefaultCharacterItemOptions Data", mLoadet);
            }
        }

        public static bool GetDefaultCharacterByID(byte ID, out DefaultCharacterInfo pInfo)
        {
            return CharacterInfoByClassID.TryGetValue(ID, out pInfo);
        }
    }
}