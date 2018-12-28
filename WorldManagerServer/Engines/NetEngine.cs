using System;
using System.Collections.Generic;
using System.Text;
using DFEngine;
using DFEngine.Content.Game.Engines;
using DFEngine.Logging;
using DFEngine.Network;
using DFEngine.Threading;
using DFEngine.Utils;

namespace WorldManagerServer.Engines
{
	internal class NetEngine : IEngine
	{
		private static readonly Dictionary<NetworkConnection, long> ConnectionsToPing = new Dictionary<NetworkConnection, long>();

		public void Main(long now)
		{
			ConnectionsToPing.ForBackwards((connection, l) =>
			{
				if (now - l < ServerTaskTimes.SESSION_GAME_PING_SYNC.Milliseconds())
				{
					return;
				}

				if (!connection.IsConnected)
				{
					RemovePing(connection);
				}
				else if (now - connection.LastPing >= ServerTaskTimes.SESSION_GAME_PING_TIMEOUT.Milliseconds())
				{
					SocketLog.Write(SocketLogLevel.Warning, $"Disconnecting {connection.Account?.Username} due to a ping timeout.");
					RemovePing(connection);

					connection.Disconnect();
				}
				else
				{
					new PROTO_NC_MISC_HEARTBEAT_REQ().Send(connection);
					ConnectionsToPing[connection] = now;
				}
			});
		}

		internal static void AddPing(NetworkConnection connection)
		{
			if (!ConnectionsToPing.ContainsKey(connection))
			{
				ConnectionsToPing.AddSafe(connection, Time.Milliseconds);
			}
		}

		internal static void RemovePing(NetworkConnection connection)
		{
			if (ConnectionsToPing.ContainsKey(connection))
			{
				ConnectionsToPing.Remove(connection);
			}
		}
	}
}
