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

	    protected PacketBase()
        {
            Stream = new MemoryStream();
        }

	    protected PacketBase(byte[] buffer)
        {
            Stream = new MemoryStream(buffer);
        }

        public byte[] GetBuffer() => Stream.ToArray();

        #region IDisposable Support

        private bool _disposedValue = false; // Used to detect redundant calls.

		protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
					// TODO: Dispose managed state (managed objects).
				}

				_disposedValue = true;
            }
        }

		// This code is added to properly implement the Dispose pattern.
		public void Dispose()
        {
            Dispose(true);
        }

        #endregion IDisposable Support
    }
}