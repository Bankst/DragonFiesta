using System;
using System.Collections.Generic;
using DFEngine;
using DFEngine.Content.Game;
using DFEngine.Content.GameObjects;
using DFEngine.Database;
using DFEngine.Logging;
using DFEngine.Network;
using DFEngine.Server;
using DFEngine.Utils;

namespace ZoneServer.Services
{
	public class CharacterService
	{
		public static List<Character> OnlineCharacters = new List<Character>();

		public static bool TryLoadCharacter(string charName, out Character character, out ushort error)
		{
			error = (ushort)CharLoginError.FAILED_ZONESERVER; // default error
			character = new Character
			{
				Name = charName
			};

			using (var p_Char_GetCharNo = new StoredProcedure("p_Char_GetCharNo", ZoneServer.CharDb))
			{
				p_Char_GetCharNo.AddParameter("sID", charName, 40);
				p_Char_GetCharNo.AddOutput<int>("nCharNo");

				character.CharNo = p_Char_GetCharNo.Run().GetOutput<int>("nCharNo");
			}

			using (var p_Char_GetUserNo = new StoredProcedure("p_Char_GetUserNo", ZoneServer.CharDb))
			{
				p_Char_GetUserNo.AddParameter("nCharNo", character.CharNo);
				p_Char_GetUserNo.AddOutput<int>("nUserNo");

				character.UserNo = p_Char_GetUserNo.Run().GetOutput<int>("nUserNo");
			}

			using (var p_Char_GetAllData = new StoredProcedure("p_Char_GetAllData", ZoneServer.CharDb))
			{
				p_Char_GetAllData.AddParameter("nCharNo", character.CharNo);

				
				using (var reader = p_Char_GetAllData.RunReader())
				{
					if (!reader.HasRows)
					{
						DatabaseLog.Write(DatabaseLogLevel.Error, $"No character data found for {charName}");
					}

					/* Technically, this would update the character structure,
						for potentially every row that is found. But there should only be 
						one. Not sure if we should modify this or not (or if we even can). */
					while (reader.Read())
					{
						character.Slot = reader.GetByte(1);
						character.Level = (byte)reader.GetInt16(3);
						character.EXP = reader.GetInt64(4);
						character.Stats.CurrentHP = reader.GetInt16(5);
						character.Stats.CurrentSP = reader.GetInt16(6);
						character.Stats.CurrentLP = reader.GetInt16(7);
						character.Stats.CurrentHPStones = reader.GetInt16(9);
						character.Stats.CurrentSPStones = reader.GetInt16(10);
						character.Stats.CurrentPwrStones = reader.GetInt32(11);
						character.Stats.CurrentGrdStones = reader.GetInt32(12);
						character.Cen = reader.GetInt64(13);
						character.Fame = reader.GetInt32(14);
						character.Flags = reader.GetInt32(15);
						character.AdminLevel = reader.GetByte(16);
						character.Position = new Position
						{
							X = reader.GetInt32(18),
							Y = reader.GetInt32(19),
							D = reader.GetByte(20)
						};
						character.PrisonMinutes = (ushort)reader.GetInt16(21);
						character.KillPoints = reader.GetInt32(22);
						character.PKYellowTime = reader.GetByte(23);
						character.Stats.FreeSTR = reader.GetByte(24);
						character.Stats.FreeEND = reader.GetByte(25);
						character.Stats.FreeDEX = reader.GetByte(26);
						character.Stats.FreeINT = reader.GetByte(27);
						character.Stats.FreeSPR = reader.GetByte(28);
						character.StatPoints = reader.GetByte(29);

						if (!MapService.TryFindInstance(reader.GetString(17), out var map))
						{
							DatabaseLog.Write(DatabaseLogLevel.Error, $"Failed to find map instance for character: character->{charName}, map->{reader.GetString(17)}");
							error = (ushort) CharLoginError.MAP_UNDER_MAINT;
							return false;
						}

						character.Position.Map = map;
					}
				}
			}

			using (var p_Char_GetShape = new StoredProcedure("p_char_GetShape", ZoneServer.CharDb))
			{
				p_Char_GetShape.AddParameter("nCharNo", character.CharNo);

				using (var reader = p_Char_GetShape.RunReader())
				{
					if (!reader.HasRows)
					{
						DatabaseLog.Write(DatabaseLogLevel.Error, $"No character shape for character: {charName}");
						error = (ushort)CharLoginError.ERROR_APPEARANCE;
						return false;
					}

					while (reader.Read())
					{
						character.Shape.Race = reader.GetByte(0);
						character.Shape.Class = (CharacterClass) reader.GetByte(1);
						character.Shape.Gender = (Gender) reader.GetByte(2);
						character.Shape.Hair = reader.GetByte(3);
						character.Shape.HairColor = reader.GetByte(4);
						character.Shape.Face = reader.GetByte(5);
					}
				}
			}

			character.Handle = GameObjectAllocator.Allocate(GameObjectType.CHARACTER);
			character.Parameters = ZoneData.ParamServer[character.Shape.Class][character.Level];

			character.Stats.Update();
			return true;
		}

