using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DragonFiesta.Utils.Utils
{
	public class PortChecker
	{
		public static bool IsPortOpen(string host, int port, int timeoutMillis = 50)
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
					client.Close();
				}
			}
			catch
			{
				return false;
			}
			return true;
		}

		public static void WaitForPort(string host, int port, bool autoExit = false, int timeout = 5000)
		{
			var watch = Stopwatch.StartNew();
			while (!IsPortOpen(host, port, 10))
			{
				if (!autoExit) continue;
				if (watch.ElapsedMilliseconds >= timeout) break;
			}
		}
	}
}
