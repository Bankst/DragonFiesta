namespace DragonFiesta.Game.Worlds
{
    public enum WorldStatus : byte
    {
        Offline = 0,
        Maintenance = 1,
        EmptyServer = 2,
        Reserved = 3,
        LoginFailed = 4,
        Full = 5,
        OK = 6,
        Low = 7,
        Medium = 9,
        High = 10,
    }
}