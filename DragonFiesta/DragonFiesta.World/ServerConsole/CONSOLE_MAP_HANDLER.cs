using DragonFiesta.Providers.Maps;
using DragonFiesta.World.Game.Maps;
using DragonFiesta.World.Game.Zone;
using DragonFiesta.World.InternNetwork.InternHandler.Server.Maps;
using System.Collections.Generic;

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
                if (Params.Length == 1 && ushort.TryParse(Params[0], out ushort MapId))
                {
                    if (!MapDataProvider.GetFieldInfosByMapID(MapId, out FieldInfo mInfo))
                    {
                        CommandLog.WriteConsoleLine(CommandLogLevel.Error, "No Map for ID : {0} found!", MapId);
                        return true;
                    }
                    else if (MapManager.GetMap(MapId, 0, out WorldServerMap Map) && mInfo.MapInfo.Type == MapType.Normal)
                    {
                        CommandLog.WriteConsoleLine(CommandLogLevel.Error, $"Map {MapId } Alredy Startet");
                        return true;
                    }
                    else if (!ZoneManager.GetZoneByID(mInfo.ZoneID, out ZoneServer Zone))
                    {
                        CommandLog.WriteConsoleLine(CommandLogLevel.Error, "ZoneServer for map {0} is not Online", MapId);
                        return true;
                    }
                    else
                    {

                        MapMethods.SendStartNewMap(mInfo.MapInfo, out ushort InstanceId);
                        return true;
                    }
                }
                else
                {
                    CommandLog.WriteConsoleLine(CommandLogLevel.Error, "Invalid Parameters use : Map Start <Mapid> ");
                }
                return true;
            }

            [ConsoleCommand("stop")]
            public static bool CMD_MAP_STOP(string[] Params)
            {
                if (Params.Length == 1 && ushort.TryParse(Params[0], out ushort MapId))
                {
                    if (!MapDataProvider.GetFieldInfosByMapID(MapId, out FieldInfo info))
                    {
                        CommandLog.WriteConsoleLine(CommandLogLevel.Error, "Can't find Map {0}", MapId);
                        return true;
                    }

                    if (!ZoneManager.GetZoneByID(info.ZoneID, out ZoneServer Zone))
                    {
                        CommandLog.WriteConsoleLine(CommandLogLevel.Error, "Zone for Map {0}", info.ZoneID);
                        return true;
                    }

                    if (!MapManager.StopMap(MapId))//hmm I need info ..
                    {
                        CommandLog.WriteConsoleLine(CommandLogLevel.Error, "Invalid Stop Map {0}", MapId);
                        return true;
                    }
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
                if (Params.Length == 1 && ushort.TryParse(Params[0], out ushort MapId))
                {
                    if (!MapManager.GetInstancesOfMapId(MapId, out List<InstanceMap> Maps))
                    {
                        CommandLog.Write(CommandLogLevel.Error, "No Instances of Map {0} found !", MapId);
                        return true;
                    }

                    Maps.ForEach(Map => CommandLog.Write(CommandLogLevel.Execute, "Instances {0} MapId {1}", Map.InstanceId, Map.MapId));

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