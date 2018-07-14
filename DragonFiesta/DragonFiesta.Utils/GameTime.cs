using DragonFiesta.Utils.Core;
using System;
using System.Globalization;

public class GameTime
{
    public static readonly GameTime Zero = new GameTime(DateTime.MinValue, TimeSpan.Zero, TimeSpan.Zero);

    public DateTime Time { get; }

    public TimeSpan Elapsed { get; }
    public TimeSpan TotalElapsed { get; }

    public GameTime()
        : this(DateTime.Now, TimeSpan.Zero, TimeSpan.Zero)
    {
    }

    public GameTime(DateTime time)
        : this(time, TimeSpan.Zero, TimeSpan.Zero)
    {
    }

    public GameTime(DateTime time, TimeSpan elapsed)
        : this(time, elapsed, TimeSpan.Zero)
    {
    }

    public GameTime(DateTime time, TimeSpan elapsed, TimeSpan totalElapsed)
    {
        this.Time = time;
        this.Elapsed = elapsed;
        this.TotalElapsed = totalElapsed;
    }

    public TimeSpan Subtract(GameTime time)
    {
        return Subtract(time.Time);
    }

    public TimeSpan Subtract(DateTime time)
    {
        return this.Time.Subtract(time);
    }

    public static GameTime Now()
    {
        return ServerMainBase.InternalInstance.CurrentTime;
    }

    public GameTime AddMilliseconds(double amount)
    {
        return new GameTime(Time.AddMilliseconds(amount), Elapsed, TotalElapsed);
    }

    public override bool Equals(object obj)
    {
        return this == obj as GameTime;
    }

	public override int GetHashCode() => Time.GetHashCode() + Elapsed.GetHashCode() + TotalElapsed.GetHashCode();

	public override string ToString() => Time.ToString(CultureInfo.CurrentCulture);

	public static explicit operator GameTime(DateTime value)
    {
        return new GameTime(value, TimeSpan.Zero, TimeSpan.Zero);
    }

    public static bool operator ==(GameTime v1, GameTime v2)
    {
        if ((object)v1 == null)
            return (object)v2 == null;

        return v2 != null && v1.Time == v2.Time && v1.Elapsed == v2.Elapsed && v1.TotalElapsed == v2.TotalElapsed;
    }

    public static bool operator !=(GameTime v1, GameTime v2)
    {
        return !(v1 == v2);
    }

    public static bool operator >=(GameTime v1, GameTime v2)
    {
        if ((object)v1 == null)
            return (object)v2 == null;

        return v1.Time >= v2.Time;
    }

    public static bool operator <=(GameTime v1, GameTime v2)
    {
        if ((object)v1 == null)
            return (object)v2 == null;

        return v1.Time <= v2.Time;
    }

    public static bool operator >(GameTime v1, GameTime v2)
    {
        if ((object)v1 == null)
            return (object)v2 == null;

        return v1.Time > v2.Time;
    }

    public static bool operator <(GameTime v1, GameTime v2)
    {
        if ((object)v1 == null)
            return (object)v2 == null;

        return v1.Time < v2.Time;
    }

    public static GameTime operator +(GameTime v1, GameTime v2)
    {
        return new GameTime(v1.Time, v1.Elapsed + v2.Elapsed, v1.TotalElapsed + v2.TotalElapsed);
    }

    public static GameTime operator -(GameTime v1, GameTime v2)
    {
        TimeSpan elapsed = v2.Elapsed > v1.Elapsed ? TimeSpan.Zero : v1.Elapsed - v2.Elapsed,
                 totalElapsed = v2.TotalElapsed > v1.TotalElapsed ? TimeSpan.Zero : v1.TotalElapsed - v2.TotalElapsed;

        return new GameTime(v1.Time, elapsed, totalElapsed);
    }

    public static bool operator >=(GameTime v1, DateTime v2)
    {
        if ((object)v1 == null)
            return false;
        return v1.Time >= v2;
    }

    public static bool operator <=(GameTime v1, DateTime v2)
    {
        if ((object)v1 == null)
            return false;
        return v1.Time <= v2;
    }

    public static bool operator >(GameTime v1, DateTime v2)
    {
        if ((object)v1 == null)
            return false;
        return v1.Time > v2;
    }

    public static bool operator <(GameTime v1, DateTime v2)
    {
        if ((object)v1 == null)
            return false;
        return v1.Time < v2;
    }

    public static bool operator >=(DateTime v1, GameTime v2)
    {
	    return v1 >= v2.Time;
    }

    public static bool operator <=(DateTime v1, GameTime v2)
    {
	    return v1 <= v2.Time;
    }

    public static bool operator >(DateTime v1, GameTime v2)
    {
	    return v1 > v2.Time;
    }

    public static bool operator <(DateTime v1, GameTime v2)
    {
        return v1 < v2.Time;
    }
}