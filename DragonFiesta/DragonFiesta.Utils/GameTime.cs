using DragonFiesta.Utils.Core;
using System;

public class GameTime
{
    public static readonly GameTime Zero = new GameTime(DateTime.MinValue, TimeSpan.Zero, TimeSpan.Zero);

    public DateTime Time { get; private set; }

    public TimeSpan Elapsed { get; private set; }
    public TimeSpan TotalElapsed { get; private set; }

    public GameTime()
        : this(DateTime.Now, TimeSpan.Zero, TimeSpan.Zero)
    {
    }

    public GameTime(DateTime Time)
        : this(Time, TimeSpan.Zero, TimeSpan.Zero)
    {
    }

    public GameTime(DateTime Time, TimeSpan Elapsed)
        : this(Time, Elapsed, TimeSpan.Zero)
    {
    }

    public GameTime(DateTime Time, TimeSpan Elapsed, TimeSpan TotalElapsed)
    {
        this.Time = Time;
        this.Elapsed = Elapsed;
        this.TotalElapsed = TotalElapsed;
    }

    public TimeSpan Subtract(GameTime Time)
    {
        return Subtract(Time.Time);
    }

    public TimeSpan Subtract(DateTime Time)
    {
        return this.Time.Subtract(Time);
    }

    public static GameTime Now()
    {
        return ServerMainBase.InternalInstance.CurrentTime;
    }

    public GameTime AddMilliseconds(double Amount)
    {
        return new GameTime(Time.AddMilliseconds(Amount), Elapsed, TotalElapsed);
    }

    public override bool Equals(object obj)
    {
        return (this == (obj as GameTime));
    }

    public override int GetHashCode()
    {
        return (Time.GetHashCode() + Elapsed.GetHashCode() + TotalElapsed.GetHashCode());
    }

    public override string ToString()
    {
        return Time.ToString();
    }

    public static explicit operator GameTime(DateTime Value)
    {
        return new GameTime(Value, TimeSpan.Zero, TimeSpan.Zero);
    }

    public static bool operator ==(GameTime v1, GameTime v2)
    {
        if ((object)v1 == null)
            return ((object)v2 == null);

        return (v1.Time == v2.Time
             && v1.Elapsed == v2.Elapsed
             && v1.TotalElapsed == v2.TotalElapsed);
    }

    public static bool operator !=(GameTime v1, GameTime v2)
    {
        return !(v1 == v2);
    }

    public static bool operator >=(GameTime v1, GameTime v2)
    {
        if ((object)v1 == null)
            return ((object)v2 == null);

        return (v1.Time >= v2.Time);
    }

    public static bool operator <=(GameTime v1, GameTime v2)
    {
        if ((object)v1 == null)
            return ((object)v2 == null);

        return (v1.Time <= v2.Time);
    }

    public static bool operator >(GameTime v1, GameTime v2)
    {
        if ((object)v1 == null)
            return ((object)v2 == null);

        return (v1.Time > v2.Time);
    }

    public static bool operator <(GameTime v1, GameTime v2)
    {
        if ((object)v1 == null)
            return ((object)v2 == null);

        return (v1.Time < v2.Time);
    }

    public static GameTime operator +(GameTime v1, GameTime v2)
    {
        return new GameTime(v1.Time, (v1.Elapsed + v2.Elapsed), (v1.TotalElapsed + v2.TotalElapsed));
    }

    public static GameTime operator -(GameTime v1, GameTime v2)
    {
        TimeSpan elapsed = (v2.Elapsed > v1.Elapsed ? TimeSpan.Zero : (v1.Elapsed - v2.Elapsed)),
                 totalElapsed = (v2.TotalElapsed > v1.TotalElapsed ? TimeSpan.Zero : (v1.TotalElapsed - v2.TotalElapsed));

        return new GameTime(v1.Time, elapsed, totalElapsed);
    }

    public static bool operator >=(GameTime v1, DateTime v2)
    {
        if ((object)v1 == null)
            return ((object)v2 == null);
        return (v1.Time >= v2);
    }

    public static bool operator <=(GameTime v1, DateTime v2)
    {
        if ((object)v1 == null)
            return ((object)v2 == null);
        return (v1.Time <= v2);
    }

    public static bool operator >(GameTime v1, DateTime v2)
    {
        if ((object)v1 == null)
            return ((object)v2 == null);
        return (v1.Time > v2);
    }

    public static bool operator <(GameTime v1, DateTime v2)
    {
        if ((object)v1 == null)
            return ((object)v2 == null);
        return (v1.Time < v2);
    }

    public static bool operator >=(DateTime v1, GameTime v2)
    {
        if ((object)v1 == null)
            return ((object)v2 == null);
        return (v1 >= v2.Time);
    }

    public static bool operator <=(DateTime v1, GameTime v2)
    {
        if ((object)v1 == null)
            return ((object)v2 == null);
        return (v1 <= v2.Time);
    }

    public static bool operator >(DateTime v1, GameTime v2)
    {
        if ((object)v1 == null)
            return ((object)v2 == null);
        return (v1 > v2.Time);
    }

    public static bool operator <(DateTime v1, GameTime v2)
    {
        if ((object)v1 == null)
            return ((object)v2 == null);
        return (v1 < v2.Time);
    }
}