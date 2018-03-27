using System;
using System.Runtime.Serialization;

public class CrashException : Exception
{
    public CrashException()
    {
    }

    public CrashException(string message) : base(message)
    {
    }

    public CrashException(string message, Exception inner) : base(message, inner)
    {
    }

    protected CrashException(
      SerializationInfo info,
      StreamingContext context) : base(info, context)
    { }
}