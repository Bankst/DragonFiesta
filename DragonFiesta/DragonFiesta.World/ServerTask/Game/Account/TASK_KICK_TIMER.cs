using System;
using System.Threading;

namespace DragonFiesta.World.ServerTask.Accounts
{
    public class TASK_KICK_TIMER : IServerTask
    {
        public bool IsStarted { get; private set; }

        private int TimeToKick { get; set; }

        private Action OnKickCallBack { get; set; }

        private Action<int> OnTickCallBack { get; set; }


        public DateTime ExpireTime { get; private set; }

        public bool IsDisposed => (_isDisposedInt > 0);

	    ServerTaskTimes IServerTask.Interval => ServerTaskTimes.INGAME_TIMER_INTERVALL;

        GameTime IServerTask.LastUpdate { get; set; }

        private int _isDisposedInt;

        public TASK_KICK_TIMER(
            Action onKickCallBack,
            Action<int> onTickCallBack,
            int timeToKick = 0)
        {
            this.OnKickCallBack = onKickCallBack;
            this.OnTickCallBack = onTickCallBack;

            ExpireTime = DateTime.Now.AddSeconds(timeToKick);

        }

        bool IServerTask.Update(GameTime now)
        {
            if (IsDisposed) return false;


            if (now >= ExpireTime)
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
                OnTickCallBack.Invoke((int)ExpireTime.Subtract(now.Time).TotalSeconds);
                return true;
            }
        }

        public void Dispose()
        {
	        if (Interlocked.CompareExchange(ref _isDisposedInt, 1, 0) != 0) return;
	        TimeToKick = 0;
	        OnKickCallBack = null;
	        OnTickCallBack = null;
        }
    }
}