		public static void Logout(Character character, LogoutType logoutType = LogoutType.EXIT)
		{
			GameLog.Write(GameLogLevel.Internal, $"{character.Name} requested {logoutType} logout.");

			if (character.IsLoggedOut)
			{
				return;
			}

			OnlineCharacters.Remove(character);
			SaveCharacter(character);
			character.IsLoggedOut = true;
			character.Client.Disconnect();
		}

		public static void SaveCharacter(Character character)
		{
			using (var p_Char_SaveLevelExpFame = new StoredProcedure("p_Char_SaveLevelExpFame", ZoneServer.CharDb))
			{
				p_Char_SaveLevelExpFame.AddParameter("nCharNo", character.CharNo);
				p_Char_SaveLevelExpFame.AddParameter("nLevel", character.Level);
				p_Char_SaveLevelExpFame.AddParameter("nExp", character.EXP);
				p_Char_SaveLevelExpFame.AddParameter("nFame", character.Fame);
				p_Char_SaveLevelExpFame.Run();
			}

			using (var p_Char_MoneySet = new StoredProcedure("p_Char_MoneySet", ZoneServer.CharDb))
			{
				p_Char_MoneySet.AddParameter("nCharNo", character.CharNo);
				p_Char_MoneySet.AddParameter("nSetMoney", character.Cen);
				p_Char_MoneySet.AddOutput<byte>("nRet");
				p_Char_MoneySet.Run();
			}

			using (var p_User_MoneySet = new StoredProcedure("p_User_MoneySet", ZoneServer.CharDb))
			{
				p_User_MoneySet.AddParameter("nUserNo", character.UserNo);
				p_User_MoneySet.AddParameter("nSetMoney", character.UserCen);
				p_User_MoneySet.AddOutput<byte>("nRet");
				p_User_MoneySet.Run();
			}

			using (var p_Char_SaveLocation = new StoredProcedure("p_Char_SaveLocation", ZoneServer.CharDb))
			{
				p_Char_SaveLocation.AddParameter("nCharNo", character.CharNo);
				p_Char_SaveLocation.AddParameter("sLoginZone", character.Position.Map.Info.MapName, 16);
				p_Char_SaveLocation.AddParameter("nLoginZoneX", character.Position.X);
				p_Char_SaveLocation.AddParameter("nLoginZoneY", character.Position.Y);
				p_Char_SaveLocation.AddParameter("nLoginZoneD", character.Position.D);
				p_Char_SaveLocation.AddParameter("nKQHandle", 0);
				p_Char_SaveLocation.AddParameter("sKQMap", "", 16);
				p_Char_SaveLocation.AddParameter("nKQX", 0);
				p_Char_SaveLocation.AddParameter("nKQY", 0);
				p_Char_SaveLocation.Run();
			}

			using (var p_Char_SavePKCount = new StoredProcedure("p_Char_SavePKCount", ZoneServer.CharDb))
			{
				p_Char_SavePKCount.AddParameter("nCharNo", character.CharNo);
				p_Char_SavePKCount.AddParameter("nPKCount", character.KillPoints);
				p_Char_SavePKCount.Run();
			}

			using (var p_Char_SaveStat = new StoredProcedure("p_Char_SaveStat", ZoneServer.CharDb))
			{
				p_Char_SaveStat.AddParameter("nCharNo", character.CharNo);
				p_Char_SaveStat.AddParameter("nHPS", character.Stats.CurrentHPStones);
				p_Char_SaveStat.AddParameter("nSPS", character.Stats.CurrentSPStones);
				p_Char_SaveStat.AddParameter("nHP", character.Stats.CurrentHP);
				p_Char_SaveStat.AddParameter("nSP", character.Stats.CurrentSP);
				p_Char_SaveStat.AddParameter("nLP", character.Stats.CurrentLP);
				p_Char_SaveStat.Run();
			}

			using (var p_Char_RedistributePointSet = new StoredProcedure("p_Char_RedistributePointSet", ZoneServer.CharDb))
			{
				p_Char_RedistributePointSet.AddParameter("nCharNo", character.CharNo);
				p_Char_RedistributePointSet.AddParameter("nSetPoint", character.StatPoints);
				p_Char_RedistributePointSet.AddOutput<byte>("nRet");
				p_Char_RedistributePointSet.Run();
			}
		}

