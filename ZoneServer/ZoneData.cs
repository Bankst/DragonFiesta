using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;

using DFEngine;
using DFEngine.Content.Game;
using DFEngine.IO;
using DFEngine.IO.Definitions.SHN;
using DFEngine.Logging;
using DFEngine.Worlds;

namespace ZoneServer
{
	public class ZoneData
	{
		public static Dictionary<string, Field> Field = new Dictionary<string, Field>();
		public static ObjectCollection<MapInfo> MapInfo = new ObjectCollection<MapInfo>();

		public static bool LoadShineTables()
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();
			try
			{
				using (var fieldsFile = new ShineTable(Path.Combine(ServerMain.ZoneConfig.ShinePath, "World", "Field.txt")))
				using (var fieldsReader = new DataTableReader(fieldsFile["FieldList"]))
				{
					if (fieldsReader.HasRows)
					{
						while (fieldsReader.Read())
						{
							if (Field.ContainsKey(fieldsReader.GetString(0)))
							{

							}

							var field = new Field
							{
								MapIDClient = fieldsReader.GetString(0),
								SubHandles = new ConcurrentQueue<int>(),
								MapName = fieldsReader.GetString(4),
								Type = (FieldMapType) fieldsReader.GetByte(5),
								ImmortalSeconds = fieldsReader.GetInt16(8),
								ScriptName = fieldsReader.GetString(9),
								CanAttackEnemyGuild = fieldsReader.GetByte(12) != 0,
								NeedParty = fieldsReader.GetByte(13) != 0,
								IsPKKQ = fieldsReader.GetByte(16) != 0,
								IsPVP = fieldsReader.GetByte(17) != 0,
								IsPartyBattle = fieldsReader.GetByte(18) != 0,
								LinkIN = fieldsReader.GetByte(20) != 0,
								LinkOUT = fieldsReader.GetByte(21) != 0,
								IsSystemMap = fieldsReader.GetByte(22) != 0,
								RegenCity = fieldsReader.GetByte(23) != 0,
								CanRestart = fieldsReader.GetByte(33) != 0,
								CanTrade = fieldsReader.GetByte(34) != 0,
								CanMinihouse = fieldsReader.GetByte(35) != 0,
								CanUseItem = fieldsReader.GetByte(36) != 0,
								CanUseSkill = fieldsReader.GetByte(37) != 0,
								CanChat = fieldsReader.GetByte(38) != 0,
								CanShout = fieldsReader.GetByte(39) != 0,
								CanBooth = fieldsReader.GetByte(40) != 0,
								CanProduce = fieldsReader.GetByte(41) != 0,
								CanRide = fieldsReader.GetByte(42) != 0,
								CanStone = fieldsReader.GetByte(43) != 0,
								CanParty = fieldsReader.GetByte(44) != 0,
								ExpLossByMob = Convert.ToUInt16(fieldsReader.GetValue(45)),
								ExpLossByPlayer = Convert.ToUInt16(fieldsReader.GetValue(46)),
								IsSubLimit = fieldsReader.GetString(1) != "-" && fieldsReader.GetString(2) != "-",
								ZoneNo = fieldsReader.GetByte(49)
							};

							if (field.IsSubLimit)
							{
								field.SubFrom = int.Parse(fieldsReader.GetString(1));
								field.SubTo = int.Parse(fieldsReader.GetString(2));
								field.SubHandles = new ConcurrentQueue<int>(Enumerable.Range(field.SubFrom, field.SubTo));
							}

							var regenCount = fieldsReader.GetByte(32);
							var currentIndx = 24;

							for (var i = 0; i < regenCount; i++, currentIndx += 2)
							{
								field.Regens.Add(new Vector2(fieldsReader.GetInt32(currentIndx), fieldsReader.GetInt32(currentIndx + 1)));
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
	}
}
