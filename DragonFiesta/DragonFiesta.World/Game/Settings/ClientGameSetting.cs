namespace DragonFiesta.World.Game.Settings
{
    public class ClientGameSetting : IGameSetting
    {
        public GameSettingType SettingType { get; private set; }

        public bool Enable { get; set; }

        public ClientGameSetting(GameSettingType Type, bool Enable)
        {
            SettingType = Type;
            this.Enable = Enable;
        }
    }
}