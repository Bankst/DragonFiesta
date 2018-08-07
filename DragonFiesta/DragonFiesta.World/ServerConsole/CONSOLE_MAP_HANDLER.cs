using DragonFiesta.Providers.Maps;
using DragonFiesta.World.Game.Maps;
using DragonFiesta.World.Game.Zone;
using DragonFiesta.World.InternNetwork.InternHandler.Server.Maps;

namespace DragonFiesta.World.ServerConsole
{
    public class CONSOLE_MAP_HANDLER
    {
        [ConsoleCommandCategory("Map")]
        public sealed class CONSOLE_ACCOUNT_HANDLER
        {
            //Account ban 1 Name
            [ConsoleCommand("start")]
            public static bool CMD_MAP_START(string[] Params)
            {
                if (Params.Length == 1 && ushort.TryParse(Params[0], out var mapId))
                {
                    if (!MapDataProvider.GetFieldInfosByMapID(mapId, out var mInfo))
                    {
                        CommandLog.WriteConsoleLine(CommandLogLevel.Error, $"No Map for ID : {mapId} found!");
                        return true;
                    }

	                if (MapManager.GetMap(mapId, 0, out var map) && mInfo.MapInfo.Type == MapType.Normal)
	                {
		                CommandLog.WriteConsoleLine(CommandLogLevel.Error, $"Map {mapId} Already Started");
		                return true;
	                }
	                if (!ZoneManager.GetZoneByID(mInfo.ZoneID, out var zone))
	                {
		                CommandLog.WriteConsoleLine(CommandLogLevel.Error, $"ZoneServer for map {mapId} is not Online");
		                return true;
	                }

	                MapMethods.SendStartNewMap(mInfo.MapInfo, out var instanceId);
	                return true;
                }

	            CommandLog.WriteConsoleLine(CommandLogLevel.Error, "Invalid Parameters use : Map Start <Mapid> ");
	            return true;
            }

            [ConsoleCommand("stop")]
            public static bool CMD_MAP_STOP(string[] Params)
            {
                if (Params.Length == 1 && ushort.TryParse(Params[0], out var mapId))
                {
                    if (!MapDataProvider.GetFieldInfosByMapID(mapId, out var info))
                    {
                        CommandLog.WriteConsoleLine(CommandLogLevel.Error, $"Can't find Map {mapId}");
                        return true;
                    }

                    if (!ZoneManager.GetZoneByID(info.ZoneID, out var zone))
                    {
                        CommandLog.WriteConsoleLine(CommandLogLevel.Error, $"Zone for Map {info.ZoneID}");
                        return true;
                    }

	                if (MapManager.StopMap(mapId)) return true; //hmm I need info ..
					CommandLog.WriteConsoleLine(CommandLogLevel.Error, $"Invalid Stop Map {mapId}");
	                return true;
                }
                else
                {
                    CommandLog.WriteConsoleLine(CommandLogLevel.InvalidParameters, "Invalid Parameters use : Map stop <Mapid> ");
                }

                return true;
            }

            [ConsoleCommand("Instances")]
            public static bool CMD_MAP_INSTACES(string[] Params)
            {
                if (Params.Length == 1 && ushort.TryParse(Params[0], out var mapId))
                {
                    if (!MapManager.GetInstancesOfMapId(mapId, out var maps))
                    {
                        CommandLog.Write(CommandLogLevel.Error, $"No Instances of Map {mapId} found !");
                        return true;
                    }

                    maps.ForEach(map => CommandLog.Write(CommandLogLevel.Execute, $"Instances {map.InstanceId} MapId {map.MapId}"));


					return true;
                }
                else
                {
                    CommandLog.Write(CommandLogLevel.Error, "Invalid Command Command Use Map Instances <MapId>");
                    return true;
                }
            }
        }
    }
}