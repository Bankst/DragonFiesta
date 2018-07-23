#region

using System.Runtime.Serialization;

#endregion

public interface IInstanceMap : IMap, ISerializable
{
    ushort InstanceId { get; }
}