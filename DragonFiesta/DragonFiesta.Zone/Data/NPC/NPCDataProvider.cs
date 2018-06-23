using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using DragonFiesta.Providers.Maps;
using DragonFiesta.Utils.Shine;
using DragonFiesta.Zone.Data.Mob;

namespace DragonFiesta.Zone.Data.NPC
{
	[GameServerModule(ServerType.Zone, GameInitalStage.NPC)]
    public class NPCDataProvider
    {
        private static ConcurrentDictionary<ushort, List<NPCInfo>> NPCDataByMapId;

        [InitializerMethod]
        public static bool InitialNPCDataProvider()
        {
            LoadNPCInfos();
            return true;
        }

        private static void LoadNPCInfos()
        {
            NPCDataByMapId = new ConcurrentDictionary<ushort, List<NPCInfo>>();

            NPCInfo currentInfo = null;
            SQLResult pResult = DB.Select(DatabaseType.Data, "SELECT * FROM NPCTable");

            for (int i = 0; i < pResult.Count; i++)
            {
                ushort MobID = pResult.Read<ushort>(i, "MobID");
                ushort MapID = pResult.Read<ushort>(i, "MapID");
                MapInfo minf;
                MobInfo mobinf;
                if (!MapDataProvider.GetMapInfoByID(MapID, out minf))
                {
                    DatabaseLog.Write(DatabaseLogLevel.Warning, "Error loading MapInfo for NPC '{0}' {1}.", MobID, MapID);
                }
                else if (!MobDataProvider.GetMobInfoByID(MobID, out mobinf))
                {
                    DatabaseLog.Write(DatabaseLogLevel.Warning, "Can't find MobInfo for NPC. Mob ID: " + MobID);
                }
                else
                {
                    List<NPCInfo> list;
                    currentInfo = new NPCInfo(pResult, i);
                    if (!NPCDataByMapId.TryGetValue(currentInfo.MapInfo.ID, out list))
                    {
                        list = new List<NPCInfo>();
                        NPCDataByMapId.TryAdd(currentInfo.MapInfo.ID, list);
                    }

                    if (currentInfo.Role == NPCRole.Gate || currentInfo.Role == NPCRole.IDGate)
                    {
                        SQLResult pgate = DB.Select(DatabaseType.Data, "SELECT * FROM LinkTable WHERE ID='" + (ushort)currentInfo.RoleArgument + "'");
                        if (pgate.Count == 1)
                        {
                            for (int i2 = 0; i2 < pgate.Count; i2++)
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
						if (!LoadItemList(currentInfo, out int incrItemCount))
						{
						    DatabaseLog.Write(DatabaseLogLevel.Warning, "Error loading item list for NPC '{0}'-'{1}'.", currentInfo.MobInfo.ID, currentInfo.MobInfo.Index);
						    continue;
						}
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
			List<NPCItem> itemList = new List<NPCItem>();

			// open NPCItemList for currentInfo
			var shinePath = "Shine/NPCItemList/";
			var filePath = $"{shinePath}{currentInfo.MobInfo.Index}.txt";
			
			//itemListShine.
			
			// parse
			try
			{
				ShineFile itemListShine = new ShineFile(filePath);
				foreach (var kvp in itemListShine.TableDict)
				{
					//byte tabNum = Convert.ToByte(kvp.Key.Substring(3,2));
					int slotNo = 0;
					foreach (DataRow row in kvp.Value.Rows)
					{
							//byte rowNum = Convert.ToByte(row.Table.Rows[0]);
							for (int i = 1; i < 7; i++)
							{
								slotNo++;
								var itemInxName = row.ItemArray[i];
								//var itemID = 
							}
							incrItemCount++;
						
					}					
				}
				foreach (NPCItem item in itemList)
				{
					currentInfo.Items.Add(item);
					//incrItemCount++;
				}
				DatabaseLog.Write(DatabaseLogLevel.Startup, "Loaded NPC {0} with {1} items in shop", currentInfo.MobInfo.Index, currentInfo.Items.Count);
				return true;
			}
			catch(Exception ex)
			{
				return false;
			}
		}

        public static bool GetNPCListByMapId(ushort MapId, out List<NPCInfo> NpcList) => NPCDataByMapId.TryGetValue(MapId, out NpcList);
    }
}