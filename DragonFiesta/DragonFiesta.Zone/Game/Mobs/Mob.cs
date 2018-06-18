using DragonFiesta.Game.Stats;
using DragonFiesta.Zone.Data.Mob;
using DragonFiesta.Zone.Game.Maps.Event;
using DragonFiesta.Zone.Game.Maps.Object;
using DragonFiesta.Zone.Game.Maps.Interface;
using DragonFiesta.Zone.Game.Stats;
using System;
using System.Collections.Concurrent;

namespace DragonFiesta.Zone.Game.Mobs
{
    public class Mob : MapObject, ILivingObject
    {
        public virtual StatsManager Stats => _Stats;

        public ConcurrentDictionary<ushort, Position> WalkPosition { get; internal set; }

        public event EventHandler<LivingObjectMovementEventArgs> OnMove;

        public event EventHandler<MapChangedEventArgs> OnMapChanged;

        public event EventHandler<UpdateCounterChangeEventArgs> OnUpdateCounterChanged;
        public DateTime LastMovment { get; set; }

        public LivingObjectSelectionBase Selection { get; set; }

        public virtual byte Level { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private ushort _UpdateCounter;

        public ushort UpdateCounter
        {
            get
            {
                if (_UpdateCounter == 0xffff)
                {
                    _UpdateCounter = 0;
                }
                _UpdateCounter++;
                return _UpdateCounter;
            }
        }

        public ushort MaxMovement = 1;
        private ushort MoveCounter = 0;
        public ushort MoveIndex
        {
            get
            {
                if (MoveCounter == MaxMovement)
                {
                    MoveCounter = 0;
                    return MoveCounter;
                }
                MoveCounter++;
                return MoveCounter;
            }
        }

        public new IMap Map
        {
            get { return base.Map; }
            set
            {
                var oldMap = base.Map;
                base.Map = value;
                InvokeOnMapChanged(oldMap);
            }
        }

        public LivingStats LivingStats { get; set; }

        public MobInfo Info { get; internal set; }
        public override MapObjectType Type => MapObjectType.Mob;

        public virtual bool IsAlive => LivingStats.HP > 0;
        private MobStatsManager _Stats;

        public MobChat Chat { get; private set; }
        public DateTime LastUpdate { get; internal set; }

        public Mob(MobInfo Info) : base()
        {
            this.Info = Info;
            LivingStats = new LivingStats(this);
            _Stats = new MobStatsManager(this);

            _Stats.UpdateAll();

            LivingStats.Load((uint)_Stats.FullStats.MaxHP,
                (uint)_Stats.FullStats.MaxSP,
                (uint)_Stats.FullStats.MaxLP);

            Chat = new MobChat(this);
            Selection = new MobObjectSelection(this);
            InRange = new InRangeCollection(this);
            WalkPosition = new ConcurrentDictionary<ushort, Position>();

        }

        public override void WriteDisplay(FiestaPacket packet)
        {
            packet.Write<ushort>(MapObjectId);
            packet.Write<bool>(1);
            packet.Write<ushort>(Info.ID);
            packet.Write<uint>(Position.X);
            packet.Write<uint>(Position.Y);
            packet.Write<byte>(Position.Rotation);
            packet.Write<byte>(0);
            packet.Fill(140, 00); // EU -> 139
        }

        public virtual void WriteSelectionUpdate(FiestaPacket Packet)
        {
            Packet.Write<uint>(100);
            Packet.Write<uint>(100);
            Packet.Write<uint>(LivingStats.SP);
            Packet.Write<uint>(LivingStats.SP);
            Packet.Write<byte>(200);
            Packet.Write<ushort>(UpdateCounter);
        }

        protected override void DisposeInternal()
        {
            Chat?.Dispose();
            Chat = null;
            Info = null;
            _Stats = null;
        }

        protected virtual void InvokeOnMapChanged(IMap OldMap)
        {
            if (OnMapChanged != null)
            {
                var args = new MapChangedEventArgs(this, OldMap, Map);
                OnMapChanged.Invoke(this, args);
            }
        }

        public void Move(Position NewPosition, bool IsRun, bool IsStop)
        {
            lock (ThreadLocker)
            {
                var OldPosition = Position;
                Position = NewPosition;
                if (OnMove != null)
                {
                    var args = new LivingObjectMovementEventArgs(this, OldPosition, NewPosition, IsRun, IsStop);
                    OnMove.Invoke(this, args);
                }
            }
        }

        public virtual void MoveToNextPoint()
        {
            if (WalkPosition.TryGetValue(MoveIndex, out Position WayPoint))
            {
                Move(WayPoint, false, false);
            }
        }

        public virtual void Die()
        {
            throw new NotImplementedException();
        }
    }
}