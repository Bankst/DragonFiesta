using System.Collections.Generic;
using DragonFiesta.Database.SQL;

namespace DragonFiesta.Zone.Data.Mob
{
    public class MobChatInfo
    {
        public List<MobChatMessageInfo> MobChatList { get; set; }
        public ushort MobId { get; private set; }
        public int Rate { get; set; }
        public MobChatType Type { get; private set; }

        public MobChatInfo(SQLResult Result, int i)
        {
            MobId = Result.Read<ushort>(i, "MobId");
            Rate = Result.Read<int>(i, "Rate");
            Type = (MobChatType)Result.Read<byte>(i, "ChatType");
        }
    }
}