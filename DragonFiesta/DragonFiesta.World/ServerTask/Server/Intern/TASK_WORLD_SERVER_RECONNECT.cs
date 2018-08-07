using DragonFiesta.Utils.Logging;

namespace DragonFiesta.World.ServerTask.Intern
{
    public class TASK_WORLD_SERVER_RECONNECT :  IServerTask
    {
     
        public GameTime LastUpdate { get; set; }

        public ServerTaskTimes Interval => ServerTaskTimes.SERVER_WORLD_RECONNECT;

        public void Dispose()
        {
        }

        public TASK_WORLD_SERVER_RECONNECT()
        {
        }
        public bool Update(GameTime gameTime)
        {
            if (!InternLoginConnector.GetIsConnected())
            {
                ServerMain.InternalInstance.ServerIsReady = false;

                GameLog.Write(GameLogLevel.Internal, "LoginServer Connection Lose Reconnecting...");
                if (InternLoginConnector.Connect())
                {
                    GameLog.Write(GameLogLevel.Internal, "Reconnecting to LoginServer Success!");

                    return false; //leave Task
                }

                return true;
            }

            return true;
        }
    }
}