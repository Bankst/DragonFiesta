using DragonFiesta.Zone.Game.Maps.Interface;
using DragonFiesta.Zone.Game.Maps.Object;
using System;

namespace DragonFiesta.Zone.Game.Maps.Event
{
    public class MapSectorChangedEventArgs<pObjectType> : MapSectorChangedEventArgs
               where pObjectType : IMapObject
    {
        public new pObjectType Object { get; private set; }

        public MapSectorChangedEventArgs(pObjectType Object, MapSector OldSector, MapSector NewSector)
            : base(Object, OldSector, NewSector)
        {
            this.Object = Object;
        }

        ~MapSectorChangedEventArgs()
        {
            Object = default(pObjectType);
        }
    }

    public class MapSectorChangedEventArgs : EventArgs
    {
        public IMapObject Object { get; private set; }
        public MapSector OldSector { get; private set; }
        public MapSector NewSector { get; private set; }

        public MapSectorChangedEventArgs(IMapObject Object, MapSector OldSector, MapSector NewSector)
        {
            this.Object = Object;
            this.OldSector = OldSector;
            this.NewSector = NewSector;
        }

        ~MapSectorChangedEventArgs()
        {
            Object = null;

            OldSector = null;
            NewSector = null;
        }
    }
}