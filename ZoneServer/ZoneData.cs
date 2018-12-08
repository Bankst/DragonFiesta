using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DFEngine;
using DFEngine.Content.Game;
using DFEngine.Content.GameObjects;
using DFEngine.IO;
using DFEngine.IO.Definitions;
using DFEngine.IO.Definitions.SHN;
using DFEngine.Logging;
using DFEngine.Worlds;
using ZoneServer.Services;

namespace ZoneServer
{
	public class ZoneData
	{
		// Shine tables
		public static Dictionary<string, Field> Field = new Dictionary<string, Field>();

		// SHN files
		public static ObjectCollection<ClassName> ClassName = new ObjectCollection<ClassName>();
		public static ObjectCollection<MapInfo> MapInfo = new ObjectCollection<MapInfo>();
		public static ObjectCollection<ItemInfo> ItemInfo = new ObjectCollection<ItemInfo>();
//		public static ObjectCollection<ItemInfoServer> ItemInfoServer = new ObjectCollection<ItemInfoServer>(); // TODO: Find definition
		public static ObjectCollection<SingleData> SingleData = new ObjectCollection<SingleData>();

		// Map stuff

		// Other
		public static Dictionary<SHN_DATA_FILE_INDEX, string> SHNChecksums = new Dictionary<SHN_DATA_FILE_INDEX, string>();
		public static Dictionary<CharacterClass, Dictionary<int, Parameters>> ParamServer = new Dictionary<CharacterClass, Dictionary<int, Parameters>>();

		private static readonly Stopwatch Stopwatch = new Stopwatch();

		public static bool CalculateSHNChecksums()
		{
			Stopwatch.Reset();
			Stopwatch.Start();
			// Load SHN checksums
			try
			{
				for (byte i = 0; i < (byte)SHN_DATA_FILE_INDEX.SHN_MaxCnt; i++)
				{
					var shnIndex = (SHN_DATA_FILE_INDEX)i;
					var shnName = shnIndex.ToString().Replace("SHN_", "");
					var shnFullPath = Path.Combine(ZoneServer.ZoneConfig.ShinePath, $"{shnName}.shn");

					if (File.Exists(shnFullPath))
					{
						using (var md5 = MD5.Create())
						{
							var hash = Encoding.Default.GetString(md5.ComputeHash(File.ReadAllBytes(shnFullPath)));
							SHNChecksums.Add(shnIndex, hash);
						}
					}
					else
					{
						EngineLog.Write(EngineLogLevel.Warning, $"Missing SHN Checksum file {shnName}!");
					}
				}
			}
			catch (Exception ex)
			{
				Stopwatch.Stop();
				EngineLog.Write(EngineLogLevel.Exception, $"Error calculating SHN checksums!\n {ex}");
				return false;
			}

			Stopwatch.Stop();
			EngineLog.Write(EngineLogLevel.Startup, $"Calculated SHN checksums in {Stopwatch.ElapsedMilliseconds}ms");
			return true;
		}

		public static bool LoadSHNs()
		{
			/* SHN File Data */
			try
			{
				Stopwatch.Reset();
				Stopwatch.Start();
				SHNFile.LoadFromFolder(ZoneServer.ZoneConfig.ShinePath);

				SHNFile.TryGetObjects("ClassName", out ClassName);
				SHNFile.TryGetObjects("MapInfo", out MapInfo);
				SHNFile.TryGetObjects("ItemInfo", out ItemInfo);
//				SHNFile.TryGetObjects("ItemInfoServer", out ItemInfoServer); // TODO: Figure this out
				SHNFile.TryGetObjects("SingleData", out SingleData);

				Stopwatch.Stop();
				EngineLog.Write(EngineLogLevel.Startup, $"Loaded {SHNFile.Count} SHNs in {Stopwatch.ElapsedMilliseconds}ms");
			}
			catch (Exception ex)
			{
				Stopwatch.Stop();
				EngineLog.Write(EngineLogLevel.Exception, $"Error loading SHNs!\n {ex}");
				return false;
			}
			return true;
		}

		public static bool LoadOther()
		{
			return true;
		}

