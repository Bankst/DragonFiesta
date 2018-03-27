using DragonFiesta.Game.Attributes;
using System;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class WorldCommandAttribute : GameCommandAttribute
{
    public WorldCommandAttribute(string pCommand) :
        base(pCommand, GameCommandType.World)
    {
    }
}