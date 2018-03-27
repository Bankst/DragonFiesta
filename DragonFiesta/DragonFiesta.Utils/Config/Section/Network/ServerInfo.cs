using System;

namespace DragonFiesta.Utils.Config.Section.Network
{
    [Serializable]
    public class ServerInfo
    {
        public string ListeningIP { get; set; } = "127.0.0.1";

        public virtual ushort ListeningPort { get; set; } = 8888;

        public string Password { get; set; } = "Dubistdoof";

        public ushort MaxConnection { get; set; }
    }
}