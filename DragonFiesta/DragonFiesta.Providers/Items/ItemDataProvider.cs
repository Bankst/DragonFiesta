using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DragonFiesta.Providers.Items.SHN;
using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Providers.Items
{
    public class ItemDataProviderBase
    {
	    public const ushort ItemInfoDefaultMiniHouseID = 31000;
        public static ItemInfo ItemInfoDefaultMiniHouse { get; protected set; }
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
			var watch = Stopwatch.StartNew();
			ItemInfoSC = new SecureCollection<ItemInfo>();
            ItemInfoByID = new ConcurrentDictionary<ushort, ItemInfo>();

			var pResult = SHNManager.Load(SHNType.ItemInfo);
            DatabaseLog.WriteProgressBar(">> Load ItemInfo");
            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
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

            if (!ItemInfoByID.TryGetValue(ItemInfoDefaultMiniHouseID, out var defaultMiniHouse))
            {
                throw new InvalidOperationException($"Can't find 'Mushroom House' item (ID: {ItemInfoDefaultMiniHouseID}).");
            }

            ItemInfoDefaultMiniHouse = defaultMiniHouse;
        }

		public static void LoadItemInfoServer()
		{
			var watch = Stopwatch.StartNew();
			ItemInfoServerSC = new SecureCollection<ItemInfoServer>();
			ItemInfoServerByID = new ConcurrentDictionary<ushort, ItemInfoServer>();

			var pResult = SHNManager.Load(SHNType.ItemInfoServer);
			DatabaseLog.WriteProgressBar(">> Load ItemInfoServer");
			using (var mBar = new ProgressBar(pResult.Count))
			{
				for (var i = 0; i < pResult.Count; i++)
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
			var watch = Stopwatch.StartNew();
			BelongTypeInfoSC = new SecureCollection<BelongTypeInfo>();

			var pResult = SHNManager.Load(SHNType.BelongTypeInfo);
			DatabaseLog.WriteProgressBar(">> Load BelongTypeInfo SHN");
			using (var mBar = new ProgressBar(pResult.Count))
			{
				for (var i = 0; i < pResult.Count; i++)
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
			var watch = Stopwatch.StartNew();
			UpgradeInfoSC = new SecureCollection<List<UpgradeInfo>>();
			UpgradeInfosByID = new ConcurrentDictionary<ushort, List<UpgradeInfo>>();
			var pResult = SHNManager.Load(SHNType.UpgradeInfo);
			DatabaseLog.WriteProgressBar(">> Load UpgradeInfo SHN");
			using (var mBar = new ProgressBar(pResult.Count))
			{
				for (var i = 0; i < pResult.Count; i++)
				{
					var info = new UpgradeInfo(pResult, i);
					if (!UpgradeInfosByID.TryGetValue(info.ID, out var list))
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
			for (var i = 0; i < ItemInfoSC.Count; i++)
			{
				var item = ItemInfoSC.ElementAt(i);
				var info = (ItemBaseInfo)Activator.CreateInstance(typeof(ItemBaseInfo), item, BelongTypeInfoSC);
				if (!ItemBaseInfosByID.TryAdd(item.ID, info))
				{
					// something happened yo
				}
			}
		}
		
        public static bool GetUpgradeInfosByID(ushort id, out List<UpgradeInfo> list)
        {
            return UpgradeInfosByID.TryGetValue(id, out list);
        }
    }
}