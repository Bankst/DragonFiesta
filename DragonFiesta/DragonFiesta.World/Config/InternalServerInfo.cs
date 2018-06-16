using DragonFiesta.Utils.Config.Section.Network;
using System;

namespace DragonFiesta.World.Config
{
    [Serializable]
    public class InternalServerInfo : ExternServerInfo
    {
        public override ushort ListeningPort { get; set; } = 8820;
    }
}