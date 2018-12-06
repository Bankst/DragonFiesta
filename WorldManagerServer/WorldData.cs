using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using DFEngine;
using DFEngine.Content.GameObjects;
using DFEngine.Content.Other;
using DFEngine.IO;
using DFEngine.IO.Definitions.SHN;

namespace WorldManagerServer
{
	public static class WorldData
	{
		private static readonly string[] SHNList =
		{
			"HairInfo.shn",
			"HairColorInfo.shn",
			"FaceInfo.shn",
			"SingleData.shn",
			"ItemInfo.shn"
		};

		// Shine tables
		internal static Dictionary<CharacterClass, DefaultCharacterData> DefaultCharacterData = new Dictionary<CharacterClass, DefaultCharacterData>();

		// SHN Files
		internal static ObjectCollection<FaceInfo> FaceInfo = new ObjectCollection<FaceInfo>();
		internal static ObjectCollection<HairInfo> HairInfo = new ObjectCollection<HairInfo>();
		internal static ObjectCollection<HairColorInfo> HairColorInfo = new ObjectCollection<HairColorInfo>();
		internal static ObjectCollection<SingleData> SingleData = new ObjectCollection<SingleData>();
		internal static ObjectCollection<ItemInfo> ItemInfo =  new ObjectCollection<ItemInfo>();

		public static void LoadSHNs()
		{
			/* SHN File Data */
			SHNFile.LoadFromFolder(WorldManagerServer.WorldConfig.ShinePath, SHNList);

			SHNFile.TryGetObjects("HairInfo", out HairInfo);
			SHNFile.TryGetObjects("HairColorInfo", out HairColorInfo);
			SHNFile.TryGetObjects("FaceInfo", out FaceInfo);
			SHNFile.TryGetObjects("SingleData", out SingleData);
			SHNFile.TryGetObjects("ItemInfo", out ItemInfo);
		}

		public static void LoadScripts()
		{
			// DefaultCharacterData
			using (var file = new Script(Path.Combine(WorldManagerServer.WorldConfig.ShinePath, "DefaultCharacterData.txt")))
			{
				using (var reader = new ScriptReader(file["CHARACTER"]))
				{
					if (reader.HasRows)
					{
						while (reader.Read())
						{
							DefaultCharacterData.Add((CharacterClass)reader.GetByte(0), new DefaultCharacterData
							{
								Class = (CharacterClass)reader.GetByte(0),
								MapIndx = reader.GetString(1),
								PosX = reader.GetInt32(2),
								PosY = reader.GetInt32(3),
								HP = reader.GetInt32(4),
								SP = reader.GetInt32(5),
								HPStone = reader.GetInt32(6),
								SPStone = reader.GetInt32(7),
								Cen = reader.GetInt64(8),
								Level = reader.GetByte(9),
								EXP = reader.GetInt64(10)
							});
						}
					}
				}

				using (var reader = new ScriptReader(file["ITEM"]))
				{
					if (reader.HasRows)
					{
						while (reader.Read())
						{
							DefaultCharacterData.GetSafe((CharacterClass)reader.GetByte(0))?.Items.Add(new Tuple<ushort, byte>(reader.GetUInt16(1), reader.GetByte(2)));
						}
					}
				}

				using (var reader = new ScriptReader(file["SKILL"]))
				{
					if (reader.HasRows)
					{
						while (reader.Read())
						{
							DefaultCharacterData.GetSafe((CharacterClass)reader.GetByte(0))?.Skills.Add(reader.GetUInt16(1));
						}
					}
				}

				using (var reader = new ScriptReader(file["QUEST"]))
				{
					if (reader.HasRows)
					{
						while (reader.Read())
						{
							DefaultCharacterData.GetSafe((CharacterClass)reader.GetByte(0))?.Quests.Add(reader.GetInt32(1));
						}
					}
				}

				using (var reader = new ScriptReader(file["SHORTCUT"]))
				{
					if (reader.HasRows)
					{
						while (reader.Read())
						{
							DefaultCharacterData.GetSafe((CharacterClass)reader.GetByte(0))?.Shortcuts.Add(new Shortcut
							{
								Slot = reader.GetByte(1),
								Code = reader.GetUInt16(2),
								Value = reader.GetUInt16(3)
							});
						}
					}
				}
			}

			// Any other scripts
		}
	}
}