		public static void ChangeHP(Character character, int HP)
		{
			character.Stats.CurrentHP = Math.Max(HP, character.Stats.CurrentMinHP);
			character.Stats.CurrentHP = Math.Min(HP, character.Stats.CurrentMaxHP);
			character.NextMHHPTick = Time.Milliseconds + character.MiniHouse.MiniHouse.HPTick;
			new PROTO_NC_BAT_HPCHANGE_CMD(character).Send(character);
			character.ToSelectedBy(c => new PROTO_NC_BAT_TARGETINFO_CMD(0, character).Send((Character)c), true);
			// TODO: Parties!
			/*
			if (character.PartyMember == null)
				return;
			character.PartyMember.CurrentHP = character.Stats.CurrentHP;
			new PROTO_NC_PARTY_MEMBERINFORM_CMD((byte)1, character.PartyMember).Broadcast(character.Party, true, character.Position.Map, new PartyMember[1]
			{
				character.PartyMember
			});
			*/
		}

		public static void ChangeSP(Character character, int SP)
		{
			character.Stats.CurrentSP = Math.Max(SP, character.Stats.CurrentMinSP);
			character.Stats.CurrentSP = Math.Min(SP, character.Stats.CurrentMaxSP);
			character.NextMHSPTick = Time.Milliseconds + character.MiniHouse.MiniHouse.SPTick;
			new PROTO_NC_BAT_SPCHANGE_CMD(character).Send(character);
			character.ToSelectedBy(c => new PROTO_NC_BAT_TARGETINFO_CMD(0, character).Send((Character)c), true);
			// TODO: Parties!
			/*
			if (character.PartyMember == null)
				return;
			character.PartyMember.CurrentSP = character.Stats.CurrentSP;
			new PROTO_NC_PARTY_MEMBERINFORM_CMD((byte)1, character.PartyMember).Broadcast(character.Party, true, character.Position.Map, new PartyMember[1]
			{
				character.PartyMember
			});
			*/
		}

		internal static void ChangeLP(Character character, int LP)
		{
			character.Stats.CurrentLP = LP;
			character.LastLPUpdate = Time.Milliseconds;
			new PROTO_NC_BAT_LPCHANGE_CMD(character).Send(character);
			character.ToSelectedBy(c => new PROTO_NC_BAT_TARGETINFO_CMD(0, character).Send((Character)c), true);
			// TODO: Parties!
			/*
			if (character.PartyMember == null)
				return;
			character.PartyMember.CurrentLP = character.Stats.CurrentLP;
			new PROTO_NC_PARTY_MEMBERINFORM_CMD((byte)1, character.PartyMember).Broadcast(character.Party, true, character.Position.Map, new PartyMember[1]
			{
				character.PartyMember
			});
			*/
		}
	}
}
