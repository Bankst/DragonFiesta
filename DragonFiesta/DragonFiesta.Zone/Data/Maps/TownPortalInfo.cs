using DragonFiesta.Providers.Maps;
using System;
using DragonFiesta.Database.SQL;

namespace DragonFiesta.Zone.Data.Maps
{
    public sealed class TownPortalInfo
    {
        public byte Index { get; private set; }
        public byte MinLevel { get; private set; }
        public MapInfo MapInfo { get; private set; }
        public Position Position { get; private set; }

        public TownPortalInfo(SQLResult row, int i)
        {
            Index = row.Read<byte>(i, "Index");
            MinLevel = row.Read<byte>(i, "MinLevel");
            var mapIndex = row.Read<ushort>(i, "MapID");

            if (!MapDataProvider.GetMapInfoByID(mapIndex, out MapInfo mapInfo))
            {
                throw new InvalidOperationException(String.Format("Can't find map with index '{0}' for town portal info '{1}'", mapIndex, Index));
            }

            MapInfo = mapInfo;
            Position = new Position()
            {
                X = row.Read<uint>(i, "X"),
                Y = row.Read<uint>(i, "Y")
            };
        }
    }
}