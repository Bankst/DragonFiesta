using System;

[Serializable]
public class UpdateWorldServer : IMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int NowPlayerCount { get; set; }
    public bool WorldReady { get; set; }
}