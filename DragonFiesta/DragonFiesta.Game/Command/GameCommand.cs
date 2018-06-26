using System.Collections.Concurrent;
using System.Reflection;

namespace DragonFiesta.Game.CommandAccess
{
    public class GameCommand
    {
        public int Id { get; private set; }
        public string Command { get; private set; }

        public string CommandCategory { get; set; }

        public MethodInfo Method { get; set; }

        public GameCommandType Type { get; set; }

        public ConcurrentDictionary<byte, GameRole> RoleInfo { get; private set; }

        public GameCommand(SQLResult Res, int i)
        {
            RoleInfo = new ConcurrentDictionary<byte, GameRole>();
            Id = Res.Read<int>(i, "ID");
            Command = Res.Read<string>(i, "Command").ToUpper();
            CommandCategory = Res.Read<string>(i, "CommandCategory").ToUpper();
        }
    }
}