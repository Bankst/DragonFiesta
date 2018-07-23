#region

using System.Collections.Concurrent;
using System.Data;
using System.Data.SqlClient;
using DragonFiesta.Database.SQL;
using DragonFiesta.Providers.Items;

#endregion

namespace DragonFiesta.Game.Characters.Data
{
    public class CharacterInventory
    {
        public ConcurrentDictionary<uint, ItemBaseInfo> CharacterInventoryData { get; set; }

        public bool RefreshFromSQL(SQLResult pResult, int i)
        {
            CharacterInventoryData = new ConcurrentDictionary<uint, ItemBaseInfo>();

            var charID = pResult.Read<long>(i, "ID");

            using (var iResult = DB.Select(DatabaseType.World, "SELECT * FROM Items WHERE Owner = @pOwner",
                new SqlParameter("@pOwner", charID)))
            {
                foreach (DataRow item in iResult.Rows)
                {
                    var ItemKey = (long)item["ItemKey"];
                    //					var StorageType = (byte) item["StorageType"];
                    //					var Owner = (long) item["Owner"];
                    //					vart Storage = (short) item["Storage"];
                    //					var ItemID = (int) item["ItemID"];
                    //					var Flags = (byte) item["Flags"];

                    using (var ioResult = DB.Select(DatabaseType.World, "SELECT * FROM ItemOptions WHERE ItemKey = @pItemKey",
                        new SqlParameter("@pItemKey", ItemKey)))
                    {
                        // TODO: figure out ItemOption and OptionTypes
                    }

                }
            }

            return true;
        }



        ~CharacterInventory()
        {
            CharacterInventoryData = null;
        }
    }
}
