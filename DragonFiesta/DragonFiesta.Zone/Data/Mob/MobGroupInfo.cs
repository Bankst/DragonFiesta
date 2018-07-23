using System.Collections.Concurrent;
using DragonFiesta.Database.SQL;

namespace DragonFiesta.Zone.Data.Mob
{
    public class MobGroupInfo
    {
        public int GroupId { get; set; }

        public bool IsFamaly { get; set; }

        public Position Center { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public ushort MapId { get; private set; }

        public int Range { get; private set; }

        public ConcurrentDictionary<ushort, MobGroupMemberInfo> MemberInfo;

        public MobGroupInfo(SQLResult Result, int i)
        {
            MemberInfo = new ConcurrentDictionary<ushort, MobGroupMemberInfo>();

            GroupId = Result.Read<int>(i, "GroupId");

            IsFamaly = Result.Read<bool>(i, "IsFamaly");

            Center = new Position()
            {
                X = Result.Read<uint>(i, "CenterX"),
                Y = Result.Read<uint>(i, "CenterY"),
            };

            Width = Result.Read<int>(i, "Width");
            Height = Result.Read<int>(i, "Height");

            MapId = Result.Read<ushort>(i, "MapId");

            Range = Result.Read<ushort>(i, "Range");
        }
    }
}