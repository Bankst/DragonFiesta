using DragonFiesta.Providers.Maps;
using DragonFiesta.Zone.Data.WayPoints;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;

namespace DragonFiesta.Zone.Data.Mob
{
    [GameServerModule(ServerType.Zone, GameInitalStage.Mob)]
    public class MobDataProvider
    {
        public const ushort MobInfoSummonID = 6504;
        public static MobInfo MobInfoSummon { get; private set; }
        private static ConcurrentDictionary<ushort, MobInfo> _mobInfosByID;
        private static ConcurrentDictionary<string, MobInfo> _mobInfosByIndex;

        private static ConcurrentDictionary<int, MobGroupInfo> MobGroupInfosById { get; set; }

        //Mapid GroupId MobGroup
        private static ConcurrentDictionary<ushort, ConcurrentDictionary<int, MobGroupInfo>> MobGroupInfosByMapId { get; set; }

        private static ConcurrentDictionary<MobChatType, ConcurrentDictionary<ushort, MobChatInfo>> MobChatByType { get; set; }
        private static ConcurrentDictionary<int, WayPointInfo> WayPointsById { get; set; }

        [InitializerMethod]
        public static bool InitialMobData()
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

            using (var pResult = DB.Select(DatabaseType.Data, "SELECT * FROM MobChat"))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    var mobId = pResult.Read<ushort>(i, "MobId");

                    var type = (MobChatType)pResult.Read<byte>(i, "ChatType");

                    if (MobChatByType.TryGetValue(type, out var value))
                        MobChatByType.TryAdd(type, new ConcurrentDictionary<ushort, MobChatInfo>());

