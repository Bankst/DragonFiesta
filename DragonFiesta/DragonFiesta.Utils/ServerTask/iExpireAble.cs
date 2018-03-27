using System;

namespace DragonFiesta.Utils.ServerTask
{
    public interface iExpireAble
    {
        DateTime ExpireTime { get; }
        bool IsDisposed { get; }

        void OnExpire(GameTime Now);

        void Update(GameTime Now);
    }
}