using DragonFiesta.Providers.Maps;
using DragonFiesta.Zone.Data.Maps;
using DragonFiesta.Zone.Game.Maps.Object;
using System;
using System.Runtime.Serialization;
using System.Threading;

namespace DragonFiesta.Zone.Game.Maps.Types
{
    public class SectorMap : IMap
    {
        public bool IsDisposed { get { return (IsDisposedInt > 0); } }
        private int IsDisposedInt;

        public const uint DefaultSectorCount = 16;
        public uint SectorCount { get; private set; }

        public uint SectorWidth { get; private set; }
        public uint SectorHeight { get; private set; }

        public MapSector[,] Sectors { get; private set; }
        public FieldInfo Info { get; private set; }

        public BlockInfo BlockInfos { get; private set; }
        public ushort MapId => Info.MapInfo.ID;

        public byte ZoneId => Info.ZoneID;

        protected object ThreadLocker { get; set; }

        public MapInfo MapInfo => Info.MapInfo;

        public SectorMap(FieldInfo Info)
        {
            ThreadLocker = new object();
            this.Info = Info;
        }

        ~SectorMap()
        {
            Dispose();
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
            Info = null;
            BlockInfos = null;
            ThreadLocker = null;

            SectorAction((s) => s.Dispose());
            Sectors = null;
        }

        public void SectorAction(Action<MapSector> Action)
        {
            for (uint y = 0; y < SectorCount; y++)
            {
                for (uint x = 0; x < SectorCount; x++)
                {
                    Action.Invoke(Sectors[y, x]);
                }
            }
        }

        public MapSector GetSectorByPosition(Position Position) => GetSectorByPosition(Position.X, Position.Y);

        public MapSector GetSectorByPosition(uint X, uint Y) => Sectors[(Y / SectorHeight), (X / SectorWidth)];

        public bool CheckPositionInMap(Position Position) => ((Position.Y / SectorHeight) < SectorCount || (Position.X / SectorWidth) < SectorCount);

        public virtual bool Start()
        {
            if (!Data.Maps.ZoneMapDataProvider.GetBlockInfoByMapID(MapId, out BlockInfo block))
            {
                DatabaseLog.Write(DatabaseLogLevel.Warning, "Error loading sector map. Can't find block info for map '{0}'.", Info.MapInfo.Index);
                return false;
            }
            BlockInfos = block;

            SectorCount = DefaultSectorCount;

            while ((block.Width / SectorCount) < Info.MapInfo.ViewRange
                && SectorCount != 1)
            {
                SectorCount--;
            }

            SectorWidth = Math.Max((uint)Info.MapInfo.ViewRange, (block.Width / SectorCount));
            SectorHeight = Math.Max((uint)Info.MapInfo.ViewRange, (block.Height / SectorCount));

            Sectors = new MapSector[SectorCount, SectorCount];

            for (uint y = 0; y < SectorCount; y++)
            {
                for (uint x = 0; x < SectorCount; x++)
                {
                    Sectors[y, x] = new MapSector(this, x, y);
                }
            }
            SectorAction((s) => (s as MapSector).Load());

            return true;
        }

        public virtual bool Stop()
        {
            return true;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}