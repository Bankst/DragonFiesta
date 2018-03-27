using DragonFiesta.Utils.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

public sealed class ThreadPool : IDisposable
{
    private static ThreadPool Instance;

    private readonly ConcurrentQueue<IUpdateAbleServer> _Servertasks = new ConcurrentQueue<IUpdateAbleServer>();
    private readonly ConcurrentQueue<Action> _Calls = new ConcurrentQueue<Action>();

    private List<Thread> _workers;

    private bool _disposed;

    private object SyncObject = new object();

    private int TaskPerSecond = 0;

    private DateTime LastUpdateTick = DateTime.Now;

    public static int PerfomanceCount { get => Instance.TaskPerSecond; }

    public static void Start(int ThreadCount)
    {
        if (ThreadCount < 1)
            throw new InvalidOperationException("Invalid WorkCount for ThreadPool");

        Instance = new ThreadPool();

        Instance.StartThread(ThreadCount);
    }

    public static void Stop()
    {
        Instance.Dispose();
    }

    private void StartThread(int Count)
    {
        _workers = new List<Thread>();

        for (var i = 0; i < Count; ++i)
        {
            var worker = new Thread(Worker) { Name = string.Concat("Worker ", i) };
            worker.Start();
            _workers.Add(worker);
        }
    }

    public void Dispose()
    {

        if (!_disposed)
        {

            _disposed = true;



            while (_Servertasks.TryDequeue(out IUpdateAbleServer Task))
                Task.Dispose();

            Monitor.PulseAll(SyncObject);

            //Wait of all wotker endet
          
        }
    }

    private bool CallAbleServer(GameTime TimeNow)
    {
       
        if (!_Servertasks.TryDequeue(out IUpdateAbleServer Task))
        {
            return false;
        }

        try

        {

            if (TimeNow.Subtract(Task.LastUpdate).TotalMilliseconds >= Task.UpdateInterval.TotalMilliseconds)
            {
                if (!Task.Update(TimeNow))
                {
                    Task.Dispose();
                    return false;
                }
            }
        }
        catch (Exception ex)
        {
            EngineLog.Write(ex, "Unhandelt AbleServer Exception");
        }
        finally
        {
            if (!_disposed)
                _Servertasks.Enqueue(Task);

        }
        return true;
    }

    private void Worker()
    {
        var ServerStartUpTime = DateTime.Now;

        for (;;)
        {
            if (_disposed)
                break;

            lock (SyncObject)
            {

                var NowTime = DateTime.Now;
                var elapsed = (ServerStartUpTime - NowTime);
                GameTime NowGameTime = new GameTime(NowTime, elapsed, ServerMainBase.InternalInstance.TotalUpTime);

                ServerMainBase.InternalInstance.TotalUpTime += elapsed;
                ServerMainBase.InternalInstance.CurrentTime = NowGameTime;

                if (_Calls.TryDequeue(out Action Call)) //Calls
                {
                    Call();
                    TaskPerSecond++;
                }

                //Perfomanc Testing...^^
                if (NowTime.Subtract(LastUpdateTick).TotalSeconds >= 1)
                {
                    LastUpdateTick = DateTime.Now;
                    TaskPerSecond = 0;
                }


                if (CallAbleServer(NowGameTime) && _Calls.IsEmpty)
                {
                    Monitor.Wait(SyncObject, 1000);
                    continue;
                }
            }
        }

        _workers.Remove(Thread.CurrentThread);
        GameLog.Write(GameLogLevel.Internal, $"Thread {Thread.CurrentThread.Name } endet !");
    }

    public static void AddCall(Action Call)
    {
        lock (Instance.SyncObject)
        {
            if (!Instance._disposed)
            {
                Instance._Calls.Enqueue(Call);
                Monitor.Pulse(Instance.SyncObject);
            }
        }
    }

    public static void AddUpdateAbleServer(IUpdateAbleServer mTask)
    {
        lock (Instance.SyncObject)
        {
            if (Instance._disposed)
            {
                throw new ObjectDisposedException("This Pool instance has already been disposed");
            }

            Instance._Servertasks.Enqueue(mTask);
            Monitor.Pulse(Instance.SyncObject);
        }
    }
}