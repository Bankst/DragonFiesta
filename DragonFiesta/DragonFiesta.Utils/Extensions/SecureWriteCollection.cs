using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class SecureWriteCollection<T> : IEnumerable<T>
{
    public int Count
    {
        get { return List.Count; }
    }

    public bool IsDisposed { get { return (IsDisposedInt > 0); } }
    private int IsDisposedInt;

    private SecureCollection<T> List;

    public SecureWriteCollection(out Func<T, bool> AddFunction, out Func<T, bool> RemoveFunction, out Action ClearAction)
    {
        Load();

        AddFunction = Add;
        RemoveFunction = Remove;
        ClearAction = List.Clear;
    }

    public void Dispose()
    {
        if (Interlocked.CompareExchange(ref IsDisposedInt, 1, 0) == 0)
        {
            List.Clear();
            List = null;
        }
    }

    ~SecureWriteCollection()
    {
        Dispose();
    }

    private void Load()
    {
        List = new SecureCollection<T>();
    }

    // IEnumerable
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return List.GetEnumerator();
    }

    public T this[int Index]
    {
        get { return List[Index]; }
        set { List[Index] = value; }
    }

    public T Find(Predicate<T> Match)
    {
        return List.Find(Match);
    }

    public void ForEach(Action<T> Action)
    {
        List.ForEach(Action);
    }

    public bool Contains(T Object)
    {
        return List.Contains(Object);
    }

    protected virtual bool Add(T Object)
    {
        return List.Add(Object);
    }

    private bool Remove(T Object)
    {
        return List.Remove(Object);
    }
}