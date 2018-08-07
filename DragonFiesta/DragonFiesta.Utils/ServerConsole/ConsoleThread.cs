using System;
using System.Threading;
using System.Windows.Forms;

namespace DragonFiesta.Utils.ServerConsole
{
    public static class ConsoleThread
    {
        private static bool _reading;

        private static Thread CmdThread { get; set; }

        public static void Stop()
        {
            
            _reading = false;
            CmdThread.Abort();

            SendEnter();
        }

      private static void SendEnter() =>
            SendKeys.SendWait("~");
        public static void Start()
        {
            _reading = true;
            CmdThread = new Thread(Read) { Name = "CmdThread" };
            CmdThread.Start();
        }

        private static void Read()
        {
            while (_reading)
            {
                var input = Console.ReadLine();
                var args = input?.Split(' ');
	            if (args?.Length < 1) continue;
	            var cmd = args?[0];
	            if (!ConsoleCommandHandlerStore.InvokeConsoleCommand(cmd?.ToUpper(), args))
	            {
		            CommandLog.Write(CommandLogLevel.Error, "Can't find Command {0}", input);
	            }
            }
        }
    }
}