using System;

namespace ZoneServer
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			// Attempt to size window for best log view
			try { Console.SetWindowSize(140, 40); }
			catch { }

			if (args.Length == 1 && byte.TryParse(args[0], out var zoneId))
			{
				ZoneServer.Initialize(zoneId);
			}
			else
			{
				ZoneServer.Initialize();
			}
		}
	}
}
