public sealed class ThreeRecord<T1, T2, T3>
{
    public T1 First { get; private set; }
    public T2 Second { get; private set; }
    public T3 Three { get; private set; }

    public ThreeRecord(T1 pFirst, T2 pSecond, T3 pThree)
    {
        First = pFirst;
        Second = pSecond;
        Three = pThree;
    }
}