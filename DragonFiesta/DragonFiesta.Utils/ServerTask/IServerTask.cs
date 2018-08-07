using System;


public interface IServerTask : IDisposable
{
    ServerTaskTimes Interval { get;  }

    GameTime LastUpdate { get; set; }

    bool Update(GameTime gameTime);
}


