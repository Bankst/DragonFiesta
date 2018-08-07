using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace DragonFiesta.Networking.Packet
{
    public class PacketReader
    {
        private Encoding Encoding { get; set; }
        private long Remaining => Stream.Length - Stream.Position;

        private MemoryStream Stream { get; set; }
        private BinaryReader Reader { get; set; }
        private Dictionary<Type, dynamic> ReadFunctions;

        internal delegate bool PacketAction<T>(out T a);
        public PacketReader(MemoryStream Stream, Encoding Coder)
        {
            this.Encoding = Coder;
            this.Stream = Stream;
            Reader = new BinaryReader(Stream);

            ReadFunctions = new Dictionary<Type, dynamic>();

            RegisterPrimitiveTypeReadMethods();
        }

        internal void RegisterMethod<T>(Type pType, PacketAction<T> ReadAction) where T : struct
        {
            if (ReadFunctions.ContainsKey(pType))
            {
                throw new ArgumentException("Cannot register the same Type twice on PacketReader");
            }
            ReadFunctions.Add(typeof(T), ReadAction);
        }

        private void RegisterPrimitiveTypeReadMethods()
        {
            RegisterMethod(typeof(Boolean), (out bool o) => ReadBool(out o));
            RegisterMethod(typeof(Byte), (out Byte o) => ReadPacket(out o));
            RegisterMethod(typeof(SByte), (out SByte o) => ReadPacket(out o));
            RegisterMethod(typeof(Int16), (out Int16 o) => ReadPacket(out o));
            RegisterMethod(typeof(UInt16), (out UInt16 o) => ReadPacket(out o));
            RegisterMethod(typeof(Int32), (out Int32 o) => ReadPacket(out o));
            RegisterMethod(typeof(UInt32), (out UInt32 o) => ReadPacket(out o));
            RegisterMethod(typeof(Int64), (out Int64 o) => ReadPacket(out o));
            RegisterMethod(typeof(UInt64), (out UInt64 o) => ReadPacket(out o));
        }


        private bool ReadBool(out bool Result)
        {
            Result = false;

            if (!Read(out byte NumberResult))
                return false;

            Result = (NumberResult == 1);

            return true;
        }
        public bool Read<T>(out T Object) where T : struct
        {
            Object = default(T);

            if (!IsTypeRegistered(typeof(T)))
                return false;

            return ReadFunctions[typeof(T)].Invoke(out Object);
        }

        private bool ReadPacket<T>(out T Object) where T : struct
        {
            // Marshall Size of the Generic Type T
            int size = Marshal.SizeOf(typeof(T));
            Object = default(T);

            if (size < 0)
            {
                return false;
            }

            //Check if Can read...
            if ((Stream.Position + size) > Stream.Length)
            {
                return false;
            }

            // Read Blank Byte[]
            byte[] DataInByte = new byte[size];
            Array.Copy(Stream.ToArray(), Stream.Position, DataInByte, 0, size);
            // Convert to the right type.
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(DataInByte, 0, ptr, size);
            Object = (T)Marshal.PtrToStructure(ptr, typeof(T));
            Marshal.FreeHGlobal(ptr);

            // Increase Index
            Stream.Position += size;

            // Return
            return true;
        }

        public bool IsTypeRegistered(Type pType)
        {
            // again a simple wrapper, but makes things cleaner
            return ReadFunctions.ContainsKey(pType);
        }

        public bool ReadString(out string Value)
        {
            Value = "";

            if (!ReadPacket(out byte length))
                return false;

            return ReadString(out Value, length);
        }

        public bool ReadString(out string Value, int Length)
        {
            Value = "";

            if (!ReadBytes(Length, out byte[] buffer))
                return false;

            //remove nulls
            var nullsLength = 0;
            if (buffer[(Length - 1)] != 0)
            {
                nullsLength = Length;
            }
            else
            {
                while (buffer[nullsLength] != 0x00
                    && nullsLength < Length)
                {
                    nullsLength++;
                }
            }

            if (Length > 0)
            {
                Value = Encoding.GetString(buffer, 0, nullsLength);
            }

            return true;
        }

        public bool ReadEncodeString(out string mString, int pLength)
        {
            // we cannot simply wrap this as we need to specify the length of the string
            if (ReadBytes(pLength, out byte[] buffer))
            {
                string data = Encoding.ASCII.GetString(buffer);
                mString = data.Trim().Replace("\0", "");

                return mString != null;
            }
            mString = null;
            return false;
        }

        public bool SkipBytes(int pCount)
        {
            // the same as reading bytes, but discarding the return value

            if (!ReadBytes(pCount, out byte[] skip))
                return false;

            return true;
        }

        public bool ReadBytes(int pLength, out byte[] Bytes)
        {
            Bytes = null;

            if (Reader.BaseStream.Position + pLength > Reader.BaseStream.Length) return false;
            // this cannot be wrapped easily, as we need to specify just how many bytes we want to read
            Bytes = Reader.ReadBytes(pLength);
            return true;
        }
    }
}