using DragonFiesta.Messages.Zone.Maps;
using DragonFiesta.Utils.Logging;
using DragonFiesta.World.Game.Maps;
using DragonFiesta.World.InternNetwork.InternHandler.Server.Maps;

namespace DragonFiesta.World.InternNetwork.InternHandler.Response.Zone.Map
{
    public static class Map_Response
    {
        public static void StartNewMap_Response(IMessage msg)
        {
            if (msg is StartMap_Response mResponse)
            {
                if (!MapManager.StartMap(mResponse.MapId, mResponse.InstanceId, out WorldServerMap Map))
                {
                    GameLog.Write(GameLogLevel.Warning, "Failed to Start Map {0} {1}", mResponse.MapId, mResponse.InstanceId);
                    MapMethods.SendStopMap(mResponse.MapId, mResponse.InstanceId);
                    return;
                }
                GameLog.Write(GameLogLevel.Debug, $"Start New Map : {mResponse.MapId } Instance : {mResponse.InstanceId }");
            }
        }
     


        public static void StartNewInstanceMapTimeout(IMessage msg) => MapManager.AvailablInstanceIds.Enqueue((msg as StartMap).InstanceId);
    }
}