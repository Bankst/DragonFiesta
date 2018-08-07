using System;

namespace DragonFiesta.Utils.Module.Server
{
    [ServerTaskClass]
    public class Task_Server_GC : IServerTask
    {
        public ServerTaskTimes Interval => ServerTaskTimes.SERVER_GC_INTERVAL;

        GameTime IServerTask.LastUpdate { get; set; }

        public void Dispose()
        {
        }

        public bool Update(GameTime gameTime)
        {
            GC.Collect();
            return true;
        }
    }
}