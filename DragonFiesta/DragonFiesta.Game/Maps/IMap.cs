using DragonFiesta.Providers.Maps;
using System.Runtime.Serialization;

public interface IMap : ISerializable
{
    MapInfo MapInfo { get; }
    byte ZoneId { get; }
    ushort MapId { get; }

    bool Start();

    bool Stop();
}