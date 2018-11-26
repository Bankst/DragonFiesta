using System;

namespace DFEngine.Utils.ServerConsole
{
	public class ConsoleTitle
	{
		public static readonly string DebuggingString = System.Diagnostics.Debugger.IsAttached ? "(Debugging)" : "";

		public virtual void Update(string titleString, params object[] args)
		{
			Title = string.Format(titleString + DebuggingString, args);
		}

		public string Title
		{
			get => Console.Title;
			set => Console.Title = value;
		}
	}
}
