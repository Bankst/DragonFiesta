using System.Collections.Concurrent;
using DragonFiesta.Database.SQL;

namespace DragonFiesta.Zone.Data.Skills
{
    [GameServerModule(ServerType.World, GameInitalStage.Skill)]
    public static class SkillDataProvider
    {
        private static ConcurrentDictionary<ushort, ActiveSkillInfo> ActiveSkillInfosByID;

        [InitializerMethod]
        public static bool Initialize()
        {
            //LoadActiveSkillInfos();
            return true;
        }
        /*
        private static void LoadActiveSkillInfos()
        {
            ActiveSkillInfosByID = new ConcurrentDictionary<ushort, ActiveSkillInfo>();

            SQLResult pResult = DB.Select(DatabaseType.Data, "SELECT * FROM ActiveSkills");

            DatabaseLog.WriteProgressBar(">> Load ActiveSkills");
            using (ProgressBar mBar = new ProgressBar(pResult.Count))
            {
                for (int i = 0; i < pResult.Count; i++)
                {
                    var info = new ActiveSkillInfo(pResult, i);

                    mBar.Step();
                    if (!ActiveSkillInfosByID.TryAdd(info.ID, info))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate active skill ID found: {0}", info.ID);
                        continue;
                    }
                }
                DatabaseLog.WriteProgressBar(">> Loaded {0} ActiveSkills", ActiveSkillInfosByID.Count);
            }
            DatabaseLog.WriteProgressBar(">> Finalize Load ActiveSkills");
            using (ProgressBar mBar = new ProgressBar(pResult.Count))
            {
                for (int i = 0; i < pResult.Count; i++)
                {
                    ushort SkillID = pResult.Read<ushort>(i, "ID");
                    if (ActiveSkillInfosByID.TryGetValue(SkillID, out ActiveSkillInfo pInfo))
                    {
                        if (!pInfo.FinalizeLoad(pResult, i))
                        {
                            ActiveSkillInfosByID.TryRemove(pInfo.ID, out ActiveSkillInfo p);
                        }
                    }
                    mBar.Step();
                }
                DatabaseLog.WriteProgressBar(">> Finalize Loaded {0} ActiveSkills", ActiveSkillInfosByID.Count);
            }
        }

        public static bool GetActiveSkillInfoByID(ushort ID, out ActiveSkillInfo ActiveSkillInfo)
        {
            return ActiveSkillInfosByID.TryGetValue(ID, out ActiveSkillInfo);
        }*/

        /*
        public static bool InsertSkillToDatabase(int Owner, ushort SkillID, bool IsPassive, out long ID, byte[] Upgrades = null)
        {
            ID = 0;
            try
            {
                ;
                using (var cmd = SQLCon.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "dbo.Skill_Insert";
                    cmd.Parameters.Add(new SqlParameter("@pOwnerID", Owner));
                    cmd.Parameters.Add(new SqlParameter("@pSkillID", (short)SkillID));
                    cmd.Parameters.Add(new SqlParameter("@pIsPassive", IsPassive));
                    cmd.Parameters.Add(new SqlParameter("@pUpgrades", Upgrades ?? new byte[4]));

                    var idParam = cmd.Parameters.Add(new SqlParameter("@pID", SqlDbType.BigInt)
                        {
                            Direction = ParameterDirection.Output,
                        });
                    var res = (int)cmd.ExecuteScalar();
                    switch (res)
                    {
                        case 0:
                            ID = (long)idParam.Value;
                            return true;

                        case -1:
                        case -2:
                        default:
                            EngineLog.Write(EngineLogLevel.Warnings, "SQL Error occured while inserting skill to database: {0}", (res == -1 ? "ID Overflow" : "Internal sql error"));
                            return false;
                    }
                }
            }
            catch (Exception ex)
            {
                EngineLog.Write(ex, "Error adding skill to database:");
                return false;
            }
        }*/
    }
}