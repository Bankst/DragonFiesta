using System;
using System.Runtime.Serialization;

[Serializable]
public class ConnectExecption : Exception
{
    public ConnectExecption(string message)
        : base(message)
    { }

    public ConnectExecption(string format, params object[] args)
        : base(string.Format(format, args))
    { }

    protected ConnectExecption(SerializationInfo info, StreamingContext context)
        : base(info, context)
    { }
}