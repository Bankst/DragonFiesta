public static class MathExtensions
{
    public static bool IsBetween(this short Value, short Lower, short Upper)
    {
        return (Value >= Lower
             && Value <= Upper);
    }

    public static bool IsBetween(this int Value, int Lower, int Upper)
    {
        return (Value >= Lower
             && Value <= Upper);
    }

    public static bool IsBetween(this long Value, long Lower, long Upper)
    {
        return (Value >= Lower
             && Value <= Upper);
    }

    public static bool IsBetween(this ushort Value, ushort Lower, ushort Upper)
    {
        return (Value >= Lower
             && Value <= Upper);
    }

    public static bool IsBetween(this uint Value, uint Lower, uint Upper)
    {
        return (Value >= Lower
             && Value <= Upper);
    }

    public static bool IsBetween(this ulong Value, ulong Lower, ulong Upper)
    {
        return (Value >= Lower
             && Value <= Upper);
    }
}