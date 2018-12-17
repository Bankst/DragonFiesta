using System;
using System.Collections.Generic;
using System.IO;
using DFEngine;
using DFEngine.Content.Game;
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
		internal static byte[] DEFAULT_GAME_SETTINGS =
		{
			0x1C, 0x00, 0x00, 0x00, 0x01, 0x01, 0x00, 0x01, 0x02, 0x00, 0x01, 0x03, 0x00, 0x01, 0x04, 0x00, 0x00, 0x05,
			0x00, 0x01, 0x06, 0x00, 0x00, 0x07, 0x00, 0x01, 0x08, 0x00, 0x01, 0x09, 0x00, 0x01, 0x0A, 0x00, 0x01, 0x0B,
			0x00, 0x01, 0x0C, 0x00, 0x00, 0x0D, 0x00, 0x00, 0x0E, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x10, 0x00, 0x01, 0x11,
			0x00, 0x00, 0x12, 0x00, 0x00, 0x13, 0x00, 0x00, 0x14, 0x00, 0x01, 0x15, 0x00, 0x00, 0x16, 0x00, 0x00, 0x17,
			0x00, 0x00, 0x18, 0x00, 0x01, 0x19, 0x00, 0x01, 0x1A, 0x00, 0x01, 0x1B, 0x00, 0x01
		};

		internal static byte[] DEFAULT_KEYMAP_SETTINGS =
		{
			0x5F, 0x00, 0x00, 0x00, 0x00, 0x79, 0x01, 0x00, 0x00, 0x1B, 0x02, 0x00, 0x00, 0x43, 0x03, 0x00, 0x00, 0x49,
			0x04, 0x00, 0x00, 0x4B, 0x05, 0x00, 0x00, 0x4C, 0x06, 0x00, 0x00, 0x46, 0x07, 0x00, 0x00, 0x48, 0x08, 0x00,
			0x00, 0x56, 0x09, 0x00, 0x00, 0x0D, 0x0A, 0x00, 0x11, 0x4E, 0x0B, 0x00, 0x11, 0x47, 0x0C, 0x00, 0x11, 0x50,
			0x0D, 0x00, 0x11, 0x57, 0x0E, 0x00, 0x00, 0xDE, 0x0F, 0x00, 0x00, 0x58, 0x10, 0x00, 0x00, 0x47, 0x11, 0x00,
			0x00, 0x00, 0x12, 0x00, 0x00, 0x00, 0x13, 0x00, 0x00, 0x00, 0x14, 0x00, 0x00, 0x00, 0x15, 0x00, 0x00, 0x52,
			0x16, 0x00, 0x11, 0x41, 0x17, 0x00, 0x00, 0x57, 0x18, 0x00, 0x00, 0x53, 0x19, 0x00, 0x00, 0x00, 0x1A, 0x00,
			0x00, 0x41, 0x1B, 0x00, 0x00, 0x44, 0x1C, 0x00, 0x00, 0x5A, 0x1D, 0x00, 0x00, 0x20, 0x1E, 0x00, 0x00, 0x26,
			0x1F, 0x00, 0x00, 0x28, 0x20, 0x00, 0x00, 0x25, 0x21, 0x00, 0x00, 0x27, 0x22, 0x00, 0x00, 0x24, 0x23, 0x00,
			0x00, 0x54, 0x24, 0x00, 0x00, 0x51, 0x25, 0x00, 0x00, 0x45, 0x26, 0x00, 0x00, 0xF5, 0x27, 0x00, 0x00, 0x42,
			0x28, 0x00, 0x00, 0x50, 0x29, 0x00, 0x00, 0x4D, 0x2A, 0x00, 0x00, 0x55, 0x2B, 0x00, 0x10, 0x5A, 0x2C, 0x00,
			0x00, 0x00, 0x2D, 0x00, 0x00, 0x00, 0x2E, 0x00, 0x00, 0x23, 0x2F, 0x00, 0x00, 0x31, 0x30, 0x00, 0x00, 0x32,
			0x31, 0x00, 0x00, 0x33, 0x32, 0x00, 0x00, 0x34, 0x33, 0x00, 0x00, 0x35, 0x34, 0x00, 0x00, 0x36, 0x35, 0x00,
			0x00, 0x37, 0x36, 0x00, 0x00, 0x38, 0x37, 0x00, 0x00, 0x39, 0x38, 0x00, 0x00, 0x30, 0x39, 0x00, 0x00, 0xBD,
			0x3A, 0x00, 0x00, 0xBB, 0x3B, 0x00, 0x10, 0x31, 0x3C, 0x00, 0x10, 0x32, 0x3D, 0x00, 0x10, 0x33, 0x3E, 0x00,
			0x10, 0x34, 0x3F, 0x00, 0x10, 0x35, 0x40, 0x00, 0x10, 0x36, 0x41, 0x00, 0x10, 0x37, 0x42, 0x00, 0x10, 0x38,
			0x43, 0x00, 0x10, 0x39, 0x44, 0x00, 0x10, 0x30, 0x45, 0x00, 0x10, 0xBD, 0x46, 0x00, 0x10, 0xBB, 0x47, 0x00,
			0x12, 0x31, 0x48, 0x00, 0x12, 0x32, 0x49, 0x00, 0x12, 0x33, 0x4A, 0x00, 0x12, 0x34, 0x4B, 0x00, 0x12, 0x35,
			0x4C, 0x00, 0x12, 0x36, 0x4D, 0x00, 0x12, 0x37, 0x4E, 0x00, 0x12, 0x38, 0x4F, 0x00, 0x12, 0x39, 0x50, 0x00,
			0x12, 0x30, 0x51, 0x00, 0x12, 0xBD, 0x52, 0x00, 0x12, 0xBB, 0x53, 0x00, 0x00, 0x00, 0x54, 0x00, 0x00, 0x00,
			0x55, 0x00, 0x00, 0x00, 0x56, 0x00, 0x00, 0x00, 0x57, 0x00, 0x00, 0x00, 0x58, 0x00, 0x00, 0x00, 0x59, 0x00,
			0x00, 0x00, 0x5A, 0x00, 0x00, 0x00, 0x5B, 0x00, 0x00, 0x00, 0x5C, 0x00, 0x00, 0x00, 0x5D, 0x00, 0x00, 0x00,
			0x5E, 0x00, 0x00, 0x00
		};

		internal static byte[] DEFAULT_SHORTCUT_SETTINGS = {0x00, 0x00};

		internal static byte[] DEFAULT_SHORTCUTSIZE_SETTINGS =
		{
			0x05, 0x00, 0x03, 0x18, 0x00, 0x00, 0x05, 0x00, 0x0C, 0x00, 0x00, 0x0C, 0x01, 0x00, 0x0C, 0x02, 0x00, 0x0C,
			0x03, 0x00, 0x0C, 0x04, 0x00, 0x00
		};

		internal static byte[] DEFAULT_WINDOWPOS_SETTINGS =
		{
			0x40, 0xF4, 0xFD, 0x1E, 0xAD, 0x14, 0x1D, 0x77, 0x98, 0x9D, 0x58, 0x1D, 0x68, 0x9D, 0x58, 0x1D, 0x98, 0x9D,
			0x58, 0x1D, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x72, 0x00, 0x40, 0x1B, 0x0F, 0x23,
			0x7C, 0x9D, 0x58, 0x1D, 0x40, 0x1B, 0x0F, 0x23, 0x0C, 0x00, 0x0B, 0x00, 0xBD, 0x08, 0x00, 0x00, 0x7C, 0x9D,
			0x58, 0x1D, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x28, 0x1B, 0x0F, 0x23, 0x01, 0x00, 0x00, 0x00,
			0x38, 0x1B, 0x0F, 0x23, 0x90, 0xF4, 0xFD, 0x1E, 0xA3, 0xE0, 0xD4, 0x77, 0x40, 0x1B, 0x0F, 0x23, 0x00, 0x00,
			0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x38, 0x1B, 0x0F, 0x23, 0xDC, 0xF4, 0xFD, 0x1E, 0xCD, 0x98, 0xCE, 0x75,
			0x00, 0x00, 0x72, 0x00, 0x00, 0x00, 0x00, 0x00, 0xDA, 0x98, 0xCE, 0x75, 0xF8, 0xA7, 0xDF, 0x50, 0x00, 0x00,
			0x00, 0x00, 0x38, 0x63, 0xD9, 0x10, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
			0x38, 0x63, 0xD9, 0x10, 0xCC, 0xF4, 0xFD, 0x1E, 0xA4, 0xF4, 0xFD, 0x1E, 0xDC, 0xCC, 0x38, 0x01, 0xB4, 0xF8,
			0xFD, 0x1E, 0xD5, 0x8C, 0xD0, 0x75, 0xC4, 0xCB, 0xEC, 0x3B, 0x00, 0xF5, 0xFD, 0x1E, 0x06, 0xE4, 0xEE, 0x74,
			0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x8C, 0xD2, 0xF6, 0x0C, 0x38, 0x63, 0xD9, 0x10, 0x51, 0x53,
			0xF9, 0x0C, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x40, 0x1B, 0x0F, 0x23, 0x18, 0xF5, 0xFD, 0x1E,
			0xA5, 0xBE, 0xA8, 0x00, 0x38, 0x63, 0xD9, 0x10, 0x00, 0x00, 0x00, 0x00, 0xFD, 0xFF, 0xFF, 0xFF, 0x51, 0x53,
			0xF9, 0x0C, 0xB8, 0xF5, 0xFD, 0x1E, 0x0E, 0x6A, 0xA3, 0x00, 0x9C, 0xE3, 0xF6, 0x0C, 0x7F, 0x06, 0x00, 0x00,
			0x14, 0x00, 0x00, 0x00, 0x88, 0xD2, 0xF6, 0x0C, 0x57, 0x6A, 0xA3, 0x00, 0x14, 0xF5, 0xFD, 0x1E, 0xDC, 0xCC,
			0x38, 0x01, 0xB4, 0xF8, 0xFD, 0x1E, 0xD5, 0x8C, 0xD0, 0x75, 0xC4, 0xCB, 0xEC, 0x3B, 0x70, 0xF5, 0xFD, 0x1E,
			0x06, 0xE4, 0xEE, 0x74, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x53, 0x53, 0xF9, 0x0C, 0x38, 0x63,
			0xD9, 0x10, 0x53, 0x53, 0xF9, 0x0C, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x98, 0x83, 0x0D, 0x23,
			0x8C, 0xF5, 0xFD, 0x1E, 0x51, 0xB0, 0xA8, 0x00, 0x9A, 0x7A, 0xDC, 0x50, 0xB8, 0xF5, 0xFD, 0x1E, 0x7A, 0xFF,
			0xA4, 0x00, 0x9C, 0xE3, 0xF6, 0x0C, 0x94, 0xF5, 0xFD, 0x1E, 0x88, 0xD2, 0xF6, 0x0C, 0x8B, 0xFF, 0xA4, 0x00,
			0x00, 0x52, 0x68, 0x7C, 0x2F, 0xDD, 0x05, 0x05, 0x08, 0x07, 0xA6, 0x00, 0x8C, 0x03, 0x39, 0x0D, 0x72, 0x42,
			0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xC0, 0xF5, 0xFD, 0x1E, 0x04, 0x1E, 0xA8, 0x00
		};

		internal static List<Avatar> LoadAll(int userNo)
		{
			var ret = new List<Avatar>();

			using (var p_Char_GetListOfUserChar = new StoredProcedure("p_Char_GetListOfUserChar", WorldManagerServer.CharDb))
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

			using (var p_Char_GetLoginData = new StoredProcedure("p_Char_GetLoginData", WorldManagerServer.CharDb))
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
						avatar.TutorialState = (TutorialState)reader.GetByte(22);
						avatar.TutorialStep = reader.GetByte(23);
					}
				}
			}

			using (var usp_Tutorial_GetCharInfo = new StoredProcedure("usp_Tutorial_GetCharInfo", WorldManagerServer.CharDb))
			{
				usp_Tutorial_GetCharInfo.AddParameter("nCharNo", charNo);
				usp_Tutorial_GetCharInfo.AddOutput<int>("nRet");

				using (var reader = usp_Tutorial_GetCharInfo.RunReader())
				{
					if (!reader.HasRows)
					{
						return false;
					}

					while (reader.Read())
					{
						avatar.TutorialState = (TutorialState) reader.GetByte(0);
						avatar.TutorialStep = reader.GetByte(1);
					}
				}
			}

			// load equipment
			using (var p_Item_GetListType = new StoredProcedure("p_Item_GetListType", WorldManagerServer.CharDb))
			{
				p_Item_GetListType.AddParameter("nCharNo", avatar.CharNo);
				p_Item_GetListType.AddParameter("nStorageType", (byte) InventoryType.EQUIPPED);

				using (var reader = p_Item_GetListType.RunReader())
				{
					if (!reader.HasRows)
					{
						DatabaseLog.Write(DatabaseLogLevel.Warning, $"No equipped items for avatar - {avatar.Name}");
//						return false;
					}

					while (reader.Read())
					{
						var equip = (ItemEquip) (byte) reader.GetValue(1);
						var equipped = avatar.Equipment[equip];

						if (equipped != null)
						{
							DatabaseLog.Write(DatabaseLogLevel.Warning, $"Already existing item in slot for character - {avatar.Name} | {equip}");
							avatar.Equipment.Items.Remove(equipped);
						}

						var id = Convert.ToUInt16(reader.GetValue(2));
						var info = WorldData.ItemInfo[id];

						if (info == null)
						{
							DatabaseLog.Write(DatabaseLogLevel.Error, $"Avatar has item equipped that doesn't exist in ItemInfo.. - {avatar.Name} | id");
							continue;
						}

						avatar.Equipment.Items.Add(new Item
						{
							Key = Convert.ToInt64(reader.GetValue(0)),
							Slot = (ItemSlot) reader.GetByte(1),
							Info = info
						});
					}
				}
			}

			using (var p_Char_GetOptShortCutData = new StoredProcedure("p_Char_GetOptShortCutData", WorldManagerServer.CharDb))
			{
				p_Char_GetOptShortCutData.AddParameter("nCharNo", avatar.CharNo);
				p_Char_GetOptShortCutData.AddOutput<int>("nRet");
				p_Char_GetOptShortCutData.AddOutput<byte[]>("sData", 1024);
				 
				var data = p_Char_GetOptShortCutData.Run().GetOutput<byte[]>("sData");
				avatar.ShortcutData = data ?? DEFAULT_SHORTCUT_SETTINGS;
			}

			using (var p_Char_GetOptKeyMapping = new StoredProcedure("p_Char_GetOptKeyMapping", WorldManagerServer.CharDb))
{
				p_Char_GetOptKeyMapping.AddParameter("nCharNo", avatar.CharNo);
				p_Char_GetOptKeyMapping.AddOutput<int>("nRet");
				p_Char_GetOptKeyMapping.AddOutput<byte[]>("sData", 382);

				var data = p_Char_GetOptKeyMapping.Run().GetOutput<byte[]>("sData");
				avatar.KeyMapData = data ?? DEFAULT_KEYMAP_SETTINGS;
			}

			using (var p_Char_GetOptGame = new StoredProcedure("p_Char_GetOptGame", WorldManagerServer.CharDb))
{
				p_Char_GetOptGame.AddParameter("nCharNo", avatar.CharNo);
				p_Char_GetOptGame.AddOutput<int>("nRet");
				p_Char_GetOptGame.AddOutput<byte[]>("sData", 86);

				var data = p_Char_GetOptGame.Run().GetOutput<byte[]>("sData");
				avatar.GameOptionData = data ?? DEFAULT_GAME_SETTINGS;
			}

			using (var p_Char_GetOptShortCutSize = new StoredProcedure("p_Char_GetOptShortCutSize", WorldManagerServer.CharDb))
			{
				p_Char_GetOptShortCutSize.AddParameter("nCharNo", avatar.CharNo);
				p_Char_GetOptShortCutSize.AddOutput<int>("nRet");
				p_Char_GetOptShortCutSize.AddOutput<byte[]>("sData", 24);

				var data = p_Char_GetOptShortCutSize.Run().GetOutput<byte[]>("sData");
				avatar.ShortcutSizeData = data ?? DEFAULT_SHORTCUTSIZE_SETTINGS;
			}


			using (var p_Char_GetOptWindowPos = new StoredProcedure("p_Char_GetOptwindowspos", WorldManagerServer.CharDb))
			{
				p_Char_GetOptWindowPos.AddParameter("nCharNo", avatar.CharNo);
				p_Char_GetOptWindowPos.AddOutput<int>("nRet");
				p_Char_GetOptWindowPos.AddOutput<byte[]>("sData", 392);

				var data = p_Char_GetOptWindowPos.Run().GetOutput<byte[]>("sData");
				avatar.WindowPosData = data ?? DEFAULT_WINDOWPOS_SETTINGS;
			}

			return true;
		}

		internal static bool IsNameInUse(string name)
		{
			using (var p_Char_Find = new StoredProcedure("p_Char_Find", WorldManagerServer.CharDb))
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

			using (var p_Char_Create = new StoredProcedure("p_Char_Create", WorldManagerServer.CharDb))
			{
				p_Char_Create.AddParameter("nUserNo", connection.Account.UserNo);
				p_Char_Create.AddParameter("nCreateWorld", WorldManagerServer.NetConfig.WorldNetConfig.WorldID);
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

			if (WorldData.DefaultCharacterData.TryGetValue(@class, out var defaultValues))
			{
				using (var p_Char_CreateSetDefaultData = new StoredProcedure("p_Char_CreateSetDefaultData", WorldManagerServer.CharDb))
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

				using (var p_Char_RedistributePointSet = new StoredProcedure("p_Char_RedistributePointSet", WorldManagerServer.CharDb))
				{
					p_Char_RedistributePointSet.AddParameter("nCharNo", charNo);
					p_Char_RedistributePointSet.AddParameter("nSetPoint", defaultValues.Level);
					p_Char_RedistributePointSet.AddOutput<byte>("nRet");
					p_Char_RedistributePointSet.Run();
				}

				foreach (var item in defaultValues.Items)
				{
					long itemKey;

					using (var p_Item_Create = new StoredProcedure("p_Item_Create", WorldManagerServer.CharDb))
					{
						p_Item_Create.AddParameter("nOwner", charNo);
						p_Item_Create.AddParameter("nStorageType", (byte)InventoryType.CHAR_INVENTORY);
						p_Item_Create.AddParameter("nStorage", (byte)defaultValues.Items.IndexOf(item));
						p_Item_Create.AddParameter("nItemID", (int)item.Item1);
						p_Item_Create.AddParameter("nFlags", 0);
						p_Item_Create.AddOutput<long>("nItemKey");
						p_Item_Create.AddOutput<int>("nRet");
						itemKey = p_Item_Create.Run().GetOutput<long>("nItemKey");
					}

					using (var p_Item_SetOption = new StoredProcedure("p_Item_SetOption", WorldManagerServer.CharDb))
					{
						p_Item_SetOption.AddParameter("nItemKey", itemKey);
						p_Item_SetOption.AddParameter("nOptionType", 1);
						p_Item_SetOption.AddParameter("nOptionData", item.Item2);
						p_Item_SetOption.AddOutput<int>("nRet");
						p_Item_SetOption.Run();
					}
				}

				long houseKey;

				using (var p_Item_Create = new StoredProcedure("p_Item_Create", WorldManagerServer.CharDb))
				{
					p_Item_Create.AddParameter("nOwner", charNo);
					p_Item_Create.AddParameter("nStorageType", (byte)InventoryType.MINIHOUSE_SKIN);
					p_Item_Create.AddParameter("nStorage", 0);
					p_Item_Create.AddParameter("nItemID", Convert.ToInt32(31000));
					p_Item_Create.AddParameter("nFlags", 0);
					p_Item_Create.AddOutput<long>("nItemKey");
					p_Item_Create.AddOutput<int>("nRet");
					houseKey = p_Item_Create.Run().GetOutput<long>("nItemKey");
				}

				using (var p_Item_SetOption = new StoredProcedure("p_Item_SetOption", WorldManagerServer.CharDb))
				{
					p_Item_SetOption.AddParameter("nItemKey", houseKey);
					p_Item_SetOption.AddParameter("nOptionType", 1);
					p_Item_SetOption.AddParameter("nOptionData", 1);
					p_Item_SetOption.AddOutput<int>("nRet");
					p_Item_SetOption.Run();
				}

				foreach (var quest in defaultValues.Quests)
				{
					using (var p_Quest_Add = new StoredProcedure("p_Quest_Add", WorldManagerServer.CharDb))
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
					using (var p_Skill_Set = new StoredProcedure("p_Skill_Set", WorldManagerServer.CharDb))
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

				using (var p_Char_SetOptShortCutData = new StoredProcedure("p_Char_SetOptShortCutData", WorldManagerServer.CharDb))
				{
					p_Char_SetOptShortCutData.AddParameter("nCharNo", charNo);
					p_Char_SetOptShortCutData.AddParameter("sData", shortcutData, 1024);
					p_Char_SetOptShortCutData.AddOutput<int>("nRet");
					p_Char_SetOptShortCutData.Run();
				}

				using (var usp_Tutorial_SetCharInfo = new StoredProcedure("usp_Tutorial_SetCharInfo", WorldManagerServer.CharDb))
				{
					usp_Tutorial_SetCharInfo.AddParameter("nUserNo", connection.Account.UserNo);
					usp_Tutorial_SetCharInfo.AddParameter("nCharNo", charNo);
					usp_Tutorial_SetCharInfo.AddParameter("nState", (byte) 0);
					usp_Tutorial_SetCharInfo.AddParameter("nStep", (byte) 0);
					usp_Tutorial_SetCharInfo.AddOutput<int>("nRet");


					if (usp_Tutorial_SetCharInfo.Run().GetOutput<int>("nRet") < 0)
					{
						DatabaseLog.Write(DatabaseLogLevel.DatabaseClientError, $"SQLError -> usp_Tutorial_setCharInfo : charNo - {charNo}");
					}
				}

				return TryLoad(charNo, out avatar);
			}

			DatabaseLog.Write(DatabaseLogLevel.Warning, $"No default character data found for {@class}.");
			return false;
		}

		internal static bool Delete(NetworkConnection connection, Avatar avatar)
		{
			// TODO: Leave guild & guild academy.

			using (var p_Char_Delete = new StoredProcedure("p_Char_Delete", WorldManagerServer.CharDb))
			{
				p_Char_Delete.AddParameter("nCharNo", avatar.CharNo);
				p_Char_Delete.AddOutput<int>("nRet");
				p_Char_Delete.Run();
			}

			return true;
		}
	}
}
