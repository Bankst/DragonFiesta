using System;


public interface IUpdateAbleServer
{
    TimeSpan UpdateInterval { get; }
    GameTime LastUpdate { get; }
    bool Update(GameTime Now);

    void Dispose();
}
