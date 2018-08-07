using System;
using System.Collections.Generic;

namespace DragonFiesta.Networking.Network
{
    public class ClientStateInfo
    {
        public ClientStateInfo()
        {
            Storage = new Dictionary<string, object>();
            storageLock = new object();
        }

        public object this[string pName]
        {
            get
            {
                lock (storageLock)
                {
                    if (!Storage.ContainsKey(pName))
                    {
                        return null;
                    }
                    return Storage[pName];
                }
            }
            set { lock (storageLock) Storage[pName] = value; }
        }

        public DateTime TimestampOfLastPing
        {
            get { return (DateTime)this[nameof(TimestampOfLastPing)]; }
            set { this[nameof(TimestampOfLastPing)] = value; }
        }

        public Int32 PacketsReceived
        {
            get { return (Int32)(this[nameof(PacketsReceived)] ?? default(Int32)); }
            set { this[nameof(PacketsReceived)] = value; }
        }

        public Int32 PacketsSent
        {
            get { return (Int32)(this[nameof(PacketsSent)] ?? default(Int32)); }
            set { this[nameof(PacketsSent)] = value; }
        }

        protected Dictionary<string, object> Storage;
        protected object storageLock;

        public ushort SessionId { get; set; } = 0;
    }
}