using DragonFiesta.Utils.Core;
using System;
using System.Threading;

namespace DragonFiesta.Game.ServerConsole.Handler
{
    public abstract class ShutdownHandlerBase : IServerTask
    {
      
        private DateTime ExpireTime { get; set; }

        public bool IsDisposed { get { return (IsDisposedInt > 0); } }

        private int IsDisposedInt;

        private string Reason { get; set; }

        public ServerTaskTimes Interval { get; private set; }

        GameTime IServerTask.LastUpdate { get; set; }

        public ShutdownHandlerBase(int ShutdownTime, string reason)
        {
            Reason = reason;
            ExpireTime = ServerMainBase.InternalInstance.CurrentTime.Time.Add(TimeSpan.FromSeconds(ShutdownTime));
        }

        public abstract void FinalyShutdown();
        public abstract void ShutdownSequense_1Seconds(string Reason, TimeSpan RestTime);
        public abstract void ShutdownSequense_30Seconds(string Reason, TimeSpan RestTime);

        public abstract void ShutdownSequense_5Minutes(string Reason, TimeSpan RestTime);
        public abstract void ShutdownSequense_2Minutes(string Reason, TimeSpan RestTime);

        protected void UpdateTime(int Seconds, string Reason)
        {
            this.Reason = Reason;
            ExpireTime = GameTime.Now().Time.AddSeconds(Seconds);
        }

        bool IServerTask.Update(GameTime Now)
        {
            if (IsDisposedInt == 1) return false;

            TimeSpan RestTime = ExpireTime.Subtract(Now.Time);



            if (RestTime.TotalHours <= 1 &&
                RestTime.TotalHours > 1 &&
                RestTime.TotalMinutes >= 20)
            {
                ShutdownSequense_5Minutes(Reason, RestTime);

                Interval = (ServerTaskTimes)TimeSpan.FromMinutes(5).TotalMilliseconds;

                return true;
            }
            else if (RestTime.TotalMinutes <= 20 &&
                    RestTime.TotalSeconds > 30 &&
                    RestTime.TotalMinutes > 2)
            {
                ShutdownSequense_2Minutes(Reason, RestTime);

                Interval = (ServerTaskTimes)TimeSpan.FromMinutes(2).TotalMilliseconds;
                return true;
            }
            if (RestTime.TotalMinutes <= 2 &&
                RestTime.TotalSeconds > 30)
            {
                ShutdownSequense_30Seconds(Reason, RestTime);

                Interval = (ServerTaskTimes)TimeSpan.FromSeconds(30).TotalMilliseconds;
                return true;
            }
            else if (RestTime.TotalSeconds <= 30)
            {
                Interval = (ServerTaskTimes)TimeSpan.FromSeconds(1).TotalMilliseconds;

                if (RestTime.TotalSeconds < 1)
                {
                    FinalyShutdown();
                    return false;
                }
                ShutdownSequense_1Seconds(Reason, RestTime);

                return true;
            }

            return true;
        }


        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref IsDisposedInt, 1, 0) == 0)
            {
                DisposeInternal();
            }
        }

        protected void DisposeInternal()
        {
        }



    }
}