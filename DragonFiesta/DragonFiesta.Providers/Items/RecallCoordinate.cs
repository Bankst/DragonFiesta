using DragonFiesta.Providers.Maps;
using System;

namespace DragonFiesta.Providers.Data.Items
{
    public sealed class RecallCoordinate
    {
        public ushort ItemID { get; private set; }
        public MapInfo MapInfo { get; private set; }
        public Position Position { get; private set; }

        public RecallCoordinate(SQLResult pResult, int i)
        {
            ItemID = pResult.Read<ushort>(i, "ItemID");
            ushort MapID = pResult.Read<ushort>(i, "MapID");
            if (!MapDataProvider.GetMapInfoByID(MapID, out MapInfo mapInfo))
                throw new InvalidOperationException(String.Format("Can't find map with ItemID '{0}' for recall coordinate '{1}'", MapID, ItemID));
            MapInfo = mapInfo;
            Position = new Position()
            {
                X = pResult.Read<uint>(i, "X"),
                Y = pResult.Read<uint>(i, "Y"),
            };
        }
    }
}