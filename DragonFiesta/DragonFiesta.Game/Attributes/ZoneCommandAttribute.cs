using DragonFiesta.Game.Attributes;

public sealed class ZoneCommandAttribute : GameCommandAttribute
{
    public ZoneCommandAttribute(string pCommand) :
        base(pCommand, GameCommandType.Zone)
    {
    }
}