using DragonFiesta.Zone.Game.Maps.Object;
using DragonFiesta.Zone.Game.Maps.Event;
using System;

namespace DragonFiesta.Zone.Game.Maps.Interface
{
    public interface IMapObject : IDisposable
    {
        MapSector MapSector { get; set; }

        InRangeCollection InRange { get; set; }

        IMap Map { get; set; }
        MapObjectType Type { get; }
        ushort MapObjectId { get; set; }
        Position Position { get; set; }

        event EventHandler<MapObjectEventArgs> OnDispose;

        bool IsDisposed { get; }
        object ThreadLocker { get; }

        void WriteDisplay(FiestaPacket packet);

        void Broadcast(FiestaPacket packet, bool IncludeSelf);

        bool GiveEXP(uint Amount, ushort MobId = 0xFFFF);
    }
}