		public static bool LoadShineTables()
		{
			// Field.txt
			try
			{
				Stopwatch.Reset();
				Stopwatch.Start();
				using (var file = new ShineTable(Path.Combine(ZoneServer.ZoneConfig.ShinePath, "World", "Field.txt")))
				using (var reader = new DataTableReader(file["FieldList"]))
				{
					if (reader.HasRows)
					{
						while (reader.Read())
						{
							if (Field.ContainsKey(reader.GetString(0)))
							{
								continue;
							}

							var field = new Field
							{
								MapIDClient = reader.GetString(0),
								SubHandles = new ConcurrentQueue<int>(),
								MapName = reader.GetString(4),
								Type = (FieldMapType) reader.GetByte(5),
								ImmortalSeconds = reader.GetInt16(8),
								ScriptName = reader.GetString(9),
								CanAttackEnemyGuild = reader.GetByte(12) != 0,
								NeedParty = reader.GetByte(13) != 0,
								IsPKKQ = reader.GetByte(16) != 0,
								IsPVP = reader.GetByte(17) != 0,
								IsPartyBattle = reader.GetByte(18) != 0,
								LinkIN = reader.GetByte(20) != 0,
								LinkOUT = reader.GetByte(21) != 0,
								IsSystemMap = reader.GetByte(22) != 0,
								RegenCity = reader.GetString(23),
								CanRestart = reader.GetByte(33) != 0,
								CanTrade = reader.GetByte(34) != 0,
								CanMinihouse = reader.GetByte(35) != 0,
								CanUseItem = reader.GetByte(36) != 0,
								CanUseSkill = reader.GetByte(37) != 0,
								CanChat = reader.GetByte(38) != 0,
								CanShout = reader.GetByte(39) != 0,
								CanBooth = reader.GetByte(40) != 0,
								CanProduce = reader.GetByte(41) != 0,
								CanRide = reader.GetByte(42) != 0,
								CanStone = reader.GetByte(43) != 0,
								CanParty = reader.GetByte(44) != 0,
								ExpLossByMob = Convert.ToUInt16(reader.GetValue(45)),
								ExpLossByPlayer = Convert.ToUInt16(reader.GetValue(46)),
								IsSubLimit = reader.GetString(1) != "-" && reader.GetString(2) != "-",
								ZoneNo = reader.GetByte(49),
								Regens = new ConcurrentBag<Vector2>()
							};

							if (field.IsSubLimit)
							{
								field.SubFrom = int.Parse(reader.GetString(1));
								field.SubTo = int.Parse(reader.GetString(2));
								field.SubHandles = new ConcurrentQueue<int>(Enumerable.Range(field.SubFrom, field.SubTo));
							}

							var regenCount = reader.GetByte(32);
							var currentIndx = 24;

							for (var i = 0; i < regenCount; i++, currentIndx += 2)
							{
								var regenXA = reader.GetInt32(currentIndx);
								var regenYA = reader.GetInt32(currentIndx + 1);
								field.Regens.Add(new Vector2(regenXA, regenYA));
							}

							Field.AddSafe(field.MapIDClient, field);
						}
					}
				}
				Stopwatch.Stop();
				EngineLog.Write(EngineLogLevel.Startup, $"Loaded {Field.Count} rows from Field.txt in {Stopwatch.ElapsedMilliseconds}ms");
			}
			catch (Exception ex)
			{
				Stopwatch.Stop();
				EngineLog.Write(EngineLogLevel.Exception, $"Error loading Field.txt!\n {ex}");
				return false;
			}

			//Class Params
			try
			{
				Stopwatch.Reset();
				Stopwatch.Start();

				var loadedRows = 0;

				foreach (var className in ClassName.Values)
				{
					var classID = (CharacterClass)className.ClassID;
					var engName = className.acEngName;
						
					if (!ParamServer.ContainsKey(classID))
					{
						ParamServer.Add(classID, new Dictionary<int, Parameters>());
					}

					var param = ParamServer[classID];
					var path = Path.Combine(ZoneServer.ZoneConfig.ShinePath, "World", $"Param{engName}Server.txt");

					if (!File.Exists(path))
					{
						continue;
					}


					using (var file = new ShineTable(path))
					using (var reader = new DataTableReader(file["Param"]))
					{
						if (!reader.HasRows)
						{
							continue;
						}

						while (reader.Read())
						{
							if (!param.ContainsKey(reader.GetInt32(0)))
							{
								param.Add(reader.GetInt32(0), new Parameters());
							}

							var parameters = param[reader.GetInt32(0)];

							parameters.STR = reader.GetInt32(1);
							parameters.END = reader.GetInt32(2);
							parameters.INT = reader.GetInt32(3);
							parameters.DEX = reader.GetInt32(5);
							parameters.SPR = reader.GetInt32(6);
							parameters.HPStoneHealth = reader.GetInt32(7);
							parameters.MaxHPStones = reader.GetInt32(8);
							parameters.HPStonePrice = reader.GetInt32(9);
							parameters.SPStoneSpirit = reader.GetInt32(10);
							parameters.MaxSPStones = reader.GetInt32(11);
							parameters.SPStonePrice = reader.GetInt32(12);
							parameters.IllnessResistance = reader.GetInt32(25);
							parameters.DiseaseResistance = reader.GetInt32(26);
							parameters.CurseResistance = reader.GetInt32(27);
							parameters.StunResistance = reader.GetInt32(28);
							parameters.MaxHP = reader.GetInt16(29);
							parameters.MaxSP = reader.GetInt16(30);
							parameters.SkillPoints = reader.GetInt32(32);

							param[reader.GetInt32(0)] = parameters;

							loadedRows++;
						}
					}
				}

				Stopwatch.Stop();
				EngineLog.Write(EngineLogLevel.Startup, $"Loaded {loadedRows} rows from {ParamServer.Count} ParamServer.txt(s) in {Stopwatch.ElapsedMilliseconds}ms");
			}
			catch (Exception ex)
			{
				Stopwatch.Stop();
				EngineLog.Write(EngineLogLevel.Exception, $"Error loading ParamServer.txt(s)!\n {ex}");
				return false;
			}

			// Character Common
			try
			{
				Stopwatch.Reset();
				Stopwatch.Start();

				var filePath = Path.Combine(ZoneServer.ZoneConfig.ShinePath, "World", "ChrCommon.txt");

				Character.Global = new CharacterCommon();

				using (var file = new ShineTable(filePath))
				using (var common = new DataTableReader(file["Common"]))
				using (var stats = new DataTableReader(file["StatTable"]))
				{
					if (common.HasRows)
					{
						while (common.Read())
						{
							var type = typeof(CharacterCommon);
							var propertyInfo = type.GetProperty(common.GetString(0), BindingFlags.Public | BindingFlags.Instance);

							propertyInfo?.SetValue(Character.Global, common.GetInt32(1));
						}
					}

					if (stats.HasRows)
					{
						while (stats.Read())
						{
							if (Character.Global.NextEXP.ContainsKey(stats.GetByte(0)))
							{
								continue;
							}

							Character.Global.NextEXP.Add(stats.GetByte(0), Convert.ToInt64(stats.GetValue(1)));

							if (Character.Global.EXPLostAtPVP.ContainsKey(stats.GetByte(0)))
							{
								continue;
							}

							Character.Global.EXPLostAtPVP.Add(stats.GetByte(0), Convert.ToInt64(stats.GetValue(30)));
						}
					}

					Character.Global.AttackSpeed *= 100;
				}

				Stopwatch.Stop();
				EngineLog.Write(EngineLogLevel.Startup, $"Loaded {Character.Global.NextEXP.Count} rows from ChrCommon.txt in {Stopwatch.ElapsedMilliseconds}ms");
			}
			catch (Exception ex)
			{
				Stopwatch.Stop();
				EngineLog.Write(EngineLogLevel.Exception, $"Error loading ChrCommon.txt!\n {ex}");
				return false;
			}

			return true;
		}

