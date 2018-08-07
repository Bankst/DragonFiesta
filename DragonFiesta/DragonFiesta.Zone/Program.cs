using DragonFiesta.Zone.Core;

namespace DragonFiesta.Zone
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 1 && byte.TryParse(args[0], out byte ZoneId))
            {
                if (!ServerMain.Initialize(ZoneId))
                {
                    ServerMain.InternalInstance.Shutdown();
                }
            }
            else if (!ServerMain.Initialize())
            {
                ServerMain.InternalInstance.Shutdown();
            }
        }
    }
}