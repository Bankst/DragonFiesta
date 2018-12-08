using System.Data;
using System.Diagnostics;
using System.IO;
using DFEngine.Content.Game;
using DFEngine.Content.GameObjects;
using DFEngine.Database;
using DFEngine.Logging;
using DFEngine.Server;

namespace ZoneServer.Services
{
	public class CharacterService
	{
		public static bool TryLoadCharacter(string charName, out Character character, out ushort error)
		{
			error = (ushort)CharLoginError.FAILED_ZONESERVER; // default error
			character = new Character()
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
	}
}
