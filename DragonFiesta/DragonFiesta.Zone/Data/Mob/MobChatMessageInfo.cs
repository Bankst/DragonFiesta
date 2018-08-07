using DragonFiesta.Database.SQL;

namespace DragonFiesta.Zone.Data.Mob
{
    public class MobChatMessageInfo
    {
        public string ChatText { get; set; }

        public int Rate { get; set; }

        public MobChatMessageInfo(SQLResult Result, int i)
        {
            ChatText = Result.Read<string>(i, "ChatText");
            Rate = Result.Read<int>(i, "Rate2");
        }
    }
}