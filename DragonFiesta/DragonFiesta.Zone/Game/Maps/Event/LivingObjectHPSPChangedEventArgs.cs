using DragonFiesta.Zone.Game.Maps.Interface;
using System;

namespace DragonFiesta.Zone.Game.Maps.Event
{
    public class LivingObjectInterActiveStatsChangedEventArgs : EventArgs
    {
        public ILivingObject Object { get; private set; }
        public uint OldValue { get; private set; }

        public LivingObjectInterActiveStatsChangedEventArgs(ILivingObject Object, uint OldValue)
        {
            this.Object = Object;
            this.OldValue = OldValue;
        }

        ~LivingObjectInterActiveStatsChangedEventArgs()
        {
            Object = null;
        }
    }
}