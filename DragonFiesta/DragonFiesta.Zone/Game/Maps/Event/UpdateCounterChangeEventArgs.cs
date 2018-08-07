using DragonFiesta.Zone.Game.Maps.Interface;
using System;

public class UpdateCounterChangeEventArgs : EventArgs
{
    public ushort UpdateCounter { get; private set; }

    public ILivingObject Owner { get; private set; }

    public UpdateCounterChangeEventArgs(ILivingObject Owner,ushort UpdateCounter)
    {
        this.Owner = Owner;
        this.UpdateCounter = UpdateCounter;
    }

    ~UpdateCounterChangeEventArgs()
    {
        Owner = null;
    }
}
