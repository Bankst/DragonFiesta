using DragonFiesta.Utils.Core;
using DragonFiesta.Utils.ServerTask;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DragonFiesta.Messages.Utils
{
    public class ExpireExpectManager : IUpdateAbleServer
    {
        public ConcurrentDictionary<Guid, IExpectAnAnswer> MessageObjects;

        private object _threadLocker;

        private int _isDisposedInt;


        public GameTime LastUpdate { get; private set; }

        public TimeSpan UpdateInterval { get; private set; }

        public ExpireExpectManager(int checkIntervalMs)
        {
            UpdateInterval = TimeSpan.FromMilliseconds(checkIntervalMs);
            LastUpdate = GameTime.Now();
            MessageObjects = new ConcurrentDictionary<Guid, IExpectAnAnswer>();
            _threadLocker = new object();
            
        }

        ~ExpireExpectManager()
        {
            Dispose();
        }

        bool IUpdateAbleServer.Update(GameTime gameTime)
        {
            if (_isDisposedInt == 1) return false;

            var now = ServerMainBase.InternalInstance.CurrentTime;

            lock (_threadLocker)
            {
	            if (MessageObjects.Count <= 0) return true;
	            var expired = MessageObjects.Values.Where(obj => obj.IsDisposed || now >= obj.ExpireTime).ToList();

	            foreach (var t in expired)
	            {
		            var obj = t;

		            if (!obj.IsDisposed && MessageObjects.TryRemove(obj.Id, out obj) && obj.TimeOutCallBack != null)
		            {
			            obj?.TimeOutCallBack(obj);
		            }
	            }
	            expired.Clear();
	            expired = null;
	            LastUpdate = now;
            }
            return true;
        }

        public void Dispose()
        {
	        if (Interlocked.CompareExchange(ref _isDisposedInt, 1, 0) != 0) return;
	        lock (_threadLocker)
	        {
		        MessageObjects.Clear();
	        }
	        lock (_threadLocker)
	        {
		        MessageObjects = null;
	        }
	        _threadLocker = null;
        }

        public bool AddObject(IExpectAnAnswer Object)
        {
            lock (_threadLocker)
            {
                return MessageObjects.TryAdd(Object.Id, Object);
            }
        }

        public bool RemoveObject(Guid id)
        {
            return RemoveObject(id, out IExpectAnAnswer obj);
        }

        public bool RemoveObject(Guid id, out IExpectAnAnswer Object)
        {
            lock (_threadLocker)
            {
                return MessageObjects.TryRemove(id, out Object);
            }
        }
    }
}