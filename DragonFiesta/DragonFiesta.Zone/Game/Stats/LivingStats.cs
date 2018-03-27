using DragonFiesta.Zone.Game.Maps.Interface;
using DragonFiesta.Zone.Game.Maps.Event;
using System;

namespace DragonFiesta.Zone.Game.Stats
{
    public class LivingStats
    {
        private bool IsLoaded = false;

        private uint _SP { get; set; } = 0;
        private uint _HP { get; set; } = 0;

        private uint _LP { get; set; } = 0;

        private ILivingObject Sender;

        public  uint LP
        {
            get => _LP;
            set
            {
                uint OldValue = _LP;

                if (IsLoaded)
                {
                    _LP = (uint)Math.Min(value, Sender.Stats.FullStats.MaxLP);
                    InvokeOnLPChanged(OldValue);
                }
            }
        }
        public uint HP
        {
            get => _HP;
            set
            {
                uint OldValue = _HP;

                if (IsLoaded)
                {
                    _HP = (uint)Math.Min(value, Sender.Stats.FullStats.MaxHP);
                    InvokeOnHPChanged(OldValue);
                }
            }
        }

        public uint SP
        {
            get { return _SP; }
            set
            {
                uint OldValue = _SP;

                if (IsLoaded)
                {
                    _SP = (uint)Math.Min(value, Sender.Stats.FullStats.MaxSP);
                    InvokeOnSPChanged(OldValue);
                }
            }
        }

        public event EventHandler<LivingObjectInterActiveStatsChangedEventArgs> OnHPChanged;

        public event EventHandler<LivingObjectInterActiveStatsChangedEventArgs> OnSPChanged;

        public event EventHandler<LivingObjectInterActiveStatsChangedEventArgs> OnLPChanged;

        private void InvokeOnHPChanged(uint OldValue) => OnHPChanged?.Invoke(this, new LivingObjectInterActiveStatsChangedEventArgs(Sender, OldValue));

        private void InvokeOnSPChanged(uint OldValue) => OnSPChanged?.Invoke(this, new LivingObjectInterActiveStatsChangedEventArgs(Sender, OldValue));

        private void InvokeOnLPChanged(uint OldValue) => OnLPChanged?.Invoke(this, new LivingObjectInterActiveStatsChangedEventArgs(Sender, OldValue));
        public void Load(uint HP,uint SP,uint LP)
        {
            _HP = HP;
            _SP = SP;
            _LP = LP;

            IsLoaded = true;
        }
        public LivingStats(ILivingObject Sender)
        {
            this.Sender = Sender;
        }

        public void Dispose()
        {
            this.Sender = null;
        }
    }
}
