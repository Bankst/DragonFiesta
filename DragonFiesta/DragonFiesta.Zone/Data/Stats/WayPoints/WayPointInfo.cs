using System.Collections.Concurrent;

namespace DragonFiesta.Zone.Data.WayPoints
{
    public class WayPointInfo
    {
        public int Id { get; private set; }

        public ConcurrentDictionary<ushort, Position> WalkPosition { get; private set; }

        public ushort MaxMoveIndex { get; private set; } = 1;
        public WayPointInfo(SQLResult pResult, int i)
        {
            Id = pResult.Read<int>(i, "ID");
            WalkPosition = new ConcurrentDictionary<ushort, Position>();
        }

        public bool AddPosition(SQLResult Res, int i)
        {
            ushort index = Res.Read<ushort>(i, "Index");

            if(index > MaxMoveIndex)
            {
                MaxMoveIndex = index;
            }

            return WalkPosition.TryAdd(index,
             new Position(Res.Read<uint>(i, "PosX"),
                 Res.Read<uint>(i, "PosY"),
                 Res.Read<byte>(i, "Rotation")));
        }

    }
}