using DragonFiesta.Utils.Core;
using System;
using System.Threading;

namespace DragonFiesta.Game.ServerConsole.Handler
{
    public abstract class ShutdownHandlerBase : IServerTask
    {
      
        private DateTime ExpireTime { get; set; }

        public bool IsDisposed => (_isDisposedInt > 0);

	    private int _isDisposedInt;

        private string Reason { get; set; }

        public ServerTaskTimes Interval { get; private set; }

        GameTime IServerTask.LastUpdate { get; set; }

        public ShutdownHandlerBase(int shutdownTime, string reason)
        {
            Reason = reason;
            ExpireTime = ServerMainBase.InternalInstance.CurrentTime.Time.Add(TimeSpan.FromSeconds(shutdownTime));
        }

        public abstract void FinallyShutdown();
        public abstract void ShutdownSequense_1Seconds(string reason, TimeSpan restTime);
        public abstract void ShutdownSequense_30Seconds(string reason, TimeSpan restTime);

        public abstract void ShutdownSequense_5Minutes(string reason, TimeSpan restTime);
        public abstract void ShutdownSequense_2Minutes(string reason, TimeSpan restTime);

        protected void UpdateTime(int seconds, string reason)
        {
            this.Reason = reason;
            ExpireTime = GameTime.Now().Time.AddSeconds(seconds);
        }

        bool IServerTask.Update(GameTime gameTime)
        {
            if (_isDisposedInt == 1) return false;

            var restTime = ExpireTime.Subtract(gameTime.Time);



            if (restTime.TotalHours <= 1 &&
                restTime.TotalHours > 1 &&
                restTime.TotalMinutes >= 20)
            {
                ShutdownSequense_5Minutes(Reason, restTime);

                Interval = (ServerTaskTimes)TimeSpan.FromMinutes(5).TotalMilliseconds;

                return true;
            }
            else if (restTime.TotalMinutes <= 20 &&
                    restTime.TotalSeconds > 30 &&
                    restTime.TotalMinutes > 2)
            {
                ShutdownSequense_2Minutes(Reason, restTime);

                Interval = (ServerTaskTimes)TimeSpan.FromMinutes(2).TotalMilliseconds;
                return true;
            }
            if (restTime.TotalMinutes <= 2 &&
                restTime.TotalSeconds > 30)
            {
                ShutdownSequense_30Seconds(Reason, restTime);

                Interval = (ServerTaskTimes)TimeSpan.FromSeconds(30).TotalMilliseconds;
                return true;
            }
            else if (restTime.TotalSeconds <= 30)
            {
                Interval = (ServerTaskTimes)TimeSpan.FromSeconds(1).TotalMilliseconds;

                if (restTime.TotalSeconds < 1)
                {
                    FinallyShutdown();
                    return false;
                }
                ShutdownSequense_1Seconds(Reason, restTime);

                return true;
            }

            return true;
        }


        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref _isDisposedInt, 1, 0) == 0)
            {
                DisposeInternal();
            }
        }

        protected void DisposeInternal()
        {
        }
    }
}