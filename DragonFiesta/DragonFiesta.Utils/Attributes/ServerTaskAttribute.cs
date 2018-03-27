using System;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class ServerTaskClass : Attribute
{
    public ServerTaskClass()
    {
    }
}