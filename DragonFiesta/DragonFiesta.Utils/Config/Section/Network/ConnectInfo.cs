using System;

namespace DragonFiesta.Utils.Config.Section.Network
{
    [Serializable]
    public class ConnectInfo
    {
        public virtual string ConnectIP { get; set; } = "127.0.0.1"; //using to Connect Anothers

        public virtual int ConnectPort { get; set; } = 8888;

        public virtual string Password { get; set; } = "Dubistdoof";
    }
}