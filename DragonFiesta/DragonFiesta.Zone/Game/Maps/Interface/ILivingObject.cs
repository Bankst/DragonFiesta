using DragonFiesta.Game.Stats;
using DragonFiesta.Zone.Game.Maps.Event;
using DragonFiesta.Zone.Game.Stats;
using System;
using DragonFiesta.Zone.Game.Maps.Object;

namespace DragonFiesta.Zone.Game.Maps.Interface
{
    public interface ILivingObject : IMapObject
    {

        byte Level { get; set; }
        LivingStats LivingStats { get; set; }

        event EventHandler<MapChangedEventArgs> OnMapChanged;

        event EventHandler<MapSectorChangedEventArgs> OnMapSectorChanged;

        event EventHandler<LivingObjectMovementEventArgs> OnMove;

        event EventHandler<UpdateCounterChangeEventArgs> OnUpdateCounterChanged;

        LivingObjectSelectionBase Selection { get; set; }
        StatsManager Stats { get; }

        ushort UpdateCounter { get; }
        bool IsAlive { get; }

        void Die();

        void WriteSelectionUpdate(FiestaPacket Packet);
    }
}