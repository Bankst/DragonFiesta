using System;

namespace DragonFiesta.Utils.IO.SHN
{
    public class SHNCrypto
    {
 
        public static void CryptoDefault(byte[] Data)
        {
            byte DataLength = (byte)Data.Length;
            for (Int32 Counter = Data.Length - 1; Counter >= 0; Counter--)
            {
                Data[Counter] = (byte)(Data[Counter] ^ DataLength);

                byte DL = (byte)Counter;

                DL = (byte)(DL & 15);
                DL = (byte)(DL + 0x55);
                DL = (byte)(DL ^ ((byte)(((byte)Counter) * 11)));
                DL = (byte)(DL ^ DataLength);
                DL = (byte)(DL ^ 170);

                DataLength = DL;
            }
        }

        private static Byte Method1(Byte DL, Byte Number) { return (Byte)(DL & Number); }

        private static Byte Method2(Byte DL, Byte Number) { return (Byte)(DL + Number); }

        private static Byte Method3(Byte DL, Int32 Counter, Byte Number) { return (Byte)(DL ^ ((Byte)(((Byte)Counter) * Number))); }

        private static Byte Method4(Byte DL, Byte Number) { return (Byte)(DL ^ Number); }
    }
}
