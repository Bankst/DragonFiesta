using System.Collections.Concurrent;
using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Zone.Data.Menu
{
    [GameServerModule(ServerType.Zone, GameInitalStage.Menu)]
    public static class MenutDataProvider
    {
        private static ConcurrentDictionary<uint, MenuDataInfo> _menusByID;

        [InitializerMethod]
        public static bool Initialize()
        {
	        LoadMenuSql();
            return true;
        }

		

		// totally wrong... shit...
	    private static void LoadMenu()
	    {
			_menusByID = new ConcurrentDictionary<uint, MenuDataInfo>();

		    var pResult = SHNManager.Load(SHNType.NpcDialogData);
		    for (var i = 0; i < pResult.Count; i++)
		    {
				var menuData = new MenuDataInfo(pResult, i);
			    if (!_menusByID.TryAdd(menuData.ID, menuData))
			    {
				    DatabaseLog.Write(DatabaseLogLevel.Warning, $"Duplicate Menu Found. ID: {menuData.ID}");
			    }
		    }

	    }

        private static void LoadMenuSql()
        {
            _menusByID = new ConcurrentDictionary<uint, MenuDataInfo>();

            var result = DB.Select(DatabaseType.Data, "SELECT * FROM Menus");
            for (var i = 0; i < result.Count; i++)
            {
                var menuData = new MenuDataInfo(result, i);
	            if (!_menusByID.TryAdd(menuData.ID, menuData))
	            {
		            DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate Menu Found Id {0}", menuData.ID);
	            }
            }
        }

        public static bool GetMenuByID(uint id, out MenuDataInfo value)
        {
            return _menusByID.TryGetValue(id, out value);
        }
    }
}