using System;
using System.Threading;

[ServerModule(ServerType.Login, InitializationStage.PreData)]
[ServerModule(ServerType.World, InitializationStage.PreData)]
[ServerModule(ServerType.Zone, InitializationStage.PreData)]
public sealed class ServerTaskManager : IUpdateAbleServer
{
    public static ServerTaskManager Instance { get; private set; }

    private SecureCollection<IServerTask> Objects;

    private object ThreadLocker;

    public TimeSpan UpdateInterval => TimeSpan.FromMilliseconds(100);

    public GameTime LastUpdate { get; private set; }

    private int IsDisposedInt;

    public ServerTaskManager()
    {
        LastUpdate = GameTime.Now();
        Objects = new SecureCollection<IServerTask>();
        ThreadLocker = new object();

    }
    ~ServerTaskManager()
    {
        Dispose();
    }
    [InitializerMethod]
    public static bool InitialTaskManager()
    {
        if (Instance == null)
            return false;

        foreach (var Type in Reflector.GiveServerTasks())
        {
            if (!AddObject((IServerTask)Activator.CreateInstance(Type)))
                return false;
        }

        return true;
    }
    public static ServerTaskManager InitialInstance() => Instance = new ServerTaskManager();

    public static bool AddObject(IServerTask Object)
    {
        if (Instance.IsDisposedInt == 1) return false; //Adding no more ...

        lock (Instance.ThreadLocker)
        {
            Object.LastUpdate = GameTime.Now();

            return Instance.Objects.Add(Object);
        }
    }

    public static bool RemoveObject(IServerTask Object)
    {
        if (Instance.IsDisposedInt == 1) return false; //Adding no more ...

        lock (Instance.ThreadLocker)
        {
            return Instance.Objects.Remove(Object);
        }
    }
    public void Dispose()
    {
        if (Interlocked.CompareExchange(ref IsDisposedInt, 1, 0) == 0)
        {
            Objects.Dispose();
            Objects = null;
            ThreadLocker = null;
        }
    }

    public bool Update(GameTime Now)
    {
        if (IsDisposedInt == 1)
            return false;

        foreach (var TaskObject in Objects)
        {
            if (Now.Subtract(TaskObject.LastUpdate).TotalMilliseconds >= (int)TaskObject.Interval)
            {
                if (!TaskObject.Update(Now))
                {
                    Objects.Remove(TaskObject);
                    TaskObject.Dispose();
                }
                TaskObject.LastUpdate = GameTime.Now();
            }
        }

        LastUpdate = GameTime.Now();


        return true;
    }
}
