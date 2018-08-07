namespace DragonFiesta.Login.Game.Worlds
{
    public class WorldConnectionInfo
    {
        public string WorldIP { get; set; } = "127.0.0.1";

		public string WorldExternalIP { get; set; } = "127.0.0.1";

        public ushort WorldPort { get; set; } = 0;

        public int MaxPlayers { get; set; } = 1;

        ~WorldConnectionInfo()
        {
            WorldIP = null;
			WorldExternalIP = null;
        }
    }
}
