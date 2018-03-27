using System;

namespace DragonFiesta.Login.Network
{
    public sealed class IPBlockEntry
    {
        public string IP { get; private set; }
        public DateTime Date { get; private set; }
        public string Reason { get; private set; }

        public IPBlockEntry(string IP, DateTime Date, string Reason)
        {
            this.IP = IP;
            this.Date = Date;
            this.Reason = Reason;
        }

        public void Dispose()
        {
            IP = null;
            Reason = null;
        }

        ~IPBlockEntry()
        {
            Dispose();
        }
    }
}