using DragonFiesta.Messages.Zone.Maps;
using DragonFiesta.Providers.Maps;
using DragonFiesta.Zone.Game.Maps.Object;
using System;
using System.Collections.Generic;
using DragonFiesta.Utils.Logging;

namespace DragonFiesta.Zone.InternNetwork.InternHandler.Maps
{
    public class MapHandler
    {
        [InternMessageHandler(typeof(StartMap))]
        public static void HandleStartMap(StartMap MapMessage, InternWorldConnector pSession)
        {
            MapStartResult Result = MapStartResult.OK;

            if (!MapDataProvider.GetFieldInfosByMapID(MapMessage.MapId, out FieldInfo mInfo))
            {
                Result = MapStartResult.InvalidData;

            }
            else if (!MapManager.StartNewMap(mInfo, MapMessage.InstanceId))
            {

                Result = MapStartResult.FailedStart;
            }

            StartMap_Response Response = new StartMap_Response()
            {

                Id = Guid.NewGuid(),
                InstanceId = MapMessage.InstanceId,
                MapId = mInfo.MapInfo.ID,
                Result = Result,
            };

            pSession.SendMessage(Response);
        }

        [InternMessageHandler(typeof(StopMap))]
        public static void HandleStopMap(StopMap MapMessage, InternWorldConnector pSession)
        {
            if(!MapManager.StopMap(MapMessage.MapId,MapMessage.InstanceId))
            {
                GameLog.Write(GameLogLevel.Warning, "Failed to Stop Map {0} Instance {1}", MapMessage.MapId, MapMessage.InstanceId);
                return;
            }

        }

        [InternMessageHandler(typeof(StopMapList))]
        public static void HandleStopMapList(StopMapList MapMessage, InternWorldConnector pSession)
        {
            foreach (var Row in MapMessage.MapsList)
            {
                HandleStopMap(Row, pSession);
            }
        }


        [InternMessageHandler(typeof(StartMapList))]
        public static void HandleStartMapList(StartMapList MapMessage, InternWorldConnector pSession)
        {
            StartMapList_Response mResponse = new StartMapList_Response()
            {
                Id = Guid.NewGuid(),
                MapList = new List<StartMap_Response>(),
            };

            foreach (var MapResponse in MapMessage.MapsList)
            {
                MapStartResult Result = MapStartResult.None;

                if (!MapDataProvider.GetFieldInfosByMapID(MapResponse.MapId, out FieldInfo mInfo))
                {
                    Result = MapStartResult.InvalidData;
                }

                if (MapManager.StartNewMap(mInfo, MapResponse.InstanceId))
                {
                    Result = MapStartResult.OK;
                }
                else
                {
                    Result = MapStartResult.FailedStart;
                }

                mResponse.MapList.Add(new StartMap_Response
                {
                    InstanceId = MapResponse.InstanceId,
                    Result = Result,
                    MapId = MapResponse.MapId,
                });
            }

            pSession.SendMessage(mResponse);
        }
    }
}