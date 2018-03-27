using System;

// Requester -> Server ->Response -> Requester
public interface IExpectAnAnswer : IMessage, IDisposable
{
    Action<IMessage> Callback { get; }
    Action<IMessage> TimeOutCallBack { get; }

    DateTime ExpireTime { get; }
    bool IsDisposed { get; }
}