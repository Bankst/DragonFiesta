using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Providers.Items
{
	[GameServerModule(ServerType.Zone, GameInitalStage.Item)]
	//[GameServerModule(ServerType.World, GameInitalStage.Item)]
	public class ItemDataProviderBase<ItemDataType>
        where ItemDataType : ItemInfoSHN
    {
        public const uint ExpireTime_NeverExpire = 1992027391;
        public const ushort ItemInfo_DefaultMiniHouse_ID = 31000;
        public static ItemInfoSHN ItemInfo_DefaultMiniHouse { get; protected set; }
        protected static ConcurrentDictionary<ushort, List<ItemUpgradeInfo>> UpgradeInfosByID;
        protected static ConcurrentDictionary<ushort, ItemDataType> ItemInfosByID;
		protected static SecureCollection<ItemDataType> ItemInfos;
		protected static SHNFile ItemInfoFile;

		[InitializerMethod]
		public static bool Initialize()
		{
			LoadSHN();
			LoadItemData();
			return true;
		}

		public static void LoadSHN()
		{
			MethodInfo CryptoMethod = typeof(SHNCrypto).GetMethods(BindingFlags.Static | BindingFlags.Public).Where(x => x.Name == "DefaultCrypto").First();

			ItemInfoFile = new SHNFile(String.Format("{0}ItemInfo.shn", "Shine/"), CryptoMethod);

			if (ItemInfoFile.Type == SHNType.TextData) { ItemInfoFile.SHNEncoding = Encoding.ASCII; }
			else { ItemInfoFile.SHNEncoding = Encoding.GetEncoding("ISO-8859-1"); }

			if (ItemInfoFile.Type != SHNType.QuestData) { ItemInfoFile.Read(); }
			else { ItemInfoFile.ReadQuest(); }

			ItemInfoFile.DisallowRowChanges();
			var LeatherBootsID = ItemInfos.Where(x => x.InxName == "LeatherBoots").First().ID;
		}

        public static void LoadItemData()
        {
            ItemInfosByID = new ConcurrentDictionary<ushort, ItemDataType>();
			//ItemInfoFile.Table
			SHNResult pResult = (SHNResult)ItemInfoFile.Table;
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
					ItemInfos.Add(info);
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