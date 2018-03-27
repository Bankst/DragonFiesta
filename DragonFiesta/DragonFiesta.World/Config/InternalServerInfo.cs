using DragonFiesta.Utils.Config.Section.Network;
using System;

namespace DragonFiesta.World.Config
{
    [Serializable]
    public class InternalServerInfo : ServerInfo
    {
        public override ushort ListeningPort { get; set; } = 8820;
    }
}