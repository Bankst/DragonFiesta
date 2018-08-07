using DragonFiesta.Zone.Data.WayPoints;
using System;
using System.Data;

namespace DragonFiesta.Zone.Data.Mob
{
    public class MobGroupMemberInfo
    {
        public int GroupId { get; set; }

        public bool HasWayPoint => WayPointInfo != null;
        public WayPointInfo WayPointInfo { get; private set; }
        public MobInfo MobInfo { get; set; }

        public ushort MobCount { get; set; }

        public int RespawnTime { get; set; }

        public MobGroupMemberInfo(MobInfo Info, DataRow row)
        {
            MobInfo = Info;
            GroupId = Convert.ToInt32(row["GroupId"]);
            MobCount = Convert.ToUInt16(row["MobCount"]);
            RespawnTime = Convert.ToInt32(row["RespawnTime"]);

            var WayPointId = Convert.ToInt32(row["WayPoint"]);
            WayPointInfo WayPoint = null;
            if (WayPointId != 0 && !MobDataProvider.GetWayPointById(WayPointId, out WayPoint))
            {
                throw new InvalidOperationException($"Can't find Waypoint {WayPointId}  for MobGroupMember MobId {Info.ID}");
            }

            if (WayPoint != null)
            {
                WayPointInfo = WayPoint;
            }
        }
    }
}