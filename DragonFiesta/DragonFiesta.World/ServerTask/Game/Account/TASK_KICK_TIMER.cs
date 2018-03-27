using System;
using System.Threading;

namespace DragonFiesta.World.ServerTask.Accounts
{
    public class TASK_KICK_TIMER : IServerTask
    {
        public bool IsStartet { get; private set; }

        private int TimeToKick { get; set; }

        private Action OnKickCallBack { get; set; }

        private Action<int> OnTickCallBack { get; set; }


        public DateTime ExpireTime { get; private set; }

        public bool IsDisposed { get { return (IsDisposedInt > 0); } }

        ServerTaskTimes IServerTask.Interval => ServerTaskTimes.INGAME_TIMER_INTERVALL;

        GameTime IServerTask.LastUpdate { get; set; }

        private int IsDisposedInt;

        public TASK_KICK_TIMER(
            Action OnKickCallBack,
            Action<int> OnTickCallBack,
            int TimeToKick = 0)
        {
            this.OnKickCallBack = OnKickCallBack;
            this.OnTickCallBack = OnTickCallBack;

            ExpireTime = DateTime.Now.AddSeconds(TimeToKick);

        }

        bool IServerTask.Update(GameTime Now)
        {
            if (IsDisposed) return false;


            if (Now >= ExpireTime)
            {
                try
                {
                    OnKickCallBack.Invoke();
                    Dispose();
                }
                catch (Exception ex)
                {
                    EngineLog.Write(ex, "Failed to Excute KickTimer Function OnKickAss");
                }

                return false;
            }
            else
            {
                OnTickCallBack.Invoke((int)ExpireTime.Subtract(Now.Time).TotalSeconds);
                return true;
            }
        }

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref IsDisposedInt, 1, 0) == 0)
            {
                TimeToKick = 0;
                OnKickCallBack = null;
                OnTickCallBack = null;
            }
        }
    }
}
