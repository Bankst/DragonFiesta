namespace DragonFiesta.World
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (!ServerMain.Initialize())
            {
                ServerMain.InternalInstance.Shutdown();
            }
        }
    }
}