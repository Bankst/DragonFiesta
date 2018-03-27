using System;
using System.Threading;
using System.Windows.Forms;

namespace DragonFiesta.Utils.ServerConsole
{
    public static class ConsoleThread
    {
        private static bool Reading;

        private static Thread CmdThread { get; set; }

        public static void Stop()
        {
            
            Reading = false;
            CmdThread.Abort();

            SendEnter();
        }

      private static void SendEnter() =>
            SendKeys.SendWait("~");
        public static void Start()
        {
            Reading = true;
            CmdThread = new Thread(Read) { Name = string.Concat("CmdThread") };
            CmdThread.Start();
        }

        private static void Read()
        {
            while (Reading)
            {
                string Input = Console.ReadLine();
                string[] args = Input.Split(' ');
                if (args.Length >= 1)
                {
                    string cmd = args[0];
                    if (!ConsoleCommandHandlerStore.InvokeConsoleCommand(cmd.ToUpper(), args))
                    {
                        CommandLog.Write(CommandLogLevel.Error, "Can't find Command {0}", Input);
                    }
                }
            }
        }
    }
}