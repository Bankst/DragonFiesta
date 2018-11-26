using System;
using System.Collections.Generic;
using System.IO;

using DFEngine.Content.GameObjects;
using DFEngine.Content.Items;
using DFEngine.Content.Tutorial;
using DFEngine.Database;
using DFEngine.Logging;
using DFEngine.Network;
using DFEngine.Utils;

namespace WorldManagerServer.Services
{
	internal static class AvatarService
	{
		internal const string DEFAULT_GAME_SETTINGS = "1C 00 00 00 01 01 00 01 02 00 01 03 00 01 04 00 00 05 00 01 06 00 00 07 00 01 08 00 01 09 00 01 0A 00 01 0B 00 01 0C 00 00 0D 00 00 0E 00 00 0F 00 00 10 00 01 11 00 00 12 00 00 13 00 00 14 00 01 15 00 00 16 00 00 17 00 00 18 00 01 19 00 01 1A 00 01 1B 00 01";
		internal const string DEFAULT_KEYMAP_SETTINGS = "5F 00 00 00 00 79 01 00 00 1B 02 00 00 43 03 00 00 49 04 00 00 4B 05 00 00 4C 06 00 00 46 07 00 00 48 08 00 00 56 09 00 00 0D 0A 00 11 4E 0B 00 11 47 0C 00 11 50 0D 00 11 57 0E 00 00 DE 0F 00 00 58 10 00 00 47 11 00 00 00 12 00 00 00 13 00 00 00 14 00 00 00 15 00 00 52 16 00 11 41 17 00 00 57 18 00 00 53 19 00 00 00 1A 00 00 41 1B 00 00 44 1C 00 00 5A 1D 00 00 20 1E 00 00 26 1F 00 00 28 20 00 00 25 21 00 00 27 22 00 00 24 23 00 00 54 24 00 00 51 25 00 00 45 26 00 00 F5 27 00 00 42 28 00 00 50 29 00 00 4D 2A 00 00 55 2B 00 10 5A 2C 00 00 00 2D 00 00 00 2E 00 00 23 2F 00 00 31 30 00 00 32 31 00 00 33 32 00 00 34 33 00 00 35 34 00 00 36 35 00 00 37 36 00 00 38 37 00 00 39 38 00 00 30 39 00 00 BD 3A 00 00 BB 3B 00 10 31 3C 00 10 32 3D 00 10 33 3E 00 10 34 3F 00 10 35 40 00 10 36 41 00 10 37 42 00 10 38 43 00 10 39 44 00 10 30 45 00 10 BD 46 00 10 BB 47 00 12 31 48 00 12 32 49 00 12 33 4A 00 12 34 4B 00 12 35 4C 00 12 36 4D 00 12 37 4E 00 12 38 4F 00 12 39 50 00 12 30 51 00 12 BD 52 00 12 BB 53 00 00 00 54 00 00 00 55 00 00 00 56 00 00 00 57 00 00 00 58 00 00 00 59 00 00 00 5A 00 00 00 5B 00 00 00 5C 00 00 00 5D 00 00 00 5E 00 00 00";
		internal const string DEFAULT_SHORTCUTSIZE_SETTINGS = "05 00 03 18 00 00 05 00 0C 00 00 0C 01 00 0C 02 00 0C 03 00 0C 04 00 00";
		internal const string DEFAULT_WINDOWPOS_SETTINGS = "40 F4 FD 1E AD 14 1D 77 98 9D 58 1D 68 9D 58 1D 98 9D 58 1D 00 00 00 00 00 00 00 00 00 00 72 00 40 1B 0F 23 7C 9D 58 1D 40 1B 0F 23 0C 00 0B 00 BD 08 00 00 7C 9D 58 1D 00 00 00 00 00 00 00 00 28 1B 0F 23 01 00 00 00 38 1B 0F 23 90 F4 FD 1E A3 E0 D4 77 40 1B 0F 23 00 00 00 00 01 00 00 00 38 1B 0F 23 DC F4 FD 1E CD 98 CE 75 00 00 72 00 00 00 00 00 DA 98 CE 75 F8 A7 DF 50 00 00 00 00 38 63 D9 10 01 00 00 00 00 00 00 00 01 00 00 00 38 63 D9 10 CC F4 FD 1E A4 F4 FD 1E DC CC 38 01 B4 F8 FD 1E D5 8C D0 75 C4 CB EC 3B 00 F5 FD 1E 06 E4 EE 74 00 00 00 00 00 00 00 00 8C D2 F6 0C 38 63 D9 10 51 53 F9 0C 00 00 00 00 FF FF FF FF 40 1B 0F 23 18 F5 FD 1E A5 BE A8 00 38 63 D9 10 00 00 00 00 FD FF FF FF 51 53 F9 0C B8 F5 FD 1E 0E 6A A3 00 9C E3 F6 0C 7F 06 00 00 14 00 00 00 88 D2 F6 0C 57 6A A3 00 14 F5 FD 1E DC CC 38 01 B4 F8 FD 1E D5 8C D0 75 C4 CB EC 3B 70 F5 FD 1E 06 E4 EE 74 00 00 00 00 00 00 00 00 53 53 F9 0C 38 63 D9 10 53 53 F9 0C 00 00 00 00 FF FF FF FF 98 83 0D 23 8C F5 FD 1E 51 B0 A8 00 9A 7A DC 50 B8 F5 FD 1E 7A FF A4 00 9C E3 F6 0C 94 F5 FD 1E 88 D2 F6 0C 8B FF A4 00 00 52 68 7C 2F DD 05 05 08 07 A6 00 8C 03 39 0D 72 42 00 00 00 00 00 00 C0 F5 FD 1E 04 1E A8 00";

