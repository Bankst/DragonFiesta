using System;

namespace DragonFiesta.Utils.Config.Section.Network
{
    [Serializable]
    public class ExternServerInfo : ServerInfo
    {
		public string ExternalIP { get; set; } = "127.0.0.1";
    }
}