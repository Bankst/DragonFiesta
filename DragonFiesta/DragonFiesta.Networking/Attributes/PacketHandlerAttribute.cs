using System;

[AttributeUsage(AttributeTargets.Class)]
public class PacketHandlerClassAttribute : Attribute
{
    public PacketHandlerClassAttribute(byte header)
    {
        this.Header = header;
    }

    public byte Header { get; private set; }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class PacketHandlerAttribute : Attribute
{
    public PacketHandlerAttribute(ushort Type, ClientRegion Region) //For region
    {
        this.Region = Region;
        this.Type = Type;
    }

    public PacketHandlerAttribute(ushort Type)//For ALL Clients
    {
        Region = ClientRegion.None;
        this.Type = Type;
    }

    public ClientRegion Region { get; private set; }

    public ushort Type { get; private set; }
}