		internal static List<Avatar> LoadAll(int userNo)
		{
			var ret = new List<Avatar>();

			using (var p_Char_GetListOfUserChar = new StoredProcedure("p_Char_GetListOfUserChar", DB.GetDatabaseClient(DatabaseType.Character).Connection))
			{
				p_Char_GetListOfUserChar.AddParameter("nUserNo", userNo);

				using (var reader = p_Char_GetListOfUserChar.RunReader())
				{
					if (reader.HasRows)
					{
						while (reader.Read())
						{
							var charNo = reader.GetInt32(0);

							if (!TryLoad(charNo, out var avatar))
							{
								DatabaseLog.Write(DatabaseLogLevel.Warning, $"Failed to load an avatar - CharNo: {charNo}");
								continue;
							}

							ret.Add(avatar);
						}
					}
				}
			}

			return ret;
		}

		internal static bool TryLoad(int charNo, out Avatar avatar)
		{
			avatar = new Avatar { CharNo = charNo };

			using (var p_Char_GetLoginData = new StoredProcedure("p_Char_GetLoginData", DB.GetDatabaseClient(DatabaseType.Character).Connection))
			{
				p_Char_GetLoginData.AddParameter("nCharNo", charNo);

				using (var reader = p_Char_GetLoginData.RunReader())
				{
					if (!reader.HasRows)
					{
						return false;
					}

					while (reader.Read())
					{
						avatar.Name = reader.GetString(0);
						avatar.Level = Convert.ToByte(reader.GetValue(1));
						avatar.Slot = (byte)reader.GetValue(2);
						avatar.MapIndx = reader.GetString(3);
						avatar.IsDeleted = reader.GetBoolean(7);
						avatar.DeleteTime = reader.GetDateTime(8);
						avatar.Shape.Race = (byte)reader.GetValue(9);
						avatar.Shape.Class = (CharacterClass)(byte)reader.GetValue(10);
						avatar.Shape.Gender = (Gender)(byte)reader.GetValue(11);
						avatar.Shape.Hair = (byte)reader.GetValue(12);
						avatar.Shape.HairColor = (byte)reader.GetValue(13);
						avatar.Shape.Face = (byte)reader.GetValue(14);
						avatar.KQHandle = reader.GetInt32(15);
						avatar.KQMapIndx = reader.GetString(16);
						avatar.KQPosition = new Vector2(reader.GetInt32(17), reader.GetInt32(18));
						avatar.KQDate = reader.GetDateTime(20);
						avatar.TutorialState = (TutorialState)reader.GetInt32(22);
						avatar.TutorialStep = reader.GetByte(23);
					}
				}
			}

			// load equipment

			return true;
		}

		internal static bool IsNameInUse(string name)
		{
			using (var p_Char_Find = new StoredProcedure("p_Char_Find", DB.GetDatabaseClient(DatabaseType.Character).Connection))
			{
				p_Char_Find.AddParameter("sID", name, 40);
				p_Char_Find.AddOutput<int>("nCharNo");

				return p_Char_Find.Run().GetOutput<int>("nCharNo") != 0;
			}
		}

