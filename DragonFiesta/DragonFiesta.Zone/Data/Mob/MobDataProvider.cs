using DragonFiesta.Providers.Maps;
using DragonFiesta.Zone.Data.WayPoints;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace DragonFiesta.Zone.Data.Mob
{
    [GameServerModule(ServerType.Zone, GameInitalStage.Mob)]
    public class MobDataProvider
    {
        public const ushort MobInfo_Summon_ID = 6504;
        public static MobInfo MobInfo_Summon { get; private set; }
        private static ConcurrentDictionary<ushort, MobInfo> MobInfosByID;
        private static ConcurrentDictionary<string, MobInfo> MobInfosByIndex;

        private static ConcurrentDictionary<int, MobGroupInfo> MobGroupInfosById { get; set; }

        //Mapid GroupId MobGroup
        private static ConcurrentDictionary<ushort, ConcurrentDictionary<int, MobGroupInfo>> MobGroupInfosByMapId { get; set; }

        private static ConcurrentDictionary<MobChatType, ConcurrentDictionary<ushort, MobChatInfo>> MobChatByType { get; set; }
        private static ConcurrentDictionary<int, WayPointInfo> WayPointsById { get; set; }

        [InitializerMethod]
        public static bool InitalMobData()
        {
            LoadMobInfos();
            LoadWayPoints();
            LoadMobGroups();
            LoadMobChats();
            return true;
        }

        public static void LoadMobChats()
        {
            MobChatByType = new ConcurrentDictionary<MobChatType, ConcurrentDictionary<ushort, MobChatInfo>>();

            using (SQLResult pResult = DB.Select(DatabaseType.Data, "SELECT * FROM MobChat"))
            {
                for (int i = 0; i < pResult.Count; i++)
                {
                    ushort MobId = pResult.Read<ushort>(i, "MobId");

                    MobChatType Type = (MobChatType)pResult.Read<byte>(i, "ChatType");

                    if (MobChatByType.TryGetValue(Type, out ConcurrentDictionary<ushort, MobChatInfo> Value))
                        MobChatByType.TryAdd(Type, new ConcurrentDictionary<ushort, MobChatInfo>());

                    if (MobChatByType[Type].TryGetValue(MobId, out MobChatInfo Info))
                    {
                        Info.MobChatList.Add(new MobChatMessageInfo(pResult, i));
                    }
                    else
                    {
                        var chatInfo = new MobChatInfo(pResult, i);
                        chatInfo.MobChatList.Add(new MobChatMessageInfo(pResult, i));
                        MobChatByType[Type].TryAdd(MobId, chatInfo);
                    }
                }
            }
        }

        public static void LoadMobGroups()
        {
            //SELECT * FROM Groups
            //Find ALL Members in GroupMember

            SQLResult AllMemberResult = DB.Select(DatabaseType.Data, "SELECT * FROM MobGroup_Members");
            var MemberView = AllMemberResult.DefaultView;
            MemberView.Sort = "GroupId";

            MobGroupInfosById = new ConcurrentDictionary<int, MobGroupInfo>();
            MobGroupInfosByMapId = new ConcurrentDictionary<ushort, ConcurrentDictionary<int, MobGroupInfo>>();

            using (SQLResult pResult = DB.Select(DatabaseType.Data, "SELECT * FROM MobGroups"))
            {
                for (int i = 0; i < pResult.Count; i++)
                {
                    MobGroupInfo Info = new MobGroupInfo(pResult, i);

                    if (!MapDataProvider.GetMapInfoByID(Info.MapId, out MapInfo MapInfo))//Check is Sppawnned map exis...
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Can't Find MapInfo for Group {0}", Info.GroupId);
                        continue;
                    }

                    if (!MobGroupInfosByMapId.ContainsKey(MapInfo.ID))
                        MobGroupInfosByMapId.TryAdd(MapInfo.ID, new ConcurrentDictionary<int, MobGroupInfo>());

                    if (MobGroupInfosByMapId[MapInfo.ID].ContainsKey(Info.GroupId) || MobGroupInfosById.ContainsKey(Info.GroupId))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate GroupId {0} Found!!", Info.GroupId);
                        continue;
                    }

                    var MemberResults = MemberView.FindRows(Info.GroupId);

                    if (MemberResults.Count() <= 0)
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "No Members For Group {0} Found....");
                        continue;
                    }

                    foreach (var MemberRow in MemberResults)
                    {
                        ushort MobId = Convert.ToUInt16(MemberRow.Row["MobId"]);
                        if (!GetMobInfoByID(MobId, out MobInfo MobInfo))
                        {
                            DatabaseLog.Write(DatabaseLogLevel.Warning, "Can't find MobId {0} for Group {1}", MobId, Info.GroupId);
                            continue;
                        }

                        MobGroupMemberInfo MemberInfo = new MobGroupMemberInfo(MobInfo, MemberRow.Row);

                        if (!Info.MemberInfo.TryAdd(MemberInfo.MobInfo.ID, MemberInfo))
                        {
                            DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate GroupMember Found!! MobId {0} ", MemberInfo.MobInfo.ID);
                            continue;
                        }
                    }
                    //Add Group To Data..
                    if (!MobGroupInfosByMapId[MapInfo.ID].TryAdd(Info.GroupId, Info) || !MobGroupInfosById.TryAdd(Info.GroupId, Info))
                    {
                        //TOod
                        continue;
                    }
                }
            }
        }

        private static void LoadWayPoints()
        {
            WayPointsById = new ConcurrentDictionary<int, WayPointInfo>();

            using (SQLResult pResult = DB.Select(DatabaseType.Data, "SELECT * FROM WayPoints"))
            {
                for (int i = 0; i < pResult.Count; i++)
                {
                    int WayPointId = pResult.Read<int>(i, "Id");
                    if (GetWayPointById(WayPointId, out WayPointInfo Point))
                    {
                        if (!Point.AddPosition(pResult, i))
                        {
                            DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate Waypoint Index found ID {0}", WayPointId);
                            continue;
                        }
                    }
                    else
                    {
                        var poin = new WayPointInfo(pResult, i);
                        WayPointsById.TryAdd(poin.Id, poin);
                        poin.AddPosition(pResult, i);
                    }
                }
            }
        }

        private static void LoadMobInfos()
        {
            MobInfosByID = new ConcurrentDictionary<ushort, MobInfo>();
            MobInfosByIndex = new ConcurrentDictionary<string, MobInfo>();
            //mobinfoserver
            SQLResult serverInfo = DB.Select(DatabaseType.Data, "SELECT * FROM MobInfoServer");
            var serverView = serverInfo.DefaultView;
            serverView.Sort = "ID";

            using (SQLResult pResult = DB.Select(DatabaseType.Data, "SELECT * FROM MobInfo"))
            {
                DatabaseLog.WriteProgressBar(">> Load MobInfos");

                using (ProgressBar mBar = new ProgressBar(pResult.Count))
                {
                    for (int i = 0; i < pResult.Count; i++)
                    {
                        ushort mobID = pResult.Read<ushort>(i, "ID");
                        //find server info
                        var serverResult = serverView.FindRows(mobID);
                        if (serverResult.Length < 1)
                        {
                            DatabaseLog.Write(DatabaseLogLevel.Warning, "Can't find server info for mob with  (ID: {1})", mobID);
                            mBar.Step();
                            continue;
                        }
                        var mobInfo = new MobInfo(pResult, i, serverResult[0].Row);
                        if (!MobInfosByID.TryAdd(mobInfo.ID, mobInfo))
                        {
                            DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate mob ID found: {0}", mobInfo.ID);
                            mBar.Step();
                            continue;
                        }
                        if (!MobInfosByIndex.TryAdd(mobInfo.Index, mobInfo))
                        {
                            MobInfosByID.TryRemove(mobInfo.ID, out mobInfo);
                            DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate mob index found: {0}", mobInfo.Index);
                            mBar.Step();
                            continue;
                        }
                        mBar.Step();
                    }

                    DatabaseLog.WriteProgressBar(">> Loaded {0} MobInfos", MobInfosByID.Count);
                }
            }
            //clean up
            serverInfo.Dispose();
            //get summon mobinfo
            MobInfo summon;
            if (!GetMobInfoByID(MobInfo_Summon_ID, out summon))
                throw new InvalidOperationException(String.Format("Can't find 'Resurrected Soldier' mob (ID: {0}).", MobInfo_Summon_ID));
            MobInfo_Summon = summon;
        }

        public bool GetNextFreeGroupId(out int GroupId)//need later for command....
        {
            GroupId = 0;
            IEnumerable<int> keyRange = Enumerable.Range(0, int.MaxValue);
            var freeKeys = keyRange.Except(MobGroupInfosById.Keys);
            if (freeKeys.Count() == 0)
                return false; // no free slot

            GroupId = freeKeys.First();
            return true;
        }

        public static bool GetMobInfoByID(ushort ID, out MobInfo MobInfo) => MobInfosByID.TryGetValue(ID, out MobInfo);

        public static bool GetMobInfoByIndex(string Index, out MobInfo MobInfo) => MobInfosByIndex.TryGetValue(Index, out MobInfo);

        public static bool GetMobGroupInfosById(int GroupId, out MobGroupInfo Infos) => MobGroupInfosById.TryGetValue(GroupId, out Infos);

        public static bool GetMobGroupsByMapId(ushort MapId, out ConcurrentDictionary<int, MobGroupInfo> GroupList) => MobGroupInfosByMapId.TryGetValue(MapId, out GroupList);

        public static bool GetWayPointById(int WayPointId, out WayPointInfo Info) => WayPointsById.TryGetValue(WayPointId, out Info);

        public static bool GetMobChatInfo(ushort MobId, MobChatType Type, out MobChatInfo Info)
        {
            Info = null;

            if (MobChatByType.TryGetValue(Type, out ConcurrentDictionary<ushort, MobChatInfo> Messages))
            {
                if (Messages.TryGetValue(MobId, out Info))
                    return true;
            }

            return false;
        }
    }
}