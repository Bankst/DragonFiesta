using DragonFiesta.Utils.Core;
using DragonFiesta.Utils.ServerTask;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace DragonFiesta.Messages.Utils
{
    public class ExpireExpectManager : IUpdateAbleServer
    {
        public ConcurrentDictionary<Guid, IExpectAnAnswer> MessageObjects;

        private object ThreadLocker;

        private int IsDisposedInt;


        public GameTime LastUpdate { get; private set; }

        public TimeSpan UpdateInterval { get; private set; }

        public ExpireExpectManager(int CheckIntervalMS)
        {
            UpdateInterval = TimeSpan.FromMilliseconds(CheckIntervalMS);
            LastUpdate = GameTime.Now();
            MessageObjects = new ConcurrentDictionary<Guid, IExpectAnAnswer>();
            ThreadLocker = new object();
            
        }

        ~ExpireExpectManager()
        {
            Dispose();
        }

        bool IUpdateAbleServer.Update(GameTime Now)
        {
            if (IsDisposedInt == 1) return false;

            var now = ServerMainBase.InternalInstance.CurrentTime;

            lock (ThreadLocker)
            {
                if (MessageObjects.Count > 0)
                {
                    var expired = new List<IExpectAnAnswer>();

                    foreach (var Obj in MessageObjects.Values)
                    {
                        if (Obj.IsDisposed
                         || now >= Obj.ExpireTime)
                        {
                            expired.Add(Obj);
                        }
                    }

                    for (int i = 0; i < expired.Count; i++)
                    {
                        var obj = expired[i];

                        if (!obj.IsDisposed && MessageObjects.TryRemove(obj.Id, out obj) && obj.TimeOutCallBack != null)
                        {
                            obj?.TimeOutCallBack(obj);
                        }
                    }
                    expired.Clear();
                    expired = null;
                    LastUpdate = now;
                }
            }
            return true;
        }

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref IsDisposedInt, 1, 0) == 0)
            {
                MessageObjects.Clear();
                MessageObjects = null;
                ThreadLocker = null;
            }
        }

        public bool AddObject(IExpectAnAnswer Object)
        {
            lock (ThreadLocker)
            {
                return MessageObjects.TryAdd(Object.Id, Object);
            }
        }

        public bool RemoveObject(Guid Id)
        {
            return RemoveObject(Id, out IExpectAnAnswer obj);
        }

        public bool RemoveObject(Guid Id, out IExpectAnAnswer Object)
        {
            lock (ThreadLocker)
            {
                return MessageObjects.TryRemove(Id, out Object);
            }
        }
    }
}