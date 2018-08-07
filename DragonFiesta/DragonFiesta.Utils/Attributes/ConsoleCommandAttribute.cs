using System;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class ConsoleCommandAttribute : Attribute
{
    public string Command { get; private set; }

    public ConsoleCommandAttribute(string pCommand)
    {
        Command = pCommand;
    }
}

[AttributeUsage(AttributeTargets.Class)]
public sealed class ConsoleCommandCategory : Attribute
{
    public string Category { get; private set; }

    public ConsoleCommandCategory(string pCategory)
    {
        Category = pCategory;
    }
}

[AttributeUsage(AttributeTargets.Method)]
public sealed class InitialConsoleCommandCategory : Attribute
{
    public InitialConsoleCommandCategory()
    {
    }
}