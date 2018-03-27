using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DragonFiesta.Networking.Packet
{
    public class PacketWriter
    {
        protected Encoding Encoding { get; set; }
        private BinaryWriter Writer { get; set; }
        private Dictionary<Type, Action<object, BinaryWriter>> WriteMethods;

        public PacketWriter(MemoryStream Stream, Encoding Encode)
        {
            Encoding = Encode;
            Writer = new BinaryWriter(Stream);
            WriteMethods = new Dictionary<Type, Action<object, BinaryWriter>>();
            RegisterPrimitiveTypeWriteMethods();
        }

        public void Write<T>(object pObj)
        {
            if (WriteMethods.ContainsKey(typeof(T)))
            {
                WriteMethods[typeof(T)](pObj, Writer);
            }
            else
            {
                throw new InvalidOperationException("No method registered for given type");
            }
        }

        public void WriteString(string pData, int pLength)
        {
            byte[] data = new byte[pLength];
            // fill w/ 00-bytes.
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = 0;
            }
            byte[] encoded = Encoding.GetBytes(pData);
            for (int i = 0; i < Math.Min(encoded.Length, data.Length); i++)
            {
                data[i] = encoded[i];
            }
            Write<byte[]>(data);
        }

        public void WriteHexAsBytes(string Hex)
        {
            Write<byte[]>(Hex.HexToBytes());
        }

        public void Fill(int pLength, byte pValue)
        {
            for (int i = 0; i < pLength; ++i)
            {
                Write<byte>(pValue);
            }
        }

        protected void RegisterPrimitiveTypeWriteMethods()
        {
            RegisterMethod(typeof(Boolean), (o, w) => w.Write(Convert.ToBoolean(o)));
            RegisterMethod(typeof(Byte), (o, w) => w.Write(Convert.ToByte(o)));
            RegisterMethod(typeof(SByte), (o, w) => w.Write(Convert.ToSByte(o)));
            RegisterMethod(typeof(Int16), (o, w) => w.Write(Convert.ToInt16(o)));
            RegisterMethod(typeof(UInt16), (o, w) => w.Write(Convert.ToUInt16(o)));
            RegisterMethod(typeof(Int32), (o, w) => w.Write(Convert.ToInt32(o)));
            RegisterMethod(typeof(UInt32), (o, w) => w.Write(Convert.ToUInt32(o)));
            RegisterMethod(typeof(Int64), (o, w) => w.Write(Convert.ToInt64(o)));
            RegisterMethod(typeof(UInt64), (o, w) => w.Write(Convert.ToUInt64(o)));
            RegisterMethod(typeof(byte[]), (o, w) => w.Write((byte[])o));
        }

        internal void RegisterMethod(Type pType, Action<object, BinaryWriter> pAction)
        {
            if (WriteMethods.ContainsKey(pType))
            {
                throw new ArgumentException("Cannot register the same Type twice for PacketWriter");
            }
            WriteMethods.Add(pType, pAction);
        }

        public bool IsTypeRegistered(Type pType)
        {
            return WriteMethods.ContainsKey(pType);
        }
    }
}