using System;
using System.Collections.Generic;
using System.Text;
using DFEngine.IO.Definitions;
using DFEngine.Logging;
using DFEngine.Network;
using DFEngine.Network.Protocols;
using DFEngine.Server;
using IgniteEngine.Protocols;
using ZoneServer.Services;

namespace ZoneServer.Handlers
{
	internal static class MapHandlers
	{
		internal static void NC_MAP_LOGIN_REQ(NetworkMessage message, NetworkConnection connection)
		{
			var handle = message.ReadUInt16();
			var charName = message.ReadString(20);

			for (byte i = 0; i < (byte) SHN_DATA_FILE_INDEX.SHN_MaxCnt; i++)
			{
				try
				{
					var clientChecksum = message.ReadString(32);

					if (!ZoneData.SHNChecksums.TryGetValue((SHN_DATA_FILE_INDEX) i, out var serverChecksum))
					{
						continue;
					}

					if (string.Compare(clientChecksum.ToLower(), serverChecksum.ToLower(), StringComparison.Ordinal) ==
					    0) continue;
					SocketLog.Write(SocketLogLevel.Warning, $"Client had Manipulated file: {(SHN_DATA_FILE_INDEX) i}");
					new PROTO_NC_MAP_LOGINFAIL_ACK((ushort) CharLoginError.CLIENT_MANIPULATED, i).Send(connection);
					connection.Disconnect();
					return;
				}
				catch
				{
					new PROTO_NC_MAP_LOGINFAIL_ACK((ushort) CharLoginError.CLIENT_MANIPULATED, i).Send(connection);
					connection.Disconnect();
					return;
				}
			}

			if (!CharacterService.TryLoadCharacter(charName, out var character))
			{
				new PROTO_NC_CHAR_LOGINFAIL_ACK((ushort) CharLoginError.FAILED_ZONESERVER).Send(connection);
				return;
			}


		}
	}
}
