using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class GameServerModuleAttribute : Attribute
{
    private readonly GameInitalStage stageInternal;
    private readonly ServerType InitTypeInternal;
    public ServerType InitialType { get => InitTypeInternal; }
    public GameInitalStage InitializationStage { get => stageInternal; }

    public GameServerModuleAttribute(ServerType InitialType, GameInitalStage initializationStage)
    {
        stageInternal = initializationStage;
        InitTypeInternal = InitialType;
    }
}