using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DragonFiesta.Providers.Items
{
    public class ItemDataProvider<ItemDataType>
        where ItemDataType : ItemBaseInfo
    {
        public const uint ExpireTime_NeverExpire = 1992027391;
        public const ushort ItemInfo_DefaultMiniHouse_ID = 31000;
        public static ItemBaseInfo ItemInfo_DefaultMiniHouse { get; protected set; }
        protected static ConcurrentDictionary<ushort, List<ItemUpgradeInfo>> UpgradeInfosByID;
        protected static ConcurrentDictionary<ushort, ItemDataType> ItemInfosByID;

        public static void LoadItemData()
        {
            ItemInfosByID = new ConcurrentDictionary<ushort, ItemDataType>();

            SQLResult pResult = DB.Select(DatabaseType.Data, "SELECT * FROM ItemInfos");
            DatabaseLog.WriteProgressBar(">> Load Item infos");
            using (ProgressBar mBar = new ProgressBar(pResult.Count))
            {
                for (int i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = (ItemDataType)Activator.CreateInstance(typeof(ItemDataType), pResult, i);

                    if (!ItemInfosByID.TryAdd(info.ID, info))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate item id found. ID: {0}", info.ID);
                        continue;
                    }
                    mBar.Step();
                }
                DatabaseLog.WriteProgressBar(">> Loaded {0} Item infos", ItemInfosByID.Count);
            }
            //get default minihouse

            if (!ItemInfosByID.TryGetValue(ItemInfo_DefaultMiniHouse_ID, out ItemDataType defaultMiniHouse))
                throw new InvalidOperationException(String.Format("Can't find 'Mushroom House' item (ID: {0}).", ItemInfo_DefaultMiniHouse_ID));

            ItemInfo_DefaultMiniHouse = defaultMiniHouse;
        }

        public static void LoadUpgradeInfos()
        {
            UpgradeInfosByID = new ConcurrentDictionary<ushort, List<ItemUpgradeInfo>>();

            SQLResult pResult = DB.Select(DatabaseType.Data, "SELECT * FROM UpgradeInfos");
            DatabaseLog.WriteProgressBar(">> Load upgrade infos");
            using (ProgressBar mBar = new ProgressBar(pResult.Count))
            {
                for (int i = 0; i < pResult.Count; i++)
                {
                    var info = new ItemUpgradeInfo(pResult, i);
                    if (!UpgradeInfosByID.TryGetValue(info.ID, out List<ItemUpgradeInfo> list))
                    {
                        list = new List<ItemUpgradeInfo>();
                        UpgradeInfosByID.TryAdd(info.ID, list);
                    }
                    list.Add(info);
                    mBar.Step();
                }
                DatabaseLog.WriteProgressBar(">> Loaded {0} upgrade infos", UpgradeInfosByID.Count);
            }
        }

        public static bool GetUpgradeInfosByID(ushort ID, out List<ItemUpgradeInfo> List)
        {
            return UpgradeInfosByID.TryGetValue(ID, out List);
        }
    }
}