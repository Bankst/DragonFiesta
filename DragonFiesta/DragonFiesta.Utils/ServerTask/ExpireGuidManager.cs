using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DragonFiesta.Utils.ServerTask
{
    public sealed class ExpireGuidManager : IUpdateAbleServer, IDisposable
    {
        public ConcurrentDictionary<Guid, IExpireGuidAble> Objects;

        private object _threadLocker;

        private int _isDisposedInt;

        public TimeSpan UpdateInterval { get; private set; }

        public GameTime LastUpdate { get; private set; }

        public ExpireGuidManager(int checkIntervalMs)
        {
            Objects = new ConcurrentDictionary<Guid, IExpireGuidAble>();
            _threadLocker = new object();
            LastUpdate = GameTime.Now();
        }

        ~ExpireGuidManager()
        {
            Dispose();
        }

        public bool Update(GameTime gameTime)
        {
            if (_isDisposedInt == 1) return false;


            lock (_threadLocker)
            {
	            if (Objects.Count <= 0) return true;
	            var expired = Objects.Values.Where(obj => obj.IsDisposed || gameTime >= obj.ExpireTime).ToList();

	            foreach (var t in expired)
	            {
		            var obj = t;

		            if (!obj.IsDisposed && Objects.TryRemove(obj.Id, out obj))
		            {
			            obj.OnExpire(gameTime);
		            }
	            }
	            LastUpdate = GameTime.Now();
	            expired.Clear();
	            expired = null;
            }
            return true;
        }

        public void Dispose()
        {
	        if (Interlocked.CompareExchange(ref _isDisposedInt, 1, 0) != 0) return;
	        Objects.Clear();
	        Objects = null;
	        _threadLocker = null;
        }

        public bool AddObject(IExpireGuidAble Object)
        {
            lock (_threadLocker)
            {
                return Objects.TryAdd(Object.Id, Object);
            }
        }

        public bool RemoveObject(Guid id)
        {
            return RemoveObject(id, out IExpireGuidAble obj);
        }

        public bool RemoveObject(Guid id, out IExpireGuidAble Object)
        {
            lock (_threadLocker)
            {
                return Objects.TryRemove(id, out Object);
            }
        }
    }
}