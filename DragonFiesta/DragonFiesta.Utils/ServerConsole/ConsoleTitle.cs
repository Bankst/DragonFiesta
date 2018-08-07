using System;

namespace DragonFiesta.Utils.ServerConsole
{
    public class ConsoleTitle
    {
        public string Title
        {
            get
            {
                return Console.Title;
            }
            set
            {
                Console.Title = value;
            }
        }
    }
}