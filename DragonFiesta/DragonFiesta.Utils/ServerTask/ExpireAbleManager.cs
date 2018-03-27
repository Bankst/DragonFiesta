using System;
using System.Collections.Generic;
using System.Threading;

namespace DragonFiesta.Utils.ServerTask
{
    public sealed class ExpireAbleManager : IUpdateAbleServer, IDisposable
    {
        public SecureCollection<iExpireAble> Objects;

        private object ThreadLocker;

        private int IsDisposedInt;

        public TimeSpan UpdateInterval { get; private set; }

        public GameTime LastUpdate { get; private set; }

        public ExpireAbleManager(int CheckIntervalMS)
        {
            Objects = new SecureCollection<iExpireAble>();

            LastUpdate = GameTime.Now();

            UpdateInterval = TimeSpan.FromMilliseconds(CheckIntervalMS);
            ThreadLocker = new object();

        }

        ~ExpireAbleManager()
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
                    var expired = new List<iExpireAble>();

                    for (int i = 0; i < Objects.Count; i++)
                    {
                        var obj = Objects[i];

                        if (obj.IsDisposed
                            || Now >= obj.ExpireTime)
                        {
                            expired.Add(obj);
                        }
                        obj.Update(Now);
                    }
                    for (int i = 0; i < expired.Count; i++)
                    {
                        var obj = expired[i];
                        Objects.Remove(obj);
                        if (!obj.IsDisposed)
                        {
                            obj.OnExpire(Now);
                        }
                    }
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
                Objects.Dispose();
                Objects = null;
                ThreadLocker = null;
            }
        }

        public bool AddObject(iExpireAble Object)
        {
            lock (ThreadLocker)
            {
                return Objects.Add(Object);
            }
        }

        public bool RemoveObject(iExpireAble Object)
        {
            lock (ThreadLocker)
            {
                return Objects.Remove(Object);
            }
        }
    }
}