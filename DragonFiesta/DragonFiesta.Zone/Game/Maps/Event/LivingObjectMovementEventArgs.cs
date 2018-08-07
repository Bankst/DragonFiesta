using DragonFiesta.Zone.Game.Maps.Interface;
using System;

namespace DragonFiesta.Zone.Game.Maps.Event
{
    public class LivingObjectMovementEventArgs : EventArgs
    {
        public ILivingObject Object { get; private set; }
        public Position OldPosition { get; private set; }
        public Position NewPosition { get; private set; }
        public bool IsRun { get; private set; }
        public bool IsStop { get; private set; }
        public bool IsFreeze { get; private set; }

        public LivingObjectMovementEventArgs(ILivingObject Object, Position OldPosition, Position NewPosition, bool IsRun, bool IsStop)
        {
            this.Object = Object;
            this.OldPosition = OldPosition;
            this.NewPosition = NewPosition;
            this.IsRun = IsRun;
            this.IsStop = IsStop;
        }

        ~LivingObjectMovementEventArgs()
        {
            Object = null;
        }
    }
}