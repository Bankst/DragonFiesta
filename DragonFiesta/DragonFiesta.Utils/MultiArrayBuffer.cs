using System;
using System.Collections.Concurrent;
using System.Threading;

namespace DragonFiesta.Utils
{
    public class MultiArrayBuffer<T> : IDisposable
    {
        private T[] currentBuffer;
        private object currentBufferLock;
        private int positionInCurrentBuffer;
        private ConcurrentQueue<T[]> elements;
        private int elementsRemaining;

        public MultiArrayBuffer()
        {
            elements = new ConcurrentQueue<T[]>();
            elementsRemaining = 0;
            currentBuffer = new T[0];
            positionInCurrentBuffer = 0;
            currentBufferLock = new object();
        }

        public int ElementsRemaining => elementsRemaining;

        public void AppendBuffer(T[] data, int offset, int length)
        {
            T[] newBuffer = new T[length];
            Array.Copy(data, offset, newBuffer, 0, length);
            lock (elements)
            {
                elements.Enqueue(newBuffer);
                if (elements.Count == 1)
                {
                    Monitor.PulseAll(elements);
                }
                elementsRemaining += length;
            }
        }

        public T[] ReadBuffer(int length, bool waitForNewBuffers = false)
        {
            T[] buffer = new T[length];
            int elementsRead = 0;
            while (elementsRead < length)
            {
                if (length - elementsRead > ElementsRemainingInCurrentBuffer())
                {
                    lock (currentBufferLock)
                    {
                        Array.Copy(currentBuffer, positionInCurrentBuffer, buffer, elementsRead,
                            ElementsRemainingInCurrentBuffer());
                        elementsRead += ElementsRemainingInCurrentBuffer();
                        NextBuffer(waitForNewBuffers);
                    }
                }
                else
                {
                    lock (currentBufferLock)
                    {
                        Array.Copy(currentBuffer, positionInCurrentBuffer, buffer, elementsRead, length - elementsRead);
                        positionInCurrentBuffer += length - elementsRead;
                        elementsRead = length;
                    }
                }
            }
            elementsRemaining -= elementsRead;
            return buffer;
        }

        public bool WaitForNewBuffers(TimeSpan timeout)
        {
            DateTime startTime = DateTime.Now;

            while (elements.Count == 0 && (DateTime.Now - startTime) <= timeout)
            {
                lock (elements)
                    Monitor.Wait(elements, timeout);
            }
            if ((DateTime.Now - startTime) >= timeout)
                return false;

            if (Monitor.TryEnter(elements, timeout))
            {
                while (elements.Count == 0 && (DateTime.Now - startTime) <= timeout)
                {
                    Monitor.Wait(elements, timeout);
                }
                if ((DateTime.Now - startTime) <= timeout)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        public void WaitForNewBuffers()
        {
            while (elements.Count == 0)
                lock (elements)
                    Monitor.Wait(elements);
        }

        protected internal int ElementsRemainingInCurrentBuffer()
        {
            return currentBuffer.Length - positionInCurrentBuffer;
        }

        protected internal void NextBuffer(bool waitForNewBuffers)
        {
            while (elements.Count == 0)
            {
                Monitor.Enter(elements);
                if (elements.Count == 0)
                    Monitor.Wait(elements);
                Monitor.Exit(elements);
            }
            lock (elements)
            {
                T[] res;
                if (elements.TryDequeue(out res))
                {
                    currentBuffer = res;
                    positionInCurrentBuffer = 0;
                }
            }
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    while (!elements.IsEmpty)
                    {
                        T[] e;
                        elements.TryDequeue(out e);
                    }
                    elementsRemaining = 0;
                }
                currentBuffer = null;
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~MultiArrayBuffer() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}