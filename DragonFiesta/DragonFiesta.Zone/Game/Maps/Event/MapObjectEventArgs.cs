using DragonFiesta.Zone.Game.Maps.Interface;
using System;

namespace DragonFiesta.Zone.Game.Maps.Event
{
    public class MapObjectEventArgs : EventArgs
    {
        public IMapObject MapObject { get; private set; }

        public MapObjectEventArgs(IMapObject MapObject)
        {
            this.MapObject = MapObject;
        }

        ~MapObjectEventArgs()
        {
            MapObject = null;
        }
    }
}