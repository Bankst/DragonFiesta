using DragonFiesta.Utils.Core;
using System;
using System.Threading;

namespace DragonFiesta.Utils.ServerTask
{
    public sealed class UpdateAbleManager : IUpdateAbleServer, IDisposable
    {
        private SecureCollection<iUpdateAble> Objects;
        private object ThreadLocker;
        private int IsDisposedInt;

        public TimeSpan UpdateInterval { get; private set; }

        public GameTime LastUpdate { get; set; }

        public UpdateAbleManager(int CheckIntervalMS)
        {
            Objects = new SecureCollection<iUpdateAble>();
            ThreadLocker = new object();
            UpdateInterval = TimeSpan.FromMilliseconds(CheckIntervalMS);
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


        ~UpdateAbleManager()
        {
            Dispose();
        }



        public bool AddObject(iUpdateAble Object)
        {
            lock (ThreadLocker)
            {
                return Objects.Add(Object);
            }
        }

        public bool RemoveObject(iUpdateAble Object)
        {
            lock (ThreadLocker)
            {
                return Objects.Remove(Object);
            }
        }

        public bool Update(GameTime Now)
        {
            if (IsDisposedInt == 1) return false;

            try
            {
                var now = ServerMainBase.InternalInstance.CurrentTime;

                lock (ThreadLocker)
                {
                    if (Objects.Count > 0)
                    {
                        for (int i = 0; i < Objects.Count; i++)
                        {
                            var obj = Objects[i];

                            try
                            {
                                if (now.Subtract(obj.LastUpdate) >= obj.UpdateInterval)
                                {
                                    obj.LastUpdate = now.Time;
                                    obj.OnUpdate(now);
                                }
                            }
                            catch (Exception ex)
                            {
                                EngineLog.Write(ex, "Error updating UpdateAble object:");
                                continue;
                            }
                        }
                        LastUpdate = now;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                EngineLog.Write(ex, "Error checking UpdateAble objects:");
                return false;
            }
        }
    }
}