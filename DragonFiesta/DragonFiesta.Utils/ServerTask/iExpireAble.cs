using System;

namespace DragonFiesta.Utils.ServerTask
{
    public interface iExpireAble
    {
        DateTime ExpireTime { get; }
        bool IsDisposed { get; }

        void OnExpire(GameTime gameTime);

        void Update(GameTime gameTime);
    }
}