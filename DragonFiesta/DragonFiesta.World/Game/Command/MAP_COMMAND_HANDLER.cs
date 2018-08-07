using DragonFiesta.Providers.Maps;
using DragonFiesta.World.Game.Character;
using DragonFiesta.World.Game.Chat;
using DragonFiesta.World.InternNetwork.InternHandler.Server.Maps;

namespace DragonFiesta.World.Game.Command
{
    [GameCommandCategory("Map")]
    public class MAP_COMMAND_HANDLER
    {
        [WorldCommand("Start")]
        public static bool Map_Start(WorldCharacter Character, string[] Params)
        {

            if (Params.Length < 1)
            {
                ZoneChat.CharacterNote(Character, "Invalid Parameter use &map <MapId>");
                return true;
            }


            if (!ushort.TryParse(Params[0], out ushort MapId))
            {
                ZoneChat.CharacterNote(Character, "Please use MapId");
                return true;
            }

            if (!MapDataProvider.GetFieldInfosByMapID(MapId, out FieldInfo Info))
            {
                ZoneChat.CharacterNote(Character, $"MapId {MapId} not found!");
                return true;
            }

            MapMethods.SendStartNewMap(Info.MapInfo, out ushort InstanceId);

            return true;
        }

        [WorldCommand("Stop")]
        public static bool Map_Stop(WorldCharacter Character, string[] Params)
        {
            if (Params.Length < 1)
            {
                ZoneChat.CharacterNote(Character, "Invalid Parameter use &map <MapId>");
                return true;
            }


            if (!ushort.TryParse(Params[0], out ushort MapId))
            {
                ZoneChat.CharacterNote(Character, "Please use MapId");
                return true;
            }

            if (!MapDataProvider.GetFieldInfosByMapID(MapId, out FieldInfo Info))
            {
                ZoneChat.CharacterNote(Character, $"MapId {MapId} not found!");
                return true;
            }
   
          //  MapManager.GetMap(Info)
            return true;
        }

    }
}
