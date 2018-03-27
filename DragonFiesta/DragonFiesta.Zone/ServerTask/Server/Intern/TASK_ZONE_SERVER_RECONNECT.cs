using DragonFiesta.Zone.Core;

namespace DragonFiesta.Zone.ServerTask.Intern
{
    public class TASK_ZONE_SERVER_RECONNECT : IServerTask
    {

        GameTime IServerTask.LastUpdate { get; set; }

        ServerTaskTimes IServerTask.Interval => ServerTaskTimes.SERVER_ZONE_RECONNECT;

        public TASK_ZONE_SERVER_RECONNECT()
        {
        }


        public void Dispose()
        {

        }

        public bool Update(GameTime Now)
        {
            if (!InternWorldConnector.IsConnectet())
            {
                ServerMain.InternalInstance.ServerIsReady = false;

                GameLog.Write(GameLogLevel.Internal, "WorldServer Connection Lose Reconnecting...");
                if (InternWorldConnector.Connect())
                {
                    if (InternWorldConnector.IsConnectet())
                    {

                        GameLog.Write(GameLogLevel.Internal, "Reconnecting to WorldServer Success!");
                        return false; //leave Task
                    }
                }

                return true;
            }

            return true;
        }
    }
}