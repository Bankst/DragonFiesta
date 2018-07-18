using DragonFiesta.World.Network;
using DragonFiesta.World.Network.FiestaHandler.Server;

namespace DragonFiesta.World.ServerTask.Game.Clock
{
    [ServerTaskClass]
    public sealed class TASK_CLOCK_GAMETIME_SYNC : IServerTask
    {
        public ServerTaskTimes Interval => ServerTaskTimes.SESSION_CLOCK_GAMETIME_SYNC;

        GameTime IServerTask.LastUpdate { get; set; }

        public void Dispose()
        {
           
        }

        public bool Update(GameTime gameTime)
        {
            WorldSessionManager.Instance.ClientAction((session) 
                => SH02Handler.SendGameTimeUpdatePacket(session, gameTime.Time),(m => m.Ingame));

            return true;
        }
    }
}
