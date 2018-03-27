using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace DragonFiesta.Utils.ServerTask
{
    public sealed class ExpireGuidManager : IUpdateAbleServer, IDisposable
    {
        public ConcurrentDictionary<Guid, IExpireGuidAble> Objects;

        private object ThreadLocker;

        private int IsDisposedInt;

        public TimeSpan UpdateInterval { get; private set; }

        public GameTime LastUpdate { get; private set; }

        public ExpireGuidManager(int CheckIntervalMS)
        {
            Objects = new ConcurrentDictionary<Guid, IExpireGuidAble>();
            ThreadLocker = new object();
            LastUpdate = GameTime.Now();
        }

        ~ExpireGuidManager()
        {
            Dispose();
        }

        public bool Update(GameTime Now)
        {
            if (IsDisposedInt == 1) return false;


            lock (ThreadLocker)
            {
                if (Objects.Count > 0)
                {
                    var expired = new List<IExpireGuidAble>();

                    foreach (var Obj in Objects.Values)
                    {
                        if (Obj.IsDisposed
                         || Now >= Obj.ExpireTime)
                        {
                            expired.Add(Obj);
                        }
                    }

                    for (int i = 0; i < expired.Count; i++)
                    {
                        var obj = expired[i];

                        if (!obj.IsDisposed && Objects.TryRemove(obj.Id, out obj))
                        {
                            obj.OnExpire(Now);
                        }
                    }
                    LastUpdate = GameTime.Now();
                    expired.Clear();
                    expired = null;
                }
            }
            return true;
        }

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref IsDisposedInt, 1, 0) == 0)
            {
                Objects.Clear();
                Objects = null;
                ThreadLocker = null;
            }
        }

        public bool AddObject(IExpireGuidAble Object)
        {
            lock (ThreadLocker)
            {
                return Objects.TryAdd(Object.Id, Object);
            }
        }

        public bool RemoveObject(Guid Id)
        {
            return RemoveObject(Id, out IExpireGuidAble obj);
        }

        public bool RemoveObject(Guid Id, out IExpireGuidAble Object)
        {
            lock (ThreadLocker)
            {
                return Objects.TryRemove(Id, out Object);
            }
        }
    }
}