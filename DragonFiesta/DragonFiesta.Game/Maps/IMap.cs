#region

using System.Runtime.Serialization;
using DragonFiesta.Providers.Maps;

#endregion

public interface IMap : ISerializable
{
    MapInfo MapInfo { get; }
    byte ZoneId { get; }
    ushort MapId { get; }

    bool Start();

    bool Stop();
}