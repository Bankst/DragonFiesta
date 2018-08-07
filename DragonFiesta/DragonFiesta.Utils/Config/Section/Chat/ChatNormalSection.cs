namespace DragonFiesta.Utils.Config.Section.Chat
{
    public class ChatNormalSection : ChatSection
    {
        public override byte MinLevel { get; set; } = 1;
        public override byte Delay { get; set; } = 2;
    }
}