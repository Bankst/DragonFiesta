using System;
using System.Collections.Generic;
using System.Threading;

namespace DragonFiesta.Utils.ServerTask
{
    public sealed class ExpireAbleManager : IUpdateAbleServer, IDisposable
    {
        public SecureCollection<iExpireAble> Objects;

        private object _threadLocker;

        private int _isDisposedInt;

        public TimeSpan UpdateInterval { get; private set; }

        public GameTime LastUpdate { get; private set; }

        public ExpireAbleManager(int checkIntervalMs)
        {
            Objects = new SecureCollection<iExpireAble>();

            LastUpdate = GameTime.Now();

            UpdateInterval = TimeSpan.FromMilliseconds(checkIntervalMs);
            _threadLocker = new object();

        }

        ~ExpireAbleManager()
        {
            Dispose();
        }

        public bool Update(GameTime gameTime)
        {
            if (_isDisposedInt == 1) return false;

            lock (_threadLocker)
            {
	            if (Objects.Count <= 0) return true;
	            var expired = new List<iExpireAble>();

	            foreach (var obj in Objects)
	            {
		            if (obj.IsDisposed
		                || gameTime >= obj.ExpireTime)
		            {
			            expired.Add(obj);
		            }
		            obj.Update(gameTime);
	            }
	            foreach (var obj in expired)
	            {
		            Objects.Remove(obj);
		            if (!obj.IsDisposed)
		            {
			            obj.OnExpire(gameTime);
		            }
	            }
	            expired.Clear();
	            expired = null;
            }
            return true;
        }

        public void Dispose()
        {
	        if (Interlocked.CompareExchange(ref _isDisposedInt, 1, 0) != 0) return;
	        Objects.Dispose();
	        Objects = null;
	        _threadLocker = null;
        }

        public bool AddObject(iExpireAble Object)
        {
            lock (_threadLocker)
            {
                return Objects.Add(Object);
            }
        }

        public bool RemoveObject(iExpireAble Object)
        {
            lock (_threadLocker)
            {
                return Objects.Remove(Object);
            }
        }
    }
}