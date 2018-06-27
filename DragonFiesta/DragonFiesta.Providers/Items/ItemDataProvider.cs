using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DragonFiesta.Providers.Items.SHN;
using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Providers.Items
{
    public class ItemDataProviderBase
    {
        public const uint ExpireTime_NeverExpire = 1992027391;
        public const ushort ItemInfo_DefaultMiniHouse_ID = 31000;
        public static ItemInfo ItemInfo_DefaultMiniHouse { get; protected set; }
        protected static ConcurrentDictionary<ushort, ItemInfo> ItemInfoByID;
		protected static ConcurrentDictionary<ushort, ItemInfoServer> ItemInfoServerByID;
		protected static ConcurrentDictionary<ushort, List<UpgradeInfo>> UpgradeInfosByID;
		protected static ConcurrentDictionary<ushort, List<ItemUpgradeInfo>> ItemUpgradeInfosByID;
		protected static ConcurrentDictionary<ushort, ItemBaseInfo> ItemBaseInfosByID;
		protected static SecureCollection<ItemInfo> ItemInfoSC;
		protected static SecureCollection<ItemInfoServer> ItemInfoServerSC;
		protected static SecureCollection<BelongTypeInfo> BelongTypeInfoSC;
		protected static SecureCollection<List<UpgradeInfo>> UpgradeInfoSC;

		public static void LoadItemInfo()
        {
			var watch = System.Diagnostics.Stopwatch.StartNew();
			ItemInfoSC = new SecureCollection<ItemInfo>();
            ItemInfoByID = new ConcurrentDictionary<ushort, ItemInfo>();

			SHNResult pResult = SHNManager.Load(SHNType.ItemInfo);
            DatabaseLog.WriteProgressBar(">> Load ItemInfo");
            using (ProgressBar mBar = new ProgressBar(pResult.Count))
            {
                for (int i = 0; i < pResult.Count; i++)
                {
                    //using activator...
                    var info = (ItemInfo)Activator.CreateInstance(typeof(ItemInfo), pResult, i);

                    if (!ItemInfoByID.TryAdd(info.ID, info))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, $"Duplicate item id found. ID: {info.ID}");
                        continue;
                    }
					ItemInfoSC.Add(info);
                    mBar.Step();
                }
				watch.Stop();
                DatabaseLog.WriteProgressBar($">> Loaded {ItemInfoSC.Count} ItemInfo rows in {(double)watch.ElapsedMilliseconds / 1000} sec");
            }
            //get default minihouse

            if (!ItemInfoByID.TryGetValue(ItemInfo_DefaultMiniHouse_ID, out ItemInfo defaultMiniHouse))
            {
                throw new InvalidOperationException($"Can't find 'Mushroom House' item (ID: {ItemInfo_DefaultMiniHouse_ID}).");
            }

            ItemInfo_DefaultMiniHouse = defaultMiniHouse;
        }

		public static void LoadItemInfoServer()
		{
			var watch = System.Diagnostics.Stopwatch.StartNew();
			ItemInfoServerSC = new SecureCollection<ItemInfoServer>();
			ItemInfoServerByID = new ConcurrentDictionary<ushort, ItemInfoServer>();

			SHNResult pResult = SHNManager.Load(SHNType.ItemInfoServer);
			DatabaseLog.WriteProgressBar(">> Load ItemInfoServer");
			using (ProgressBar mBar = new ProgressBar(pResult.Count))
			{
				for (int i = 0; i < pResult.Count; i++)
				{
					//using activator...
					var info = (ItemInfoServer)Activator.CreateInstance(typeof(ItemInfoServer), pResult, i);

					if (!ItemInfoServerByID.TryAdd(info.ID, info))
					{
						DatabaseLog.Write(DatabaseLogLevel.Warning, $"Duplicate item id found. ID: {info.ID}");
						continue;
					}
					ItemInfoServerSC.Add(info);
					mBar.Step();
				}
				watch.Stop();
				DatabaseLog.WriteProgressBar($">> Loaded {ItemInfoServerSC.Count} ItemInfoServer rows in {(double)watch.ElapsedMilliseconds / 1000} sec");
			}
		}

		public static void LoadBelongTypeInfo()
		{
			var watch = System.Diagnostics.Stopwatch.StartNew();
			BelongTypeInfoSC = new SecureCollection<BelongTypeInfo>();

			SHNResult pResult = SHNManager.Load(SHNType.BelongTypeInfo);
			DatabaseLog.WriteProgressBar(">> Load BelongTypeInfo SHN");
			using (ProgressBar mBar = new ProgressBar(pResult.Count))
			{
				for (int i = 0; i < pResult.Count; i++)
				{
					//using activator...
					var info = (BelongTypeInfo)Activator.CreateInstance(typeof(BelongTypeInfo), pResult, i);
					BelongTypeInfoSC.Add(info);
					mBar.Step();
				}
				watch.Stop();
				DatabaseLog.WriteProgressBar($">> Loaded {BelongTypeInfoSC.Count} BelongTypeInfo rows in {(double)watch.ElapsedMilliseconds / 1000} sec");
			}
		}

		public static void LoadUpgradeInfo()
		{
			var watch = System.Diagnostics.Stopwatch.StartNew();
			UpgradeInfoSC = new SecureCollection<List<UpgradeInfo>>();
			UpgradeInfosByID = new ConcurrentDictionary<ushort, List<UpgradeInfo>>();
			SHNResult pResult = SHNManager.Load(SHNType.UpgradeInfo);
			DatabaseLog.WriteProgressBar(">> Load UpgradeInfo SHN");
			using (ProgressBar mBar = new ProgressBar(pResult.Count))
			{
				for (int i = 0; i < pResult.Count; i++)
				{
					var info = new UpgradeInfo(pResult, i);
					if (!UpgradeInfosByID.TryGetValue(info.ID, out List<UpgradeInfo> list))
					{
						list = new List<UpgradeInfo>();
						UpgradeInfosByID.TryAdd(info.ID, list);
						UpgradeInfoSC.Add(list);
					}
					list.Add(info);
					mBar.Step();
				}
				watch.Stop();
				DatabaseLog.WriteProgressBar($">> Loaded {UpgradeInfoSC.Count} UpgradeInfo rows in {(double)watch.ElapsedMilliseconds / 1000} sec");
			}
		}

		/*

		public static void FillItemUpgradeInfos()
		{
			ItemUpgradeInfosByID = new ConcurrentDictionary<ushort, List<ItemUpgradeInfo>>();

			foreach (var upInfo in UpgradeInfosByID)
			{
				try
				{
					var info = new ItemUpgradeInfo(upInfo);
					if (!ItemUpgradeInfosByID.TryGetValue(info.ID, out List<ItemUpgradeInfo> list))
					{
						list = new List<ItemUpgradeInfo>();
						ItemUpgradeInfosByID.TryAdd(info.ID, list);
					}
					list.Add(info);
				}
				catch (Exception ex)
				{
					DatabaseLog.Write(DatabaseLogLevel.Warning, $"Error parsing UpgradeInfos ID: {i}");
				}
			}

			for (int i = 0; i <UpgradeInfoSC.Count; i++)
			{
				
				
			}
		}

		*/

		public static void FillItemBaseInfos()
		{
			ItemBaseInfosByID = new ConcurrentDictionary<ushort, ItemBaseInfo>();
			for (int i = 0; i < ItemInfoSC.Count; i++)
			{
				var item = ItemInfoSC.ElementAt(i);
				var info = (ItemBaseInfo)Activator.CreateInstance(typeof(ItemBaseInfo), item, BelongTypeInfoSC);
				if (!ItemBaseInfosByID.TryAdd(item.ID, info))
				{
					// something happened yo
					continue;
				}
			}
		}
		
        public static bool GetUpgradeInfosByID(ushort ID, out List<UpgradeInfo> List)
        {
            return UpgradeInfosByID.TryGetValue(ID, out List);
        }
    }
}