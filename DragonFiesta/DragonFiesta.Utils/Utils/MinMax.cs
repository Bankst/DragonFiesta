public class MinMax<T>
{
    public T Min { get; set; }
    public T Max { get; set; }

    public MinMax()
    {
    }

    public MinMax(T Min, T Max)
    {
        this.Min = Min;
        this.Max = Max;
    }
}