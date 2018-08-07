/// <summary>
///  Tells Another Zones Message and Called on WolrdServer Handler when exis
/// </summary>
public interface IZoneToZoneMessage : IMessage
{
    byte DestZone { get; set; }
}