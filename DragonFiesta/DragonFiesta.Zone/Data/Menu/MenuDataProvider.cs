using System.Collections.Concurrent;

namespace DragonFiesta.Zone.Data.Menu
{
    [GameServerModule(ServerType.Zone, GameInitalStage.Menu)]
    public static class MenutDataProvider
    {
        private static ConcurrentDictionary<uint, MenuDataInfo> MenusByID;

        [InitializerMethod]
        public static bool Initialize()
        {
            LoadMenuSQL();

            return true;
        }

        private static void LoadMenuSQL()
        {
            MenusByID = new ConcurrentDictionary<uint, MenuDataInfo>();

            SQLResult Result = DB.Select(DatabaseType.Data, "SELECT * FROM Menus");
            for (int i = 0; i < Result.Count; i++)
            {
                MenuDataInfo MenuData = new MenuDataInfo(Result, i);

                if (!MenusByID.TryAdd(MenuData.ID, MenuData))
                {
                    DatabaseLog.Write(DatabaseLogLevel.Warning, "Dublicate Menu Found Id {0}", MenuData.ID);
                    continue;
                }
            }
        }

        public static bool GetMenuByID(uint ID, out MenuDataInfo Value)
        {
            return MenusByID.TryGetValue(ID, out Value);
        }
    }
}