using DragonFiesta.Utils.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using DragonFiesta.Utils.Logging;

public sealed class ThreadPool : IDisposable
{
    private static ThreadPool _instance;

    private readonly ConcurrentQueue<IUpdateAbleServer> _servertasks = new ConcurrentQueue<IUpdateAbleServer>();
    private readonly ConcurrentQueue<Action> _calls = new ConcurrentQueue<Action>();

    private List<Thread> _workers;

    private bool _disposed;

    private readonly object _syncObject = new object();

    private int _taskPerSecond = 0;

    private DateTime _lastUpdateTick = DateTime.Now;

    public static int PerfomanceCount { get => _instance._taskPerSecond; }

    public static void Start(int threadCount)
    {
        if (threadCount < 1)
            throw new InvalidOperationException("Invalid WorkCount for ThreadPool");

        _instance = new ThreadPool();

        _instance.StartThread(threadCount);
    }

    public static void Stop()
    {
        _instance.Dispose();
    }

    private void StartThread(int count)
    {
        _workers = new List<Thread>();

        for (var i = 0; i < count; ++i)
        {
            var worker = new Thread(Worker) { Name = string.Concat("Worker ", i) };
            worker.Start();
            _workers.Add(worker);
        }
    }

    public void Dispose()
    {
	    if (_disposed) return;
	    _disposed = true;
		
	    while (_servertasks.TryDequeue(out var task))
		    task.Dispose();

	    Monitor.PulseAll(_syncObject);

	    //Wait of all wotker endet
    }

    private bool CallAbleServer(GameTime timeNow)
    {
       
        if (!_servertasks.TryDequeue(out var task))
        {
            return false;
        }

        try
        {
            if (timeNow.Subtract(task.LastUpdate).TotalMilliseconds >= task.UpdateInterval.TotalMilliseconds)
            {
                if (!task.Update(timeNow))
                {
                    task.Dispose();
                    return false;
                }
            }
        }
        catch (Exception ex)
        {
            EngineLog.Write(ex, "Unhandled AbleServer Exception");
        }
        finally
        {
            if (!_disposed)
                _servertasks.Enqueue(task);

        }
        return true;
    }

    private void Worker()
    {
        var serverStartUpTime = DateTime.Now;

        for (;;)
        {
            if (_disposed)
                break;

            lock (_syncObject)
            {

                var nowTime = DateTime.Now;
                var elapsed = (serverStartUpTime - nowTime);
                var nowGameTime = new GameTime(nowTime, elapsed, ServerMainBase.InternalInstance.TotalUpTime);

                ServerMainBase.InternalInstance.TotalUpTime += elapsed;
                ServerMainBase.InternalInstance.CurrentTime = nowGameTime;

                if (_calls.TryDequeue(out var call)) //Calls
                {
                    call();
                    _taskPerSecond++;
                }

                //Perfomance Testing...^^
                if (nowTime.Subtract(_lastUpdateTick).TotalSeconds >= 1)
                {
                    _lastUpdateTick = DateTime.Now;
                    _taskPerSecond = 0;
                }


	            if (!CallAbleServer(nowGameTime) || !_calls.IsEmpty) continue;
	            Monitor.Wait(_syncObject, 1000);
            }
        }

        _workers.Remove(Thread.CurrentThread);
        GameLog.Write(GameLogLevel.Internal, $"Thread {Thread.CurrentThread.Name } endet !");
    }

    public static void AddCall(Action call)
    {
        lock (_instance._syncObject)
        {
	        if (_instance._disposed) return;
	        _instance._calls.Enqueue(call);
	        Monitor.Pulse(_instance._syncObject);
        }
    }

    public static void AddUpdateAbleServer(IUpdateAbleServer mTask)
    {
        lock (_instance._syncObject)
        {
            if (_instance._disposed)
            {
                throw new ObjectDisposedException("This Pool instance has already been disposed");
            }

            _instance._servertasks.Enqueue(mTask);
            Monitor.Pulse(_instance._syncObject);
        }
    }
}