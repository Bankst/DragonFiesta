using System;

namespace DFEngine.Utils.ServerConsole
{
	public class ConsoleTitle
	{
		public string Title
		{
			get => Console.Title;
			set => Console.Title = value;
		}
	}
}
