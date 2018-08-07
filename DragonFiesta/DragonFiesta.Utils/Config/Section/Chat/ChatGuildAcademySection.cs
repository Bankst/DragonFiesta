namespace DragonFiesta.Utils.Config.Section.Chat
{
    public class ChatGuildAcademySection : ChatSection
    {
        public override byte MinLevel { get; set; } = 1;
        public override byte Delay { get; set; } = 2;
    }
}