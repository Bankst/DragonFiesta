public enum InitializationStage : uint
{
    PreData = uint.MinValue,
    Data = 0x01,
    Logic = 0x02,
    Sync = 0x03,
    InternNetwork = 0x04,
    CharacterData = 0x05,
    Networking = uint.MaxValue,
}