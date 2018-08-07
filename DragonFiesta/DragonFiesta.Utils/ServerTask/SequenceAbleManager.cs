using DragonFiesta.Utils.Core;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DragonFiesta.Utils.ServerTask
{
    public sealed class SequenceAbleManager : IUpdateAbleServer, IDisposable
    {
        private SecureCollection<iSequenceAble> Objects;
        private object ThreadLocker;
        private int IsDisposedInt;

   
        public GameTime LastUpdate { get; set; }

       public TimeSpan UpdateInterval { get; private set; }

        public SequenceAbleManager(ServerTaskTimes TimeEnum) : this((int)TimeEnum)
        {
        }
        public SequenceAbleManager(int CheckIntervalMS)
        {
            Objects = new SecureCollection<iSequenceAble>();
            UpdateInterval = TimeSpan.FromMilliseconds(CheckIntervalMS);

            ThreadLocker = new object();
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


        ~SequenceAbleManager()
        {
            Dispose();
        }

        public bool AddObject(iSequenceAble Object)
        {
            lock (ThreadLocker)
            {
                return Objects.Add(Object);
            }
        }

        public bool RemoveObject(iSequenceAble Object)
        {
            lock (ThreadLocker)
            {
                return Objects.Remove(Object);
            }
        }

        public bool Update(GameTime gameTime)
        {
            if (IsDisposedInt == 1) return false;

            try
            {
                var now = ServerMainBase.InternalInstance.CurrentTime;

                lock (ThreadLocker)
                {
                    if (Objects.Count > 0)
                    {
                        var toRemove = new List<iSequenceAble>();

                        for (int i = 0; i < Objects.Count; i++)
                        {
                            var obj = Objects[i];

                            if (obj.IsDisposed)
                            {
                                toRemove.Add(obj);
                            }
                            else if (now.Time >= obj.NextUpdate)
                            {
                                obj.LastUpdate = now.Time;
                                obj.OnUpdate(now);
                            }
                        }
                        for (int i = 0; i < toRemove.Count; i++)
                        {
                            var obj = toRemove[i];

                            Objects.Remove(obj);
                        }
                        toRemove.Clear();
                        toRemove = null;
                        LastUpdate = now;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                EngineLog.Write(ex, "Error checking SequenceAble objects:");
                return false;
            }
        }
    }
}