		internal static bool Create(NetworkConnection connection, byte slot, string name, byte race, CharacterClass @class, Gender gender, byte hair, byte hairColor, byte face, out Avatar avatar)
		{
			int charNo;
			avatar = null;

			using (var p_Char_Create = new StoredProcedure("p_Char_Create", DB.GetDatabaseClient(DatabaseType.Character).Connection))
			{
				p_Char_Create.AddParameter("nUserNo", connection.Account.UserNo);
				p_Char_Create.AddParameter("nCreateWorld", ServerMain.NetConfig.WorldNetConfig.WorldID);
				p_Char_Create.AddParameter("nAdminLevel", 0);
				p_Char_Create.AddParameter("nSlotNo", slot);
				p_Char_Create.AddParameter("sID", name, 40);
				p_Char_Create.AddParameter("nRace", race);
				p_Char_Create.AddParameter("nClass", (byte)@class);
				p_Char_Create.AddParameter("nGender", (byte)gender);
				p_Char_Create.AddParameter("nHairType", hair);
				p_Char_Create.AddParameter("nHairColor", hairColor);
				p_Char_Create.AddParameter("nFaceShape", face);
				p_Char_Create.AddOutput<int>("nCharNo");

				charNo = p_Char_Create.Run().GetOutput<int>("nCharNo");

				if (charNo <= -1)
				{
					return false;
				}
			}

			if (Data.DefaultCharacterData.TryGetValue(@class, out var defaultValues))
			{
				using (var p_Char_CreateSetDefaultData = new StoredProcedure("p_Char_CreateSetDefaultData", DB.GetDatabaseClient(DatabaseType.Character).Connection))
				{
					p_Char_CreateSetDefaultData.AddParameter("nCharNo", charNo);
					p_Char_CreateSetDefaultData.AddParameter("sLoginZone", defaultValues.MapIndx, 16);
					p_Char_CreateSetDefaultData.AddParameter("nLoginZoneX", defaultValues.PosX);
					p_Char_CreateSetDefaultData.AddParameter("nLoginZoneY", defaultValues.PosY);
					p_Char_CreateSetDefaultData.AddParameter("nHP", defaultValues.HP);
					p_Char_CreateSetDefaultData.AddParameter("nSP", defaultValues.SP);
					p_Char_CreateSetDefaultData.AddParameter("nHPS", defaultValues.HPStone);
					p_Char_CreateSetDefaultData.AddParameter("nSPS", defaultValues.SPStone);
					p_Char_CreateSetDefaultData.AddParameter("nLevel", defaultValues.Level);
					p_Char_CreateSetDefaultData.AddParameter("nMoney", defaultValues.Cen);
					p_Char_CreateSetDefaultData.AddParameter("nExp", defaultValues.EXP);
					p_Char_CreateSetDefaultData.AddOutput<int>("nRet");

					var ret = p_Char_CreateSetDefaultData.Run().GetOutput<int>("nRet");

					if (ret == 0)
					{
						return false;
					}
				}

				using (var p_Char_RedistributePointSet = new StoredProcedure("p_Char_RedistributePointSet", DB.GetDatabaseClient(DatabaseType.Character).Connection))
				{
					p_Char_RedistributePointSet.AddParameter("nCharNo", charNo);
					p_Char_RedistributePointSet.AddParameter("nSetPoint", defaultValues.Level);
					p_Char_RedistributePointSet.AddOutput<byte>("nRet");
					p_Char_RedistributePointSet.Run();
				}

				foreach (var item in defaultValues.Items)
				{
					long itemKey;

					using (var p_Item_Create = new StoredProcedure("p_Item_Create", DB.GetDatabaseClient(DatabaseType.Character).Connection))
					{
						p_Item_Create.AddParameter("nOwner", charNo);
						p_Item_Create.AddParameter("nStorageType", (byte)InventoryType.IT_INVENTORY);
						p_Item_Create.AddParameter("nStorage", (byte)defaultValues.Items.IndexOf(item));
						p_Item_Create.AddParameter("nItemID", (int)item.Item1);
						p_Item_Create.AddParameter("nFlags", 0);
						p_Item_Create.AddOutput<long>("nItemKey");
						p_Item_Create.AddOutput<int>("nRet");
						itemKey = p_Item_Create.Run().GetOutput<long>("nItemKey");
					}

					using (var p_Item_SetOption = new StoredProcedure("p_Item_SetOption", DB.GetDatabaseClient(DatabaseType.Character).Connection))
					{
						p_Item_SetOption.AddParameter("nItemKey", itemKey);
						p_Item_SetOption.AddParameter("nOptionType", 1);
						p_Item_SetOption.AddParameter("nOptionData", item.Item2);
						p_Item_SetOption.AddOutput<int>("nRet");
						p_Item_SetOption.Run();
					}
				}

				long houseKey;

				using (var p_Item_Create = new StoredProcedure("p_Item_Create", DB.GetDatabaseClient(DatabaseType.Character).Connection))
				{
					p_Item_Create.AddParameter("nOwner", charNo);
					p_Item_Create.AddParameter("nStorageType", (byte)InventoryType.IT_MINIHOUSE);
					p_Item_Create.AddParameter("nStorage", 0);
					p_Item_Create.AddParameter("nItemID", Convert.ToInt32(31000));
					p_Item_Create.AddParameter("nFlags", 0);
					p_Item_Create.AddOutput<long>("nItemKey");
					p_Item_Create.AddOutput<int>("nRet");
					houseKey = p_Item_Create.Run().GetOutput<long>("nItemKey");
				}

				using (var p_Item_SetOption = new StoredProcedure("p_Item_SetOption", DB.GetDatabaseClient(DatabaseType.Character).Connection))
				{
					p_Item_SetOption.AddParameter("nItemKey", houseKey);
					p_Item_SetOption.AddParameter("nOptionType", 1);
					p_Item_SetOption.AddParameter("nOptionData", 1);
					p_Item_SetOption.AddOutput<int>("nRet");
					p_Item_SetOption.Run();
				}

				foreach (var quest in defaultValues.Quests)
				{
					using (var p_Quest_Add = new StoredProcedure("p_Quest_Add", DB.GetDatabaseClient(DatabaseType.Character).Connection))
					{
						p_Quest_Add.AddParameter("nCharNo", charNo);
						p_Quest_Add.AddParameter("nQuestNo", quest);
						p_Quest_Add.AddParameter("nStatus", (byte)0);
						p_Quest_Add.AddParameter("sData", new byte[100], 100);
						p_Quest_Add.AddOutput<int>("nRet");
						p_Quest_Add.Run();
					}
				}

				foreach (var skill in defaultValues.Skills)
				{
					using (var p_Skill_Set = new StoredProcedure("p_Skill_Set", DB.GetDatabaseClient(DatabaseType.Character).Connection))
					{
						p_Skill_Set.AddParameter("nCharNo", charNo);
						p_Skill_Set.AddParameter("nSkillNo", Convert.ToInt32(skill));
						p_Skill_Set.AddParameter("nSkillLevel", 0);
						p_Skill_Set.AddParameter("nSkillExp", 0);
						p_Skill_Set.AddParameter("nSkillWriteTime", 0);
						p_Skill_Set.AddParameter("nSkillCoolTime", 0);
						p_Skill_Set.AddParameter("nSkillPowerDemage", 0);
						p_Skill_Set.AddParameter("nSkillPowerSP", 0);
						p_Skill_Set.AddParameter("nSkillPowerKeepTime", 0);
						p_Skill_Set.AddParameter("nSkillPowerCoolTime", 0);
						p_Skill_Set.AddOutput<int>("nRet");
						p_Skill_Set.Run();
					}
				}

				byte[] shortcutData;

				using (var stream = new MemoryStream())
				using (var writer = new BinaryWriter(stream))
				{
					writer.Write((ushort)defaultValues.Shortcuts.Count);

					foreach (var shortcut in defaultValues.Shortcuts)
					{
						writer.Write(shortcut.Slot);
						writer.Write(shortcut.Code);
						writer.Write(shortcut.Value);
						writer.Write((ushort)0);
					}

					shortcutData = stream.ToArray();
				}

				using (var p_Char_SetOptShortCutData = new StoredProcedure("p_Char_SetOptShortCutData", DB.GetDatabaseClient(DatabaseType.Character).Connection))
				{
					p_Char_SetOptShortCutData.AddParameter("nCharNo", charNo);
					p_Char_SetOptShortCutData.AddParameter("sData", shortcutData, 1024);
					p_Char_SetOptShortCutData.AddOutput<int>("nRet");
					p_Char_SetOptShortCutData.Run();
				}

				return TryLoad(charNo, out avatar);
			}

			DatabaseLog.Write(DatabaseLogLevel.Warning, $"No default character data found for {@class}.");
			return false;
		}

		internal static bool Delete(NetworkConnection connection, Avatar avatar)
		{
			// TODO: Leave guild & guild academy.

			using (var p_Char_Delete = new StoredProcedure("p_Char_Delete", DB.GetDatabaseClient(DatabaseType.Character).Connection))
			{
				p_Char_Delete.AddParameter("nCharNo", avatar.CharNo);
				p_Char_Delete.AddOutput<int>("nRet");
				p_Char_Delete.Run();
			}

			return true;
		}
	}
}
