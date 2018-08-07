using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class ServerModuleAttribute : Attribute
{
    private readonly InitializationStage stageInternal;
    private readonly ServerType InitTypeInternal;
    public ServerType InitialType { get { return InitTypeInternal; } }
    public InitializationStage InitializationStage { get { return stageInternal; } }

    public ServerModuleAttribute(ServerType InitialType, InitializationStage initializationStage)
    {
        stageInternal = initializationStage;
        InitTypeInternal = InitialType;
    }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class InitializerMethodAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class ShutdownAttribute : Attribute
{
    private readonly ShutdownType _Type;

    public ShutdownType Type { get => _Type; }

    public ShutdownAttribute(ShutdownType Type)
    {
        _Type = Type;
    }
}