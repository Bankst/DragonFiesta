using DragonFiesta.Networking.Packet;
using Microsoft.IO;
using System;
using System.IO;

public class FiestaPacket : PacketBase
{
    private static RecyclableMemoryStreamManager memoryStreamManager = new RecyclableMemoryStreamManager();

    private FiestaPacketReader Reader { get; set; }

    private PacketWriter Writer { get; set; }

    public ushort OpCode { get; private set; }

    public byte Header { get; private set; }
    public UInt16 Type { get; private set; }

    public FiestaPacket(byte[] pBuffer) : base(pBuffer) //Incomming Packets
    {
        Reader = new FiestaPacketReader(Stream, DefaultEncoding);
        ReadHeaderAndType();
    }

    public FiestaPacket(byte header, UInt16 type) : base()//Outgoing Packets
    {
        Header = header;
        Type = type;
        Writer = new FiestaPacketWriter(Stream, DefaultEncoding);
    }

    #region Read

    protected void ReadHeaderAndType()
    {
        if (Read(out ushort Data))
        {
            Header = (byte)(Data >> 10);
            Type = (byte)(Data & 1023);
        }
    }

    public bool Read<T>(out T Obj) where T : struct
    {
        if (!Reader.Read<T>(out Obj))
            return false;

        return true;
    }

    public bool ReadString(out string Value) => Reader.ReadString(out Value);

    public bool ReadString(out string Value, int Length) => Reader.ReadString(out Value, Length);

    public bool ReadEncodeString(out string mString, int pLength) => Reader.ReadEncodeString(out mString, pLength);

    public bool SkipBytes(int pCount) => Reader.SkipBytes(pCount);

    public bool ReadBytes(int pLength, out byte[] Bytes) => Reader.ReadBytes(pLength, out Bytes);

    #endregion Read

    #region Write

    public void Write(byte[] Bytes) => Writer.Write<byte[]>(Bytes);

    public void Write<T>(object Obj) where T : struct
    {
        if (!Writer.IsTypeRegistered(typeof(T)))
        {
            throw new InvalidOperationException("Write Type is not exis...");
        }

        Writer.Write<T>(Obj);
    }

    protected byte[] HeaderAndTypeToBytes()
    {
        return BitConverter.GetBytes(GetRealOpcode());
    }

    public void WriteString(string pData, int pLength) => Writer.WriteString(pData, pLength);

    public void WriteHexAsBytes(string Hex) => Writer.WriteHexAsBytes(Hex);

    public void Fill(int pLength, byte pValue) => Writer.Fill(pLength, pValue);

    #endregion Write

    private ushort GetRealOpcode()
    {
        // Magic. Do not touch.
        return (ushort)((Header << 10) + (Type & 1023));
    }

    protected byte[] BytesForSize()
    {
        if (Stream.Length <= Byte.MaxValue)
        {
            return new[] { (byte)(Stream.Length + 2) };
        }
        else
        {
            byte[] buffer = new byte[3];
            buffer[0] = 0;
            Array.Copy(BitConverter.GetBytes((ushort)(Stream.Length + 2)), 0, buffer, 1, 2);
            return buffer;
        }
    }

    public override string ToString() => $"({Header}-{Type}) Length {Stream.Length}";

    public byte[] GetPacketBytes()
    {
        using (var buffer = memoryStreamManager.GetStream("FiestaPacket.GetBytes"))
        {
            byte[] sizeBytes = BytesForSize();
            byte[] headerAndTypeBytes = HeaderAndTypeToBytes();

            buffer.Write(sizeBytes, 0, sizeBytes.Length);
            buffer.Write(headerAndTypeBytes, 0, headerAndTypeBytes.Length);
            Stream.Seek(0, SeekOrigin.Begin);
            Stream.CopyTo(buffer);

            return buffer.ToArray();
        }
    }

    protected override void Dispose(bool disposing)
    {
        Reader = null;
        Writer = null;
        base.Dispose(disposing);
    }
}