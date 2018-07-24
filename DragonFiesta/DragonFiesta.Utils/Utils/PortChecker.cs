using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DragonFiesta.Utils.Utils
{
	public class PortChecker
	{
		public static bool IsPortOpen(string host, int port, int timeoutMillis)
		{
			try
			{
				using (var client = new TcpClient())
				{
					var result = client.BeginConnect(host, port, null, null);
					var success = result.AsyncWaitHandle.WaitOne(timeoutMillis);
					if (!success)
					{
						return false;
					}

					client.EndConnect(result);
				}
			}
			catch
			{
				return false;
			}
			return true;
		}
	}
}
