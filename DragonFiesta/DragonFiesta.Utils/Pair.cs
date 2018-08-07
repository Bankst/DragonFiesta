using System;

public class Pair<T1, T2> : Tuple<T1, T2>
{
    public Pair(T1 item1, T2 item2)
        : base(item1, item2)
    {
    }

    public T1 First { get => Item1; }
    public T2 Second { get => Item2; }
}