using DragonFiesta.Utils.IO.TXT;
using DragonFiesta.Zone.Game.Maps.Event;
using DragonFiesta.Zone.Game.Maps.Interface;
using System;
using System.Threading;

namespace DragonFiesta.Zone.Game.Maps.Object
{
    public abstract class MapObject : IMapObject
    {  
        public event EventHandler<MapSectorChangedEventArgs> OnMapSectorChanged;

        public abstract MapObjectType Type { get; }
        public ushort MapObjectId { get; set; }

        public virtual InRangeCollection InRange { get; set; }
        public Position Position { get; set; }

        public bool IsDisposed { get { return (IsDisposedInt > 0); } }
        private int IsDisposedInt;

        public object ThreadLocker { get; private set; }

        public IMap Map { get; set; }

        private MapSector Sector;
        public  MapSector MapSector
        {
            get { return Sector; }
            set
            {
                var oldSector = Sector;
                Sector = value;
                InvokeOnMapSectorChanged(oldSector);
            }
        }

        public event EventHandler<MapObjectEventArgs> OnDispose;

        ~MapObject()
        {
            Dispose();
        }

        public MapObject()
        {
            ThreadLocker = new object();
        }

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref IsDisposedInt, 1, 0) == 0)
            {
                DisposeInternal();
            }
        }

        protected virtual void DisposeInternal()
        {
            OnDispose?.Invoke(this, new MapObjectEventArgs(this));
            OnDispose = null;
            InRange.Dispose();
            InRange = null;
            Sector = null;
            Map = null;
        }

        public virtual void WriteDisplay(FiestaPacket packet)
        {
            throw new NotImplementedException();
        }

        public void Broadcast(FiestaPacket Packet, bool IncludeSelf) => InRange.Broadcast(Packet);

        protected virtual void InvokeOnMapSectorChanged(MapSector OldSector)
        {
            if (OnMapSectorChanged != null)
            {
                var args = new MapSectorChangedEventArgs(this, OldSector, MapSector);
                OnMapSectorChanged.Invoke(this, args);
            }
        }

        public bool GiveEXP(uint Amount)
        {
            throw new NotImplementedException();
        }

        public bool GiveEXP(uint Amount, ushort MobId = ushort.MaxValue)
        {
            return true;
        }
    }
}