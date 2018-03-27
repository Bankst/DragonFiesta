namespace DragonFiesta.Utils.Config.Section.Chat
{
    public class ChatShoutSection : ChatSection
    {
        public override byte Delay { get; set; } = 3;
        public override byte MinLevel { get; set; } = 5;
    }
}