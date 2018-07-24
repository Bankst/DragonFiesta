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
		protected static ConcurrentDictionary<ushort, List<ItemUpgradeInfo>> UpgradeInfosByID;
		protected static ConcurrentDictionary<ushort, ItemBaseInfo> ItemBaseInfosByID;
		protected static SecureCollection<ItemInfo> ItemInfoSC;
		protected static SecureCollection<ItemInfoServer> ItemInfoServerSC;
		protected static SecureCollection<BelongTypeInfo> BelongTypeInfoSC;
	    protected static SecureCollection<GradeItemOption> GradeItemOptionSC;

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
                    var info = new ItemInfo(pResult, i);

                    if (!ItemInfoByID.TryAdd(info.ID, info))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, $"Duplicate item id found. ID: {info.ID}");
                        continue;
                    }
					ItemInfoSC.Add(info);
                    mBar.Step();
                }
				watch.Stop();
                DatabaseLog.WriteProgressBar($">> Loaded {ItemInfoSC.Count} rows in {(double)watch.ElapsedMilliseconds / 1000}s");
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
					var info = new ItemInfoServer(pResult, i);

					if (!ItemInfoServerByID.TryAdd(info.ID, info))
					{
						DatabaseLog.Write(DatabaseLogLevel.Warning, $"Duplicate item id found. ID: {info.ID}");
						continue;
					}
					ItemInfoServerSC.Add(info);
					mBar.Step();
				}
				watch.Stop();
				DatabaseLog.WriteProgressBar($">> Loaded {ItemInfoServerSC.Count} rows in {(double)watch.ElapsedMilliseconds / 1000}s");
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
					var info = new BelongTypeInfo(pResult, i);
					BelongTypeInfoSC.Add(info);
					mBar.Step();
				}
				watch.Stop();
				DatabaseLog.WriteProgressBar($">> Loaded {BelongTypeInfoSC.Count} rows in {(double)watch.ElapsedMilliseconds / 1000}s");
			}
		}

		public static void LoadUpgradeInfo()
		{
			var watch = Stopwatch.StartNew();
			UpgradeInfosByID = new ConcurrentDictionary<ushort, List<ItemUpgradeInfo>>();
			var pResult = SHNManager.Load(SHNType.UpgradeInfo);
			DatabaseLog.WriteProgressBar(">> Load ItemUpgradeInfo SHN");
			using (var mBar = new ProgressBar(pResult.Count))
			{
				for (var i = 0; i < pResult.Count; i++)
				{
					var info = new ItemUpgradeInfo(pResult, i);
					if (!UpgradeInfosByID.TryGetValue(info.ID, out var list))
					{
						list = new List<ItemUpgradeInfo>();
						UpgradeInfosByID.TryAdd(info.ID, list);
					}
					list.Add(info);
					mBar.Step();
				}
				watch.Stop();
				DatabaseLog.WriteProgressBar($">> Loaded {UpgradeInfosByID.Count} rows in {(double)watch.ElapsedMilliseconds / 1000}s");
			}
		}

	    public static void LoadGradeItemOption()
	    {
		    var watch = Stopwatch.StartNew();
			GradeItemOptionSC = new SecureCollection<GradeItemOption>();
		    var pResult = SHNManager.Load(SHNType.GradeItemOption);
			DatabaseLog.WriteProgressBar(">> Load GradeItemOption SHN");
		    using (var mBar = new ProgressBar(pResult.Count))
		    {
			    for (var i = 0; i < pResult.Count; i++)
			    {
					var info = new GradeItemOption(pResult, i);
				    GradeItemOptionSC.Add(info);
					mBar.Step();
			    }
			    watch.Stop();
				DatabaseLog.WriteProgressBar($">> Loaded {GradeItemOptionSC.Count} rows in {(double) watch.ElapsedMilliseconds / 1000}s");
		    }
	    }

	    public static void FillItemBaseInfos()
	    {
			var watch = Stopwatch.StartNew();
			ItemBaseInfosByID = new ConcurrentDictionary<ushort, ItemBaseInfo>();
			DatabaseLog.WriteProgressBar(">> Fill ItemBaseInfos from loaded SHNs");
			using (var mBar = new ProgressBar(ItemInfoSC.Count))
			{
				for (var i = 0; i < ItemInfoSC.Count; i++)
				{
					var itemInfo = ItemInfoSC.ElementAt(i);
					if (!GetUpgradeInfosByID(itemInfo.BasicUpInx, out var upgradeInfosList))
					{
						DatabaseLog.Write(DatabaseLogLevel.Debug, $"Bad UpgradeInfos for item ID: {itemInfo.ID}");
					}

					var btInfo = BelongTypeInfoSC.FirstOrDefault(x => x.BT_Inx == itemInfo.BT_Inx);
					if (btInfo == null)
					{
						DatabaseLog.Write(DatabaseLogLevel.Warning, $"Bad BelongTypeInfo for item ID: {itemInfo.ID}");
						continue;
					}

					var gioInfo = GradeItemOptionSC.FirstOrDefault(x => x.ItemIndex == itemInfo.InxName);
					if (gioInfo == null)
					{
						DatabaseLog.Write(DatabaseLogLevel.Debug,
							$"Bad or No GradeItemOption for item ID: {itemInfo.ID}");
					}
					var info = new ItemBaseInfo(itemInfo, upgradeInfosList, btInfo, gioInfo);
					if (!ItemBaseInfosByID.TryAdd(itemInfo.ID, info))
					{
						DatabaseLog.Write(DatabaseLogLevel.Warning,
							$"Create ItemBaseInfo failed for item ID: {itemInfo.ID}");
					}
					mBar.Step();
				}
				watch.Stop();
				DatabaseLog.WriteProgressBar($">> Filled {ItemBaseInfosByID.Count} rows in {(double)watch.ElapsedMilliseconds / 1000}s");
			}
		}

	    public static int GetItemIDByInxName(string inxName)
	    {
		    return ItemInfoSC.First(x => x.InxName == inxName).ID;
	    }

	    public static ItemBaseInfo GetItemBaseInfoByInxName(string inxName)
	    {
		    try
		    {
			    var id = GetItemIDByInxName(inxName);
			    return ItemBaseInfosByID.First(x => x.Key == id).Value;
		    }
		    catch
		    {
			    return null;
		    }
	    }

	    public static bool GetUpgradeInfosByID(ushort id, out List<ItemUpgradeInfo> list)
        {
            return UpgradeInfosByID.TryGetValue(id, out list);
        }
    }
}