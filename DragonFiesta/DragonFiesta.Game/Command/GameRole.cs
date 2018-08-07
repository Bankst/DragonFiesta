#region

using DragonFiesta.Database.SQL;

#endregion

namespace DragonFiesta.Game.CommandAccess
{
    public class GameRole
    {
        public byte Id { get; private set; }

        public string Name { get; private set; }

        public GameRole(SQLResult Res, int i)
        {
            Id = Res.Read<byte>(i, "Id");
            Name = Res.Read<string>(i, "RoleName");
        }
    }
}