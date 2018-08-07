using System;

namespace DragonFiesta.Utils.ServerTask
{
    public interface iUpdateAble
    {
        TimeSpan UpdateInterval { get; }
        DateTime LastUpdate { get; set; }

        void OnUpdate(GameTime gameTime);
    }
}