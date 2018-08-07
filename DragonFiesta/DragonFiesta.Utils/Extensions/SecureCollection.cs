using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

public class SecureCollection<T> : IEnumerable<T>
{
    public int Count
    {
        get
        {
            int count;

            lock (ThreadLocker)
            {
                count = List.Count;
            }

            return count;
        }
    }

    public object ThreadLocker { get; private set; }

    public bool IsDisposed { get { return (IsDisposedInt > 0); } }
    private int IsDisposedInt;

    private List<T> List;

    public SecureCollection()
    {
        Load();
    }

    public SecureCollection(IEnumerable<T> Collection)
        : this()
    {
        List.AddRange(Collection);
    }

    public void Dispose()
    {
        if (Interlocked.CompareExchange(ref IsDisposedInt, 1, 0) == 0)
        {
            DisposeInternal();

            List.Clear();
            List = null;

            ThreadLocker = null;
        }
    }

    protected virtual void DisposeInternal()
    {
    }

    ~SecureCollection()
    {
        
        Dispose();
    }

    // IEnumerable
    IEnumerator IEnumerable.GetEnumerator()
    {
        return CreateEnumerator(this);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return CreateEnumerator(this);
    }

    private static SecureCollectionEnumerator<T> CreateEnumerator(SecureCollection<T> col)
    {
        List<T> listCopy;

        lock (col.ThreadLocker)
        {
            listCopy = new List<T>(col.List);
        }

        return new SecureCollectionEnumerator<T>(listCopy);
    }

    private void Load()
    {
        List = new List<T>();
        ThreadLocker = new object();
    }

    public ReadOnlyCollection<T> AsReadOnly()
    {
        ReadOnlyCollection<T> col;

        lock (ThreadLocker)
        {
            col = List.AsReadOnly();
        }

        return col;
    }

    public void ForEach(Action<T> Action)
    {
        lock (ThreadLocker)
        {
            List.ForEach(Action);
        }
    }

    public void CopyTo(T[] Array)
    {
        lock (ThreadLocker)
        {
            List.CopyTo(Array);
        }
    }

    public void CopyTo(T[] Array, int ArrayIndex)
    {
        lock (ThreadLocker)
        {
            List.CopyTo(Array, ArrayIndex);
        }
    }

    public void CopyTo(int Index, T[] Array, int ArrayIndex, int Count)
    {
        lock (ThreadLocker)
        {
            List.CopyTo(Index, Array, ArrayIndex, Count);
        }
    }

    public T this[int Index]
    {
        get
        {
            lock (ThreadLocker)
            {
                return List[Index];
            }
        }
        set
        {
            lock (ThreadLocker)
            {
                List[Index] = value;
            }
        }
    }

    public T Find(Predicate<T> Match)
    {
        T result;

        lock (ThreadLocker)
        {
            result = List.Find(Match);
        }

        return result;
    }

    public virtual bool Contains(T Object)
    {
        lock (ThreadLocker)
        {
            return List.Contains(Object);
        }
    }

    public virtual void Clear()
    {
        if (List != null)
        {
            lock (ThreadLocker)
            {
                List.Clear();
            }
        }
    }

    public virtual bool Add(T Object)
    {
        lock (ThreadLocker)
        {
            if (!List.Contains(Object))
            {
                List.Add(Object);

                return true;
            }

            return false;
        }
    }

    public virtual bool AddRange(IEnumerable<T> Collection)
    {
        lock (ThreadLocker)
        {
            List.AddRange(Collection);

            return true;
        }
    }

    public virtual bool SecureAddRange(IEnumerable<T> Collection)
    {
        lock (ThreadLocker)
        {
            foreach (var obj in Collection)
            {
                Add(obj);
            }

            return true;
        }
    }

    public virtual bool Remove(T Object)
    {
        lock (ThreadLocker)
        {
            if (List.Contains(Object))
            {
                List.Remove(Object);

                return true;
            }

            return false;
        }
    }
}

internal sealed class SecureCollectionEnumerator<T> : IEnumerator<T>
{
    public T Current { get { return List[Index]; } }
    object IEnumerator.Current { get { return List[Index]; } }

    private List<T> List;
    private int Index;

    public SecureCollectionEnumerator(List<T> List)
    {
        this.List = List;

        Reset();
    }

    public void Dispose()
    {
        List.Clear();
        List = null;
    }

    public bool MoveNext()
    {
        Index++;

        return (Index < List.Count);
    }

    public void Reset()
    {
        Index = -1;
    }
}