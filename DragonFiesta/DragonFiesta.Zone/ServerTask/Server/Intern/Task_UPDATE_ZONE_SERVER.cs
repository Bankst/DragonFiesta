using DragonFiesta.Messages.Zone.Zone;
using DragonFiesta.Zone.Config;
using DragonFiesta.Zone.Core;
using DragonFiesta.Zone.Network;
using System;

namespace DragonFiesta.Zone.ServerTask.Intern
{
    [ServerTaskClass]
    public class TASK_UPDATE_ZONE_SERVER : IServerTask
    {
        public ServerTaskTimes Interval => ServerTaskTimes.SERVER_ZONE_UPDATE;

        GameTime IServerTask.LastUpdate { get; set; }


        bool IServerTask.Update(GameTime gameTime)
        {
            if (InternWorldConnector.GetIsConnected() && ServerMain.InternalInstance.ServerIsReady)
            {
                InternWorldConnector.Instance.SendMessage(new UpdateZoneServer
                {
                    Id = Guid.NewGuid(),
                    CurrentConnection = ZoneSessionManager.Instance.CountOfSessions,
                    ZoneId = ZoneConfiguration.Instance.ZoneID,
                });
            }

            return true;
        }

        public void Dispose()
        {
        }
    }
}