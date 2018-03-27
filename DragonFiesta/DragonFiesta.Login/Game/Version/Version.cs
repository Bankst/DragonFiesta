using System;

public class Version
{
    public string mHash { get; set; }

    public DateTime Date { get; set; }

    public Version(SQLResult pRes, int i)
    {
        mHash = pRes.Read<string>(i, "ClientHash");
        Date = pRes.Read<DateTime>(i, "ClientDate");
    }

    public Version()
    { }
}