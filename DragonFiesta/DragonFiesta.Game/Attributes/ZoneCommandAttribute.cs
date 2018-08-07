#region

using DragonFiesta.Game.Attributes;

#endregion

public sealed class ZoneCommandAttribute : GameCommandAttribute
{
    public ZoneCommandAttribute(string pCommand) :
        base(pCommand, GameCommandType.Zone)
    {
    }
}