using DragonFiesta.Game.Characters.Data;
using DragonFiesta.Providers.Maps;
using System;

namespace DragonFiesta.Zone.Game.Command.Data
{
    public sealed class TeleLocation : AreaInfo
    {
        public string LocationName { get; set; }


        public TeleLocation(MapInfo MapInfo)
        {
            this.MapInfo = MapInfo;
        }
        public TeleLocation(SQLResult pRes, int i) : base()
        {

            if (!base.RefreshFromSQL(pRes, i))
                throw new InvalidProgramException("Invalid load TeleLocation");

            LocationName = pRes.Read<string>(i, "LocationName");

        }
    }
}
