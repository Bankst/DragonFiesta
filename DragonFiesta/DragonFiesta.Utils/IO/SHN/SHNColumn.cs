using System;

namespace DragonFiesta.Utils.IO.SHN
{
    public class SHNColumn
    {
        public Int32 ID { get; set; }
        public String Name { get; set; }
        public UInt32 Type { get; set; }
        public Int32 Length { get; set; }

        public Type GetType()
        {
            switch (Type)
            {
                default:
                    { return typeof(Object); }
                case 1:
                case 12:
                    { return typeof(Byte); }
                case 2:
                    { return typeof(UInt16); }
                case 3:
                case 11:
                    { return typeof(UInt32); }
                case 0x15:
                case 13:
                    { return typeof(Int16); }
                case 0x10:
                    { return typeof(Byte); }
                case 0x12:
                case 0x1b:
                    { return typeof(UInt32); }
                case 20:
                    { return typeof(SByte); }
                case 0x16:
                    { return typeof(Int32); }
                case 5:
                    { return typeof(Single); }
                case 0x18:
                case 0x1a:
                case 9:
                    { return typeof(String); }
                case 0x1d:
                    { return typeof(String); }
            }
        }
    }
}
