using System;
using System.Collections.Generic;
using System.Text;
using DFEngine.Network;

namespace WorldManagerServer.Handlers
{
	public static class LocalHandlers
	{
		public static void NC_LOCAL_ADDTRANSFER_CMD(NetworkMessage message, NetworkConnection connection)
		{
			var validateNew = message.ReadString(64);
//			if (WorldManagerServer.Transfers.Contains(validateNew))
//			WorldManagerServer.Transfers.Add
		}
	}
}
