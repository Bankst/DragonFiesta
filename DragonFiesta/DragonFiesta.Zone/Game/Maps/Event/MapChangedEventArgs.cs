using DragonFiesta.Zone.Game.Maps.Interface;
using System;

namespace DragonFiesta.Zone.Game.Maps.Event
{
    public class MapChangedEventArgs<pObjectType, pMapType> : MapChangedEventArgs
        where pObjectType : IMapObject
        where pMapType : IMap
    {
        public new pObjectType Object { get; private set; }
        public new pMapType OldMap { get; private set; }
        public new pMapType NewMap { get; private set; }

        public MapChangedEventArgs(pObjectType Object, pMapType OldMap, pMapType NewMap)
            : base(Object, OldMap, NewMap)
        {
            this.Object = Object;
            this.OldMap = OldMap;
            this.NewMap = NewMap;
        }

        ~MapChangedEventArgs()
        {
            Object = default(pObjectType);
            OldMap = default(pMapType);
            NewMap = default(pMapType);
        }
    }

    public class MapChangedEventArgs : EventArgs
    {
        public IMapObject Object { get; private set; }
        public IMap OldMap { get; private set; }
        public IMap NewMap { get; private set; }

        public MapChangedEventArgs(IMapObject Object, IMap OldMap, IMap NewMap)
        {
            this.Object = Object;
            this.OldMap = OldMap;
            this.NewMap = NewMap;
        }

        ~MapChangedEventArgs()
        {
            Object = null;
            OldMap = null;
            NewMap = null;
        }
    }
}