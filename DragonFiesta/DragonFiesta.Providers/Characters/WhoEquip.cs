using System;
using System.Collections.Concurrent;

namespace DragonFiesta.Providers.Characters
{
    public class WhoEquip
    {
        private ConcurrentDictionary<byte, bool> Classes;

        public WhoEquip(uint Key)
            : this()
        {
            ExtractClasses(this, Key);
        }

        private WhoEquip()
        {
            Classes = new ConcurrentDictionary<byte, bool>();
        }

        public void Dispose()
        {
            if (Classes != null)
                Classes.Clear();
            Classes = null;
        }

        ~WhoEquip()
        {
            Dispose();
        }

        private static void ExtractClasses(WhoEquip WhoEquip, uint Key)
        {
            for (byte i = 1; i < 26; i++)
            {
                if ((Key & (uint)Math.Pow(2, i)) != 0)
                {
                    WhoEquip.Classes.TryAdd(i, true);
                }
            }
        }

        public bool CanEquip(ClassId ClassID)
        {
            return CanEquip(ClassID);
        }

        public bool CanEquip(byte ClassID)
        {
            return Classes.ContainsKey(ClassID);
        }
    }
}