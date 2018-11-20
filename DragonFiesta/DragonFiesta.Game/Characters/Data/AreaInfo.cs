#region

using DragonFiesta.Database.Models;
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

	    public virtual bool RefreshFromEntity(CharacterBase character)
	    {
			Position = new Position
			{
				X = (uint) character.PositionX,
				Y = (uint) character.PositionY,
				Rotation = character.Rotation

			};

		    if (!MapDataProvider.GetMapInfoByID((ushort) character.Map, out var mMapInfo))
		    {
			    GameLog.Write(GameLogLevel.Warning, $@"Can't find Map ID {character.Map} for refresh");
			    return false;
			}

		    MapInfo = mMapInfo;

		    return true;
	    }

        public virtual bool RefreshFromSQL(SQLResult pRes, int i)
        {
            Position = new Position
            {
                X = pRes.Read<uint>(i, "PositionX"),
                Y = pRes.Read<uint>(i, "PositionY"),
                Rotation = pRes.Read<byte>(i, "Rotation"),
            };
            var mapid = pRes.Read<ushort>(i, "Map");
            if (!MapDataProvider.GetMapInfoByID(mapid, out var mMapInfo))
            {
				GameLog.Write(GameLogLevel.Warning, $@"Can't find Map ID {mapid} for refresh");
				return false;
            }

            MapInfo = mMapInfo;

            return true;
        }
    }
}
