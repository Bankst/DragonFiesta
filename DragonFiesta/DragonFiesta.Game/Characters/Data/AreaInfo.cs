#region

using DragonFiesta.Database.SQL;
using DragonFiesta.Providers.Maps;
using DragonFiesta.Utils.Logging;

#endregion

namespace DragonFiesta.Game.Characters.Data
{
    public class AreaInfo
    {
       
        public virtual MapInfo MapInfo { get; protected set; }

        public Position Position { get; set; }

        public virtual bool RefreshFromSQL(SQLResult pRes, int i)
        {
            Position = new Position
            {
                X = pRes.Read<uint>(i, "PositionX"),
                Y = pRes.Read<uint>(i, "PositionY"),
                Rotation = pRes.Read<byte>(i, "Rotation"),
            };
            ushort mapid = pRes.Read<ushort>(i, "Map");
            if (!MapDataProvider.GetMapInfoByID(mapid, out MapInfo mMapInfo))
            {
                GameLog.Write(GameLogLevel.Warning, "Can't find mapid " + mapid + " from refeshing");
                return false;
            }

            MapInfo = mMapInfo;

            return true;
        }
    }
}
