using DragonFiesta.Utils.Core;
using System;
using System.Threading;

[Serializable]
public class ExpectAnswer : IExpectAnAnswer
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [NonSerialized]
    private Action<IMessage> _Callback;

    public Action<IMessage> Callback
    {
        get => _Callback;
        set => _Callback = value;
    }

    [NonSerialized]
    private Action<IMessage> _TimeOutCallBack;

    public Action<IMessage> TimeOutCallBack
    {
        get => _TimeOutCallBack;
        set => _TimeOutCallBack = value;
    }

    [NonSerialized]
    private int IsDisposedInt;

    public bool IsDisposed => (IsDisposedInt > 0);

    [NonSerialized]
    private DateTime _ExpireTime;

    public DateTime ExpireTime
    {
        get => _ExpireTime;
    }

    public ExpectAnswer(int TimeToAnswerExpire)
    {
        _ExpireTime = ServerMainBase.InternalInstance.CurrentTime.Time.Add(TimeSpan.FromMilliseconds(TimeToAnswerExpire));
    }

    ~ExpectAnswer()
    {
        Dispose();
    }

    public virtual void DisposeInternal()
    {
        TimeOutCallBack = null;
        //  Callback = null;
    }

    public void Dispose()
    {
        if (Interlocked.CompareExchange(ref IsDisposedInt, 1, 0) == 0)
        {
            DisposeInternal();
        }
    }
}