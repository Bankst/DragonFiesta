using DragonFiesta.Messages.Zone.Maps;
using DragonFiesta.Providers.Maps;
using DragonFiesta.Utils.Logging;
using DragonFiesta.World.Game.Maps;
using DragonFiesta.World.InternNetwork.InternHandler.Response.Zone.Map;
using DragonFiesta.World.InternNetwork.InternHandler.Server.Maps;

namespace DragonFiesta.World.InternNetwork.InternHandler.Client.Maps
{
    public class MapHandler
    {
        [InternMessageHandler(typeof(MapListRequest))]
        public static void HandleMapList(MapListRequest Respone, InternZoneSession pSession)
        {
            if (!pSession.Zone.IsReady)
            {
                MapListMethods.SendMapLists(pSession); // Now Remote Maps

                MapListMethods.SendMapStartZoneList(pSession.Zone.ID); //Start New Maps for new Zone :D


                pSession.Zone.IsReady = true;

                GameLog.Write(GameLogLevel.Internal, $"ZoneServer ID : {pSession.Zone.ID } is out of Maintenace!");

                pSession.SendMessage(Respone, false);
            }
        }

        [InternMessageHandler(typeof(StartMap_Response))]
        public static void HandleStartMap_Response(StartMap_Response Respone, InternZoneSession pSession)
        {
            if (Respone.Result != MapStartResult.OK)
            {
                MapMethods.SendStopMap(Respone.MapId, Respone.InstanceId);
                return;
            }


            if (!MapDataProvider.GetFieldInfosByMapID(Respone.MapId, out FieldInfo Info))
            {
                MapMethods.SendStopMap(Respone.MapId, Respone.InstanceId);
                return;
            }

            if (Info.ZoneID == pSession.Zone.ID)
            {
                Map_Response.StartNewMap_Response(Respone);
            }
        }

        [InternMessageHandler(typeof(StartMapList_Response))]
        public static void HandleStartMapList_Response(StartMapList_Response Respone, InternZoneSession pSession)
        {

            foreach (var Row in Respone.MapList)
            {
                if (MapManager.GetInstanceMapById(Row.InstanceId, out InstanceMap StopMap) && Row.Result != MapStartResult.OK)
                {
                    MapMethods.SendStopMap(Row.MapId, Row.InstanceId);
                    continue;
                }
                else if (MapManager.GetNormalMapById(Row.MapId, out NormalMap exiMap))
                {
                    continue;
                }

                if (Row.Result != MapStartResult.OK)
                {
                    MapMethods.SendStopMap(Row.MapId, Row.InstanceId);
                    continue;
                }

                Map_Response.StartNewMap_Response(Row);
            }
        }
    }
}