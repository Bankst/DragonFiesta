using DragonFiesta.Zone.Game.Maps.Interface;
using DragonFiesta.Zone.Game.Maps.Event;
using System;

namespace DragonFiesta.Zone.Game.Stats
{
    public class LivingStats
    {
        private bool _isLoaded = false;

        private uint _SP { get; set; } = 0;
        private uint _HP { get; set; } = 0;

        private uint _LP { get; set; } = 0;

        private ILivingObject _sender;

        public  uint LP
        {
            get => _LP;
            set
            {
                var oldValue = _LP;

	            if (!_isLoaded) return;
	            _LP = (uint)Math.Min(value, _sender.Stats.FullStats.MaxLP);
	            InvokeOnLPChanged(oldValue);
            }
        }
        public uint HP
        {
            get => _HP;
            set
            {
                var oldValue = _HP;

	            if (!_isLoaded) return;
	            _HP = (uint)Math.Min(value, _sender.Stats.FullStats.MaxHP);
	            InvokeOnHPChanged(oldValue);
            }
        }

        public uint SP
        {
            get => _SP;
	        set
            {
                var oldValue = _SP;

	            if (!_isLoaded) return;
	            _SP = (uint)Math.Min(value, _sender.Stats.FullStats.MaxSP);
	            InvokeOnSPChanged(oldValue);
            }
        }

        public event EventHandler<LivingObjectInterActiveStatsChangedEventArgs> OnHPChanged;

        public event EventHandler<LivingObjectInterActiveStatsChangedEventArgs> OnSPChanged;

        public event EventHandler<LivingObjectInterActiveStatsChangedEventArgs> OnLPChanged;

        private void InvokeOnHPChanged(uint oldValue) => OnHPChanged?.Invoke(this, new LivingObjectInterActiveStatsChangedEventArgs(_sender, oldValue));

        private void InvokeOnSPChanged(uint oldValue) => OnSPChanged?.Invoke(this, new LivingObjectInterActiveStatsChangedEventArgs(_sender, oldValue));

        private void InvokeOnLPChanged(uint oldValue) => OnLPChanged?.Invoke(this, new LivingObjectInterActiveStatsChangedEventArgs(_sender, oldValue));
        public void Load(uint hp,uint sp,uint lp)
        {
            _HP = hp;
            _SP = sp;
            _LP = lp;

            _isLoaded = true;
        }
        public LivingStats(ILivingObject sender)
        {
            _sender = sender;
        }

        public void Dispose()
        {
            _sender = null;
        }
    }
}
