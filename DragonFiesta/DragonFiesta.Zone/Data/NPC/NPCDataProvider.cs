using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using DragonFiesta.Database.SQL;
using DragonFiesta.Providers.Maps;
using DragonFiesta.Utils.IO.TXT;
using DragonFiesta.Zone.Data.Mob;

namespace DragonFiesta.Zone.Data.NPC
{
	[GameServerModule(ServerType.Zone, GameInitalStage.NPC)]
    public class NPCDataProvider
    {
        private static ConcurrentDictionary<ushort, List<NPCInfo>> _npcDataByMapId;

        [InitializerMethod]
        public static bool InitialNPCDataProvider()
        {
            LoadNPCInfos();
            return true;
        }

        private static void LoadNPCInfos()
        {
            _npcDataByMapId = new ConcurrentDictionary<ushort, List<NPCInfo>>();

	        var pResult = DB.Select(DatabaseType.Data, "SELECT * FROM NPCTable");

            for (var i = 0; i < pResult.Count; i++)
            {
                var mobID = pResult.Read<ushort>(i, "MobID");
                var mapID = pResult.Read<ushort>(i, "MapID");
				if (!MapDataProvider.GetMapInfoByID(mapID, out var minf))
				{
					DatabaseLog.Write(DatabaseLogLevel.Warning, "Error loading MapInfo for NPC '{0}' {1}.", mobID, mapID);
				}
				else if (!MobDataProvider.GetMobInfoByID(mobID, out var mobinf))
				{
					DatabaseLog.Write(DatabaseLogLevel.Warning, "Can't find MobInfo for NPC. Mob ID: " + mobID);
				}
				else
				{
					var currentInfo = new NPCInfo(pResult, i);
					if (!_npcDataByMapId.TryGetValue(currentInfo.MapInfo.ID, out var list))
					{
						list = new List<NPCInfo>();
						_npcDataByMapId.TryAdd(currentInfo.MapInfo.ID, list);
					}

					if (currentInfo.Role == NPCRole.Gate || currentInfo.Role == NPCRole.IDGate)
					{
						var pgate = DB.Select(DatabaseType.Data, "SELECT * FROM LinkTable WHERE ID='" + (ushort)currentInfo.RoleArgument + "'");
						if (pgate.Count == 1)
						{
							for (var i2 = 0; i2 < pgate.Count; i2++)
							{
								currentInfo.LinkTable = new LinkTableInfo(pgate, i2);
							}
						}
						else if (pgate.Count > 1)
						{
							DatabaseLog.Write(DatabaseLogLevel.Warning, "Error Found Duplicate Link for NPC '{0}'.", currentInfo.MobInfo.ID);
							continue;
						}
						else if (pgate.Count == 0)
						{
							DatabaseLog.Write(DatabaseLogLevel.Warning, "Found No link for NPC '{0}'.", currentInfo.MobInfo.ID);
							continue;
						}
					}
					else if (currentInfo.Role == NPCRole.Merchant)
					{
						/*
						if (!LoadItemList(currentInfo, out int incrItemCount))
						{
						    DatabaseLog.Write(DatabaseLogLevel.Warning, "Error loading item list for NPC '{0}'-'{1}'.", currentInfo.MobInfo.ID, currentInfo.MobInfo.Index);
						    continue;
						}
						*/
						//itemCount += incrItemCount;
					}
					list.Add(currentInfo);
				}
			}
			//DatabaseLog.Write(DatabaseLogLevel.Startup, "Loaded {0} NPCItemLists with {1} items in shop", 420, 69);
        }
		 
		private static bool LoadItemList(NPCInfo currentInfo, out int incrItemCount)
		{
			incrItemCount = 0;
			var itemList = new List<NPCItem>();

			// open NPCItemList for currentInfo
			var shinePath = "Shine/NPCItemList/";
			var filePath = $"{shinePath}{currentInfo.MobInfo.Index}.txt";
			
			//itemListShine.
			 
			// parse
			try
			{
				var itemListShine = new ShineTable(filePath);
				foreach (var kvp in itemListShine.Tables)
				{
					//byte tabNum = Convert.ToByte(kvp.Key.Substring(3,2));
					var slotNo = 0;
					foreach (DataRow row in kvp.Source.Rows)
					{
							//byte rowNum = Convert.ToByte(row.Table.Rows[0]);
							for (var i = 1; i < 7; i++)
							{
								slotNo++;
								var itemInxName = row.ItemArray[i];
								//var itemID = 
							}
							incrItemCount++;
						
					}					
				}
				foreach (var item in itemList)
				{
					currentInfo.Items.Add(item);
					//incrItemCount++;
				}
				DatabaseLog.Write(DatabaseLogLevel.Startup, "Loaded NPC {0} with {1} items in shop", currentInfo.MobInfo.Index, currentInfo.Items.Count);
				return true;
			}
			catch(Exception)
			{
				return false;
			}
		}

        public static bool GetNPCListByMapId(ushort mapId, out List<NPCInfo> npcList) => _npcDataByMapId.TryGetValue(mapId, out npcList);
    }
}