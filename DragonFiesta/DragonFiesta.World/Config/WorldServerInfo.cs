using DragonFiesta.Utils.Config.Section.Network;
using System;

namespace DragonFiesta.World.Config
{
    [Serializable]
    public class WorldServerInfo : ExternServerInfo
    {
        private ushort WorldPort = 9110;
        public override ushort ListeningPort { get => WorldPort; set => WorldPort = value; }
    }
}