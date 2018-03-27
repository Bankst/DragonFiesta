using DragonFiesta.Networking.Network;
using System;


public class SessionEventArgs<T> : SessionEventArgs
        where T : SessionBase
{
    public new T Session { get; private set; }

    public SessionEventArgs(T Session) : base(Session)
    {
        this.Session = Session;
    }

    ~SessionEventArgs()
    {
        Session = null;
    }
}


public class SessionEventArgs : EventArgs
{
    public SessionBase Session { get; private set; }






    public SessionEventArgs(SessionBase Session)
    {
        this.Session = Session;
    }
    ~SessionEventArgs()
    {
        Session = null;
    }
}