using System;

namespace LoginServer
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Console.SetWindowSize(140, 40);
			ServerMain.Initialize();
		}
	}
}
