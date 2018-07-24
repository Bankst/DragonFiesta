using DragonFiesta.World.Config;
using DragonFiesta.World.Game.Settings;
using System;
using System.Data.SqlClient;
using DragonFiesta.Database.SQL;

namespace DragonFiesta.World.Data.Settings
{
    [GameServerModule(ServerType.World, GameInitalStage.GameSettings)]
    public class GameSettingDataProvider
    {
        public static CharacterKeyMap DefaultKeyMap { get; private set; }

        private static GameSettingDataProvider Instance { get; set; }


        [InitializerMethod]
        public static bool InitialGameSettingsProvider()
        {
            Instance = new GameSettingDataProvider();
            LoadKeyMap();
            return true;
        }


        private static void LoadKeyMap()
        {
            try
            {
                SQLResult KeyMapResult = DB.Select(
                    DatabaseType.Data,
                    "SELECT * FROM DefaultKeyMaps Where ClientRegion=@pRegion",
                    new SqlParameter("@pRegion",
                    (byte)WorldConfiguration.Instance.ServerRegion));

                DefaultKeyMap = new CharacterKeyMap();

                for (int i = 0; i < KeyMapResult.Count; i++)
                {
                    var Key = new CharacterKeyMapOptions(KeyMapResult, i);

                    if (!DefaultKeyMap.AddKeyOptions(Key))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Invalid Key for CharacterKeyMapping found KeyType {0}", Key.KeyType);
                    }
                }

                if (DefaultKeyMap.KeyMapCount > CharacterKeyMap.MaxKeyMapSize)
                    throw new StartupException("Invalid DefaultKeyMap in Database found! Max Key Reached");
            }
            catch (Exception ex)
            {
                DatabaseLog.Write(ex, "Invalid Load DefaultKeyMap from Database");
            }
        }
    }
}