		public static bool LoadMaps()
		{
			// do mapload.txt?
			try
			{
				Stopwatch.Reset();
				Stopwatch.Start();

				var mapsToLoad = MapInfo.Values.Filter(map => Field.ContainsKey(map.MapName) && Field[map.MapName].ZoneNo == ZoneServer.ZoneId);

				foreach (var mapInfo in mapsToLoad)
				{
					var field = Field[mapInfo.MapName];
					if (!field.IsSubLimit && !MapService.TryInstantiate(mapInfo, field, out var unused))
					{
						EngineLog.Write(EngineLogLevel.Exception,
							$"Failed to load map: {mapInfo.MapName}. Check above for errors.");
					}
				}
				
				var loadedMapCount = MapService.Instances.Count;

				// some maps failed to load
				if (mapsToLoad.Count > MapService.Instances.Count)
				{
					var failedMaps = mapsToLoad.Filter(map => !MapService.Instances.ContainsKey(map.MapName));

					// attempt 3 times to load them again
					var attemptCount = 3;
					while (attemptCount > 0)
					{
						foreach (var failedMap in failedMaps)
						{
							var field = Field[failedMap.MapName];
							if (!field.IsSubLimit && !MapService.TryInstantiate(failedMap, field, out var unused))
							{
								EngineLog.Write(EngineLogLevel.Exception, $"Failed to load map: {failedMap.MapName}. Check above for errors.");
							}
						}
						// update failedMaps and loadedMapCount
						failedMaps = mapsToLoad.Filter(map => !MapService.Instances.ContainsKey(map.MapName));
						loadedMapCount = MapService.Instances.Count;
						attemptCount--;
					}

					var failedMapsList = failedMaps.Select(info => info.MapName).ToArray();

					EngineLog.Write(EngineLogLevel.Warning, $"Failed to load {mapsToLoad.Count - loadedMapCount} maps: {string.Join(", ", failedMapsList)}");
				}

				Stopwatch.Stop();
				if (MapService.Instances.Count != mapsToLoad.Count)
				{
					EngineLog.Write(EngineLogLevel.Startup, $"Successfully loaded {loadedMapCount} maps in {Stopwatch.ElapsedMilliseconds}ms");
				}
			
		}
			catch (Exception ex)
			{
				Stopwatch.Stop();
				EngineLog.Write(EngineLogLevel.Exception, $"Error loading Maps!\n {ex}");
				return false;
			}

			return true;
		}
	}
}
