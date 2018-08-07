#region

using System.Runtime.Serialization;
using DragonFiesta.Utils.Config.Section.Network;

#endregion

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