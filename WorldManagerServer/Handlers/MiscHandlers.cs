using DFEngine.Network;
using DFEngine.Network.Protocols;

namespace WorldManagerServer.Handlers
{
	internal static class MiscHandlers
	{
		internal static void NC_MISC_SEED_ACK(NetworkMessage message, NetworkConnection connection)
		{
			connection.SetSeed(message.ReadUInt16());
		}

		internal static void NC_MISC_GAMETIME_REQ(NetworkMessage message, NetworkConnection connection)
		{
			new PROTO_NC_MISC_GAMETIME_ACK().Send(connection);
		}

		internal static void NC_MISC_S2SCONNECTION_RDY(NetworkMessage message, NetworkConnection connection)
		{
			new PROTO_NC_MISC_S2SCONNECTION_REQ(
				NetworkConnectionType.NCT_WORLDMANAGER, // From 
				NetworkConnectionType.NCT_LOGIN, // To
				ServerMain.NetConfig.WorldNetConfig.WorldID, // World ID
				"Isya", // World Name TODO: Add this to WorldConfiguration/NetworkConfiguration
				0, // ZoneNo TODO: do this programatically?
				ServerMain.NetConfig.WorldNetConfig.ListenIP, // Client Listen IP
				(ushort)ServerMain.NetConfig.WorldNetConfig.ListenPort // Client Listen Port
			).Send(connection);
		}

		internal static void NC_MISC_S2SCONNECTION_ACK(NetworkMessage message, NetworkConnection connection)
		{
			
			/* Shine Table Data */
			// TODO: Add SHN/Script loading
			/*
			using (var file = new Script($"{Program.ShinePath}DefaultCharacterData.txt"))
			{
				using (var reader = new ScriptReader(file["CHARACTER"]))
				{
					if (reader.HasRows)
					{
						while (reader.Read())
						{
							Program.DefaultCharacterData.Add((CharacterClass)reader.GetByte(0), new DefaultCharacterData
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
							Program.DefaultCharacterData.GetSafe((CharacterClass)reader.GetByte(0))?.Items.Add(new Tuple<ushort, byte>(reader.GetUInt16(1), reader.GetByte(2)));
						}
					}
				}

				using (var reader = new ScriptReader(file["SKILL"]))
				{
					if (reader.HasRows)
					{
						while (reader.Read())
						{
							Program.DefaultCharacterData.GetSafe((CharacterClass)reader.GetByte(0))?.Skills.Add(reader.GetUInt16(1));
						}
					}
				}

				using (var reader = new ScriptReader(file["QUEST"]))
				{
					if (reader.HasRows)
					{
						while (reader.Read())
						{
							Program.DefaultCharacterData.GetSafe((CharacterClass)reader.GetByte(0))?.Quests.Add(reader.GetInt32(1));
						}
					}
				}

				using (var reader = new ScriptReader(file["SHORTCUT"]))
				{
					if (reader.HasRows)
					{
						while (reader.Read())
						{
							Program.DefaultCharacterData.GetSafe((CharacterClass)reader.GetByte(0))?.Shortcuts.Add(new Shortcut
							{
								Slot = reader.GetByte(1),
								Code = reader.GetUInt16(2),
								Value = reader.GetUInt16(3)
							});
						}
					}
				}
			}
			

			SHNFile.LoadFromFolder(Program.ShinePath,
				"HairInfo.shn",
				"HairColorInfo.shn",
				"FaceInfo.shn",
				"SingleData.shn");

			SHNFile.TryGetObjects("HairInfo", out Program.HairInfo);
			SHNFile.TryGetObjects("HairColorInfo", out Program.HairColorInfo);
			SHNFile.TryGetObjects("FaceInfo", out Program.FaceInfo);
			SHNFile.TryGetObjects("SingleData", out Program.SingleData);

			*/
		}
	}
}
