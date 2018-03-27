using System;

namespace DragonFiesta.Networking.Network
{
    public abstract class DataParserBase
    {
        public event EventHandler<DataRecievedEventArgs> OnDataRecv;

        public abstract void ParseNext(byte[] ReadBuffer, ref int Offset, int ReadLength);

        internal void InvokeDataRecv(DataRecievedEventArgs args) => OnDataRecv?.Invoke(this, args);

        ~DataParserBase()
        {
            OnDataRecv = null;
        }
    }
}