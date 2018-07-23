#region

using System;
using System.Threading;
using DragonFiesta.Utils.Core;
using DragonFiesta.Utils.ServerTask;

#endregion

namespace DragonFiesta.Game.Transfer
{
    public class ServerTransferBase : iExpireAble
    {
        public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

        private DateTime ExpireTime;
        DateTime iExpireAble.ExpireTime { get { return ExpireTime; } }

        public bool IsDisposed { get { return (IsDisposedInt > 0); } }

        private int IsDisposedInt;

        public ServerTransferBase()
        {
            ExpireTime = ServerMainBase.InternalInstance.CurrentTime.Time.Add(DefaultTimeout);
        }

        ~ServerTransferBase()
        {
            Dispose();
        }

        public virtual void OnExpire(GameTime gameTime)
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref IsDisposedInt, 1, 0) == 0)
            {
                DisposeInternal();
            }
        }

        protected virtual void DisposeInternal()
        {
        }
    }
}