                    if (MobChatByType[type].TryGetValue(mobId, out var info))
                    {
                        info.MobChatList.Add(new MobChatMessageInfo(pResult, i));
                    }
                    else
                    {
                        var chatInfo = new MobChatInfo(pResult, i);
                        chatInfo.MobChatList.Add(new MobChatMessageInfo(pResult, i));
                        MobChatByType[type].TryAdd(mobId, chatInfo);
                    }
                }
            }
        }

        public static void LoadMobGroups()
        {
            //SELECT * FROM Groups
            //Find ALL Members in GroupMember

            var allMemberResult = DB.Select(DatabaseType.Data, "SELECT * FROM MobGroup_Members");
            var memberView = allMemberResult.DefaultView;
            memberView.Sort = "GroupId";

            MobGroupInfosById = new ConcurrentDictionary<int, MobGroupInfo>();
            MobGroupInfosByMapId = new ConcurrentDictionary<ushort, ConcurrentDictionary<int, MobGroupInfo>>();

            using (var pResult = DB.Select(DatabaseType.Data, "SELECT * FROM MobGroups"))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    var info = new MobGroupInfo(pResult, i);

                    if (!MapDataProvider.GetMapInfoByID(info.MapId, out var mapInfo))//Check is Sppawnned map exis...
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Can't Find MapInfo for Group {0}", info.GroupId);
                        continue;
                    }

                    if (!MobGroupInfosByMapId.ContainsKey(mapInfo.ID))
                        MobGroupInfosByMapId.TryAdd(mapInfo.ID, new ConcurrentDictionary<int, MobGroupInfo>());

                    if (MobGroupInfosByMapId[mapInfo.ID].ContainsKey(info.GroupId) || MobGroupInfosById.ContainsKey(info.GroupId))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate GroupId {0} Found!!", info.GroupId);
                        continue;
                    }

                    var memberResults = memberView.FindRows(info.GroupId);

                    if (!memberResults.Any())
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "No Members For Group {0} Found....");
                        continue;
                    }

                    foreach (var memberRow in memberResults)
                    {
                        var mobId = Convert.ToUInt16(memberRow.Row["MobId"]);
                        if (!GetMobInfoByID(mobId, out var mobInfo))
                        {
                            DatabaseLog.Write(DatabaseLogLevel.Warning, "Can't find MobId {0} for Group {1}", mobId, info.GroupId);
                            continue;
                        }

                        var memberInfo = new MobGroupMemberInfo(mobInfo, memberRow.Row);

	                    if (info.MemberInfo.TryAdd(memberInfo.MobInfo.ID, memberInfo)) continue;
	                    DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate GroupMember Found!! MobId {0} ", memberInfo.MobInfo.ID);
                    }
                    //Add Group To Data..
                    if (!MobGroupInfosByMapId[mapInfo.ID].TryAdd(info.GroupId, info) || !MobGroupInfosById.TryAdd(info.GroupId, info))
                    {
                        // TODO
                    }
                }
            }
        }

        private static void LoadWayPoints()
        {
            WayPointsById = new ConcurrentDictionary<int, WayPointInfo>();

            using (var pResult = DB.Select(DatabaseType.Data, "SELECT * FROM WayPoints"))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    var wayPointId = pResult.Read<int>(i, "Id");
                    if (GetWayPointById(wayPointId, out var point))
                    {
	                    if (point.AddPosition(pResult, i)) continue;
	                    DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate Waypoint Index found ID {0}", wayPointId);
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
	        var watch = Stopwatch.StartNew();
            _mobInfosByID = new ConcurrentDictionary<ushort, MobInfo>();
            _mobInfosByIndex = new ConcurrentDictionary<string, MobInfo>();
            //mobinfoserver
            var serverInfo = DB.Select(DatabaseType.Data, "SELECT * FROM MobInfoServer");
            var serverView = serverInfo.DefaultView;
            serverView.Sort = "ID";

            using (var pResult = DB.Select(DatabaseType.Data, "SELECT * FROM MobInfo"))
            {
                DatabaseLog.WriteProgressBar(">> Load MobInfos");

                using (var mBar = new ProgressBar(pResult.Count))
                {
                    for (var i = 0; i < pResult.Count; i++)
                    {
                        var mobID = pResult.Read<ushort>(i, "ID");
                        //find server info
                        var serverResult = serverView.FindRows(mobID);
                        if (serverResult.Length < 1)
                        {
                            DatabaseLog.Write(DatabaseLogLevel.Warning, "Can't find server info for mob with  (ID: {1})", mobID);
                            mBar.Step();
                            continue;
                        }
                        var mobInfo = new MobInfo(pResult, i, serverResult[0].Row);
                        if (!_mobInfosByID.TryAdd(mobInfo.ID, mobInfo))
                        {
                            DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate mob ID found: {0}", mobInfo.ID);
                            mBar.Step();
                            continue;
                        }
                        if (!_mobInfosByIndex.TryAdd(mobInfo.Index, mobInfo))
                        {
                            _mobInfosByID.TryRemove(mobInfo.ID, out mobInfo);
                            DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate mob index found: {0}", mobInfo.Index);
                            mBar.Step();
                            continue;
                        }
                        mBar.Step();
                    }
	                watch.Stop();

                    DatabaseLog.WriteProgressBar($">> Loaded {_mobInfosByID.Count} MobInfos in {(double) watch.ElapsedMilliseconds / 1000}s");
                }
            }
            //clean up
            serverInfo.Dispose();
            //get summon mobinfo
	        if (!GetMobInfoByID(MobInfoSummonID, out var summon))
                throw new InvalidOperationException($"Can't find 'Resurrected Soldier' mob (ID: {MobInfoSummonID}).");
            MobInfoSummon = summon;
        }

        public bool GetNextFreeGroupId(out int groupId)//need later for command....
        {
            groupId = 0;
            var keyRange = Enumerable.Range(0, int.MaxValue);
            var freeKeys = keyRange.Except(MobGroupInfosById.Keys);
            if (!freeKeys.Any())
			{
				return false; // no free slot
			}

			groupId = freeKeys.First();
            return true;
        }

        public static bool GetMobInfoByID(ushort id, out MobInfo mobInfo) => _mobInfosByID.TryGetValue(id, out mobInfo);

        public static bool GetMobInfoByIndex(string index, out MobInfo mobInfo) => _mobInfosByIndex.TryGetValue(index, out mobInfo);

        public static bool GetMobGroupInfosById(int groupId, out MobGroupInfo infos) => MobGroupInfosById.TryGetValue(groupId, out infos);

        public static bool GetMobGroupsByMapId(ushort mapId, out ConcurrentDictionary<int, MobGroupInfo> groupList) => MobGroupInfosByMapId.TryGetValue(mapId, out groupList);

        public static bool GetWayPointById(int wayPointId, out WayPointInfo info) => WayPointsById.TryGetValue(wayPointId, out info);

        public static bool GetMobChatInfo(ushort mobId, MobChatType type, out MobChatInfo info)
        {
            info = null;

            if (MobChatByType.TryGetValue(type, out var messages))
            {
                if (messages.TryGetValue(mobId, out info))
                    return true;
            }

            return false;
        }
    }
}