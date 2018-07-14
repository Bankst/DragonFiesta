using DragonFiesta.World.Network;
using System;

namespace DragonFiesta.World.ServerTask.Intern
{
    [ServerTaskClass]
    public class TASK_UPDATE_WORLD_SERVER : IServerTask
    {
        ServerTaskTimes IServerTask.Interval => ServerTaskTimes.SERVER_WORLD_UPDATE;

        GameTime IServerTask.LastUpdate { get; set; }


        public void Dispose()
        {
        }

        bool IServerTask.Update(GameTime gameTime)
        {
            if (ServerMain.InternalInstance.ServerIsReady)
            {
                InternLoginConnector.Instance.SendMessage(new UpdateWorldServer
                {
                    Id = Guid.NewGuid(),
                    NowPlayerCount = WorldSessionManager.Instance.CountOfSessions,
                    WorldReady = ServerMain.InternalInstance.ServerIsReady,
                });
            }

            return true;
        }
    }
}