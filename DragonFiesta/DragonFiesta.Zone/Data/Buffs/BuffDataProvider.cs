using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using DragonFiesta.Database.SQL;

namespace DragonFiesta.Zone.Data.Buffs
{
    [GameServerModule(ServerType.Zone, GameInitalStage.Buff)]
    public class BuffDataProvider
    {
        public const ushort RestEXP_ID = 8817;
        public const uint RestEXP_Strength = 0;
        public static AbStateInfo RestEXP { get; private set; }
        private static ConcurrentDictionary<ushort, SubAbStateInfo> SubAbStatesDataByID;
        private static ConcurrentDictionary<ushort, List<SubAbStateInfo>> SubAbStatesByID;
        private static ConcurrentDictionary<ushort, AbStateInfo> AbStatesByID;
        private static ConcurrentDictionary<uint, AbStateInfo> AbStatesByAbStateIndex;
        private static SecureCollection<ushort> DeactivateAbstates;

        [InitializerMethod]
        public static bool Initialize()
        {
            LoadSubAbStates();
            LoadAbstateInfos();
            LoadAbStates();
            LoadDeactivateAbstates();
            return true;
        }


        public static void LoadDeactivateAbstates()
        {
            DeactivateAbstates = new SecureCollection<ushort>();

            SQLResult pResult = DB.Select(DatabaseType.Data, "SELECT * FROM DeactivateAbstate");

            using (ProgressBar mBar = new ProgressBar(pResult.Count))
            {
                for (int i = 0; i < pResult.Count; i++)
                {
                    DeactivateAbstates.Add(pResult.Read<ushort>(i, "AbstateID"));
                }
            }
        }


        private static void LoadSubAbStates()
        {
            SubAbStatesDataByID = new ConcurrentDictionary<ushort, SubAbStateInfo>();
            SubAbStatesByID = new ConcurrentDictionary<ushort, List<SubAbStateInfo>>();
            SQLResult pResult = DB.Select(DatabaseType.Data, "SELECT * FROM SubAbStateInfo");
            DatabaseLog.WriteProgressBar(">> Load SubAbState Infos");
            using (ProgressBar mBar = new ProgressBar(pResult.Count))
            {
                for (int i = 0; i < pResult.Count; i++)
                {
                    var info = new SubAbStateInfo(pResult, i);

                    mBar.Step();
                    if (!SubAbStatesDataByID.TryAdd(info.ID, info))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate sub abstate ID found: {0}", info.ID);

                        continue;
                    }

                    if (!SubAbStatesByID.TryGetValue(info.ID, out List<SubAbStateInfo> list))
                    {
                        list = new List<SubAbStateInfo>();
                        SubAbStatesByID.TryAdd(info.ID, list);
                    }

                    list.Add(info);
                }
                DatabaseLog.WriteProgressBar(">> Loaded {0} SubAbState Infos", SubAbStatesByID.Count);
            }
        }

        private static void LoadAbstateInfos()
        {
            AbStatesByID = new ConcurrentDictionary<ushort, AbStateInfo>();
            AbStatesByAbStateIndex = new ConcurrentDictionary<uint, AbStateInfo>();
            SQLResult pResult = DB.Select(DatabaseType.Data, "SELECT * FROM AbStateInfo");
            DatabaseLog.WriteProgressBar(">> Load Abstate Infos");
            using (ProgressBar mBar = new ProgressBar(pResult.Count))
            {
                for (int ri = 0; ri < pResult.Count; ri++)
                {
                    var info = new AbStateInfo(pResult, ri);
                    mBar.Step();
                    if (!AbStatesByID.TryAdd(info.ID, info))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate abstate ID found: {0}", info.ID);

                        continue;
                    }

                    if (!AbStatesByAbStateIndex.TryAdd(info.AbStateIndex, info))
                    {
                        AbStatesByID.TryRemove(info.ID, out info);

                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate abstate index (AbStataIndex) found: {0}", info.AbStateIndex);

                        continue;
                    }
                }
                DatabaseLog.WriteProgressBar(">> Loaded {0} Abstate Infos", AbStatesByID.Count);
            }
        }

        private static void LoadAbStates()
        {
            SQLResult pPartyR = DB.Select(DatabaseType.Data, "SELECT * FROM AbStatePartyInfo");
            DatabaseLog.WriteProgressBar(">> Load AbStateParty Infos");
            int count = 0;
            using (ProgressBar mBar = new ProgressBar(pPartyR.Count))
            {
                for (int ri = 0; ri < pPartyR.Count; ri++)
                {
                    if (!AbStatesByID.TryGetValue(pPartyR.Read<ushort>(ri, "ID"), out AbStateInfo info))
                        continue;

                    //load partystates
                    for (int i = 0; i < 5; i++)
                    {
                        ushort partyIndex = pPartyR.Read<ushort>(ri, "PartyState" + i);
                        if (partyIndex == 0) continue;

                        if (!AbStatesByID.TryGetValue(partyIndex, out AbStateInfo partyState))
                        {
                            DatabaseLog.Write(DatabaseLogLevel.Warning, "Can't find party state '{0}' for abstate  (ID: {1})", partyIndex, info.ID);

                            continue;
                        }

                        info.PartyStates.Add(partyState);
                    }

                    //get subabstate
                    ushort subIndex = pPartyR.Read<ushort>(ri, "SubAbState");
                    if (subIndex == 0)
                    {
                        if (!SubAbStatesByID.TryGetValue(subIndex, out List<SubAbStateInfo> subStates))
                        {
                            DatabaseLog.Write(DatabaseLogLevel.Warning, "Can't find sub abstate '{0}' for abstate (ID: {1})", subIndex, info.ID);
                        }
                        else
                        {
                            for (int i = 0; i < subStates.Count; i++)
                            {
                                info.SubAbStates.TryAdd(subStates[i].Strength, subStates[i]);
                            }
                        }
                    }

                    //get main abstate
                    ushort mainAbStateIndex = pPartyR.Read<ushort>(ri, "MainStateInx");

                    if (mainAbStateIndex != 0)
                    {
                        if (!AbStatesByID.TryGetValue(mainAbStateIndex, out AbStateInfo mainAbState))
                        {
                            DatabaseLog.Write(DatabaseLogLevel.Warning, "Can't find main abstate '{0}' for abstate (ID: {2})", mainAbStateIndex, info.ID);

                            continue;
                        }
                        info.MainAbState = mainAbState;
                    }
                    count++;
                    mBar.Step();
                }
                DatabaseLog.WriteProgressBar(">> Finalize Loaded {0} AbStateInfos", count);
            }
            //get rest exp buff
            if (!GetAbStateInfoByID(RestEXP_ID, out AbStateInfo abState))
                throw new InvalidOperationException(String.Format("Can't find 'Rest EXP' buff (ID: {0}).", RestEXP_ID));
            RestEXP = abState;
            abState.SubAbStates.TryAdd(0, new SubAbStateInfo(ushort.MaxValue, 0, 0, 0, TimeSpan.Zero));
        }

        public static bool GetAbStateInfoByID(ushort ID, out AbStateInfo AbStateInfo)
        {
            return AbStatesByID.TryGetValue(ID, out AbStateInfo);
        }

        public static bool GetAbStateInfoByAbStateIndex(uint Index, out AbStateInfo AbStateInfo)
        {
            return AbStatesByAbStateIndex.TryGetValue(Index, out AbStateInfo);
        }
    }
}