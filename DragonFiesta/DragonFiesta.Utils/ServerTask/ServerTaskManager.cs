using System;
using System.Linq;
using System.Threading;

[ServerModule(ServerType.Login, InitializationStage.PreData)]
[ServerModule(ServerType.World, InitializationStage.PreData)]
[ServerModule(ServerType.Zone, InitializationStage.PreData)]
public sealed class ServerTaskManager : IUpdateAbleServer
{
    public static ServerTaskManager Instance { get; private set; }

    private SecureCollection<IServerTask> _objects;

    private object _threadLocker;

    public TimeSpan UpdateInterval => TimeSpan.FromMilliseconds(100);

    public GameTime LastUpdate { get; private set; }

    private int _isDisposedInt;

    public ServerTaskManager()
    {
        LastUpdate = GameTime.Now();
        _objects = new SecureCollection<IServerTask>();
        _threadLocker = new object();

    }
    ~ServerTaskManager()
    {
        Dispose();
    }
    [InitializerMethod]
    public static bool InitialTaskManager()
    {
	    return Instance != null && Reflector.GiveServerTasks().All(type => AddObject((IServerTask) Activator.CreateInstance(type)));
    }
    public static ServerTaskManager InitialInstance() => Instance = new ServerTaskManager();

    public static bool AddObject(IServerTask Object)
    {
        if (Instance._isDisposedInt == 1) return false; //Adding no more ...

        lock (Instance._threadLocker)
        {
            Object.LastUpdate = GameTime.Now();

            return Instance._objects.Add(Object);
        }
    }

    public static bool RemoveObject(IServerTask Object)
    {
        if (Instance._isDisposedInt == 1) return false; //Adding no more ...

        lock (Instance._threadLocker)
        {
            return Instance._objects.Remove(Object);
        }
    }
    public void Dispose()
    {
	    if (Interlocked.CompareExchange(ref _isDisposedInt, 1, 0) != 0) return;
	    _objects.Dispose();
	    _objects = null;
	    _threadLocker = null;
    }

    public bool Update(GameTime gameTime)
    {
        if (_isDisposedInt == 1)
            return false;

        foreach (var taskObject in _objects)
        {
	        if (!(gameTime.Subtract(taskObject.LastUpdate).TotalMilliseconds >= (int) taskObject.Interval)) continue;
	        if (!taskObject.Update(gameTime))
	        {
		        _objects.Remove(taskObject);
		        taskObject.Dispose();
	        }
	        taskObject.LastUpdate = GameTime.Now();
        }

        LastUpdate = GameTime.Now();


        return true;
    }
}
