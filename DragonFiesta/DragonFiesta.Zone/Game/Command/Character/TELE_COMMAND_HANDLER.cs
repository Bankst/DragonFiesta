using DragonFiesta.Zone.Game.Character;
using DragonFiesta.Zone.Game.Chat;
using DragonFiesta.Zone.Game.Command.Data;
using System.Collections.Concurrent;

namespace DragonFiesta.Zone.Game.Command
{
    [GameServerModule(ServerType.Zone, GameInitalStage.Command)]
    [GameCommandCategory("tele")]
    public class TELE_COMMAND_HANDLER
    {
        #region Data
        private static ConcurrentDictionary<string, TeleLocation> TeleLocation;

        [InitializerMethod]
        public static bool InitalCommand()
        {
            LoadTeleLocation();
            return true;
        }

        private static void LoadTeleLocation()
        {
            TeleLocation = new ConcurrentDictionary<string, TeleLocation>();

            SQLResult pResult = DB.Select(DatabaseType.Data, "SELECT * FROM TeleLocations");

            DatabaseLog.WriteProgressBar(">> Load TeleLocations");

            using (ProgressBar mBar = new ProgressBar(pResult.Count))
            {
                for (int i = 0; i < pResult.Count; i++)
                {
                    var info = new TeleLocation(pResult, i);


                    mBar.Step();

                    if (!TeleLocation.TryAdd(info.LocationName, info))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate Telelocation found " + info.LocationName);
                        continue;
                    }
                }

                DatabaseLog.WriteProgressBar(">> Loaded {0} TeleLocations", TeleLocation.Count);
            }
        }
        #endregion

        [ZoneCommand("remove")]
        public static bool Teleport_remove(ZoneCharacter Sender, string[] Parameters)
        {
            if (Parameters.Length < 1)
            {
                ZoneChat.CharacterNote(Sender, "Invalid Parameters use &tele remove <locationname>");
                return true;
            }

            if (!TeleLocation.TryRemove(Parameters[0], out TeleLocation Location))
            {
                ZoneChat.CharacterNote(Sender, "Locations not found!");
                return true;
            }


            DB.RunSQL(DatabaseType.Data, $"DELETE FROM TeleLocations WHERE LocationName='{Location.LocationName}'");

            ZoneChat.CharacterNote(Sender, $"Location {Location.LocationName} Removed!");

            return true;
        }

        [ZoneCommand("Add")]
        public static bool Teleport_add(ZoneCharacter Sender, string[] Parameters)
        {
            if (Parameters.Length < 1)
            {
                ZoneChat.CharacterNote(Sender, "Invalid Parameters use &tele add <locationname>");
                return true;
            }

           
        

            TeleLocation NewLocation = new TeleLocation(Sender.AreaInfo.MapInfo)
            {
                LocationName = Parameters[0].ToUpper(),
                Position = Sender.Position,
            };

            if (!TeleLocation.TryAdd(NewLocation.LocationName,NewLocation))
            {
                ZoneChat.CharacterNote(Sender, "LocationName Already Exist");
                return true;
            }

            //Updateting into database
            DB.RunSQL(DatabaseType.Data, "INSERT INTO TeleLocations (LocationName,Map,PositionX,PositionY,Rotation)" +
            "VALUES" +
            $"('{NewLocation.LocationName}'," +
            $"'{NewLocation.MapInfo.ID}'," +
            $"'{NewLocation.Position.X}'," +
            $"'{NewLocation.Position.Y}'," +
            $"'{NewLocation.Position.Rotation}')");

            ZoneChat.CharacterNote(Sender, $"Addding Location {NewLocation.LocationName} Success!");

            return true;
        }

        [ZoneCommand("me")]
        public static bool Teleport(ZoneCharacter Sender, string[] Parameters)
        {

            if(Parameters.Length < 1)
            {
                ZoneChat.CharacterNote(Sender, "Invalid Parameters use &tele me <locationname>");
                return true;
            }

            if(!TeleLocation.TryGetValue(Parameters[0].ToUpper(),out TeleLocation Location))
            {
                ZoneChat.CharacterNote(Sender, "Location not found!");
                return true;
            }

            if (Location.MapInfo.Type != MapType.Normal)
            {
                if (Parameters.Length < 2)
                {
                    ZoneChat.CharacterNote(Sender, $"Is not normal Map Need Instance Id");
                    return true;
                }

                if (!ushort.TryParse(Parameters[1],out ushort InstanceId))
                {
                    ZoneChat.CharacterNote(Sender, "Invalid Parameter use &tele me <name> <instanceid>");
                    return true;
                }

                Sender.ChangeMap(
                    Location.MapInfo.ID,
                    InstanceId,
                    Location.Position.X,
                    Location.Position.Y);


                return true;

            }
            else
            {
                Sender.ChangeMap(
               Location.MapInfo.ID,
               0,
               Location.Position.X,
               Location.Position.Y);

                return true;
            }
        }
    }
}
