using DragonFiesta.Utils.Config.Section.Network;
using System.Runtime.Serialization;

namespace DragonFiesta.Game.Zone
{
    public interface IZone : ISerializable
    {
        byte ID { get; }
        ExternServerInfo NetInfo { get; }
        bool IsConnected { get; }
        bool IsReady { get; }
    }
}