using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DFEngine;
using DFEngine.Content.Game;
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
		public static ObjectCollection<MapInfo> MapInfo = new ObjectCollection<MapInfo>();
		public static ObjectCollection<ItemInfo> ItemInfo = new ObjectCollection<ItemInfo>();
		public static ObjectCollection<ItemInfoServer> ItemInfoServer = new ObjectCollection<ItemInfoServer>();
		public static ObjectCollection<SingleData> SingleData = new ObjectCollection<SingleData>();

		// Map stuff

		// Other
		public static Dictionary<SHN_DATA_FILE_INDEX, string> SHNChecksums = new Dictionary<SHN_DATA_FILE_INDEX, string>();

		public static bool CalculateSHNChecksums()
		{
			// Load SHN checksums
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
					EngineLog.Write(EngineLogLevel.Exception, $"Missing SHN file {shnName}!");
					return false;
				}
			}

			return true;
		}

		public static bool LoadSHNs()
		{
			// Just load all SHNs
			SHNFile.LoadFromFolder(ZoneServer.ZoneConfig.ShinePath);

			SHNFile.TryGetObjects("ItemInfo", out ItemInfo);
			SHNFile.TryGetObjects("ItemInfoServer", out ItemInfoServer);
			SHNFile.TryGetObjects("MapInfo", out MapInfo);
			SHNFile.TryGetObjects("SingleData", out SingleData);

			return true;
		}

		public static bool LoadShineTables()
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();
			try
			{
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
				stopwatch.Stop();
				EngineLog.Write(EngineLogLevel.Startup, $"Loaded {Field.Count} Fields in {stopwatch.ElapsedMilliseconds}");
				return true;
			}
			catch (Exception ex)
			{
				stopwatch.Stop();
				EngineLog.Write(EngineLogLevel.Exception, $"Error loading Fields! \n {ex}");
				return false;
			}
		}

		public static bool LoadMaps()
		{
			// do mapload.txt?

			var mapsToLoad = MapInfo.Values.Filter(map => Field.ContainsKey(map.MapName) && Field[map.MapName].ZoneNo == ZoneServer.ZoneId);
			Parallel.ForEach(mapsToLoad, (info, state, i) =>
			{
				var field = Field[info.MapName];
				if (field.IsSubLimit && !MapService.TryInstantiate(info, field, out var unused))
				{
					EngineLog.Write(EngineLogLevel.Exception, $"Failed to load map: {info.MapName}. Check above for errors.");
				}
			});

			return true;
		}
	}
}
