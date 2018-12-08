using System;

using DFEngine.IO.Definitions;
using DFEngine.Logging;
using DFEngine.Network;
using DFEngine.Network.Protocols;
using DFEngine.Server;

using ZoneServer.Services;

namespace ZoneServer.Handlers
{
	internal static class MapHandlers
	{
		internal static void NC_MAP_LOGIN_REQ(NetworkMessage message, NetworkConnection client)
		{
			var handle = message.ReadUInt16();
			var charName = message.ReadString(20);

			if (ZoneServer.ZoneConfig.CheckSHNHash)
			{
				for (byte i = 0; i < (byte)SHN_DATA_FILE_INDEX.SHN_MaxCnt; i++)
				{
					try
					{
						var clientChecksum = message.ReadString(32);

						if (!ZoneData.SHNChecksums.TryGetValue((SHN_DATA_FILE_INDEX)i, out var serverChecksum))
						{
							continue;
						}

						if (string.Compare(clientChecksum.ToLower(), serverChecksum.ToLower(), StringComparison.Ordinal) == 0) continue;
						SocketLog.Write(SocketLogLevel.Warning, $"Client had Manipulated file: {(SHN_DATA_FILE_INDEX)i}");
						new PROTO_NC_MAP_LOGINFAIL_ACK((ushort)CharLoginError.CLIENT_MANIPULATED, i).Send(client);
						client.Disconnect();
						return;
					}
					catch
					{
						new PROTO_NC_MAP_LOGINFAIL_ACK((ushort)CharLoginError.CLIENT_MANIPULATED, i).Send(client);
						client.Disconnect();
						return;
					}
				}
			}

			if (!CharacterService.TryLoadCharacter(charName, out var character, out var error))
			{
				new PROTO_NC_MAP_LOGINFAIL_ACK(error, 0).Send(client);
				client.Disconnect();
				return;
			}

			SocketLog.Write(SocketLogLevel.Debug, $"{charName} is logging in");

			if (character.Position.Map != null)
			{

			}

			character.Client = client;

			client.Handle = handle;
			client.Character = character;

			character.Login();
		}
	}
}
