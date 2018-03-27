using System.Runtime.Serialization;

public interface IInstanceMap : IMap, ISerializable
{
    ushort InstanceId { get; }
}