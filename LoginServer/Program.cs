using System;

namespace LoginServer
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			// Attempt to size window for best log view
			try { Console.SetWindowSize(140, 40); }
			catch { }
			
			LoginServer.Initialize();
		}
	}
}
