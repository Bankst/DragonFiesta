namespace DragonFiesta.Utils.Config.Section.Chat
{
    public class ChatPartySection : ChatSection
    {
        public override byte Delay { get; set; } = 2;
        public override byte MinLevel { get; set; } = 1;
    }
}