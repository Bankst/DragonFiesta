using System;
using System.IO;
using System.Text;

namespace DragonFiesta.Networking.Packet
{
    public abstract class PacketBase : IDisposable
    {
        public static readonly Encoding DefaultEncoding = Encoding.UTF8;

        public virtual Encoding Encoding { get; protected set; } = DefaultEncoding;

        public MemoryStream Stream { get; private set; }

        public PacketBase()
        {
            Stream = new MemoryStream();
        }

        public PacketBase(byte[] Buffer)
        {
            Stream = new MemoryStream(Buffer);
        }

        public byte[] GetBuffer() => Stream.ToArray();

        #region IDisposable Support

        private bool disposedValue = false; // Dient zur Erkennung redundanter Aufrufe.

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: verwalteten Zustand (verwaltete Objekte) entsorgen.
                }

                disposedValue = true;
            }
        }

        // Dieser Code wird hinzugefügt, um das Dispose-Muster richtig zu implementieren.
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion IDisposable Support
    }
}