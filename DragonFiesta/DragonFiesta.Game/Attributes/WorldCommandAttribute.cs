#region

using System;
using DragonFiesta.Game.Attributes;

#endregion

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class WorldCommandAttribute : GameCommandAttribute
{
    public WorldCommandAttribute(string pCommand) :
        base(pCommand, GameCommandType.World)
    {
    }
}