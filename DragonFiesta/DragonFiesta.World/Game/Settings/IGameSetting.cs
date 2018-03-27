namespace DragonFiesta.World.Game.Settings
{
    public interface IGameSetting
    {
        GameSettingType SettingType { get; }

        bool Enable { get; set; }
    }
}
