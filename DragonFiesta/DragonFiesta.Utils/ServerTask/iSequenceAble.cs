using System;

namespace DragonFiesta.Utils.ServerTask
{
    public interface iSequenceAble
    {
        DateTime NextUpdate { get; }
        DateTime LastUpdate { get; set; }
        bool IsDisposed { get; }

        void OnUpdate(GameTime Now);
    }
}