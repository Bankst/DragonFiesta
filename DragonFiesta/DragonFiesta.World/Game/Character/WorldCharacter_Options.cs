using DragonFiesta.World.Game.Settings;
using System.Data.SqlClient;
using DragonFiesta.Database.SQL;

namespace DragonFiesta.World.Game.Character
{
    public class WorldCharacter_Options
    {

        public CharacterKeyMap KeyMap { get; private set; }

        public GameSettingManager GameSettings { get; private set; }
        private WorldCharacter Owner { get; set; }

        public WorldCharacter_Options(WorldCharacter Owner)
        {
            this.Owner = Owner;
            KeyMap = new CharacterKeyMap(Owner);
            GameSettings = new GameSettingManager(Owner);
        }

        ~WorldCharacter_Options()
        {
            Dispose();
        }
        public bool Save()
        {
            return true;
        }

        public bool Refresh()
        {

            SQLResult Result = DB.Select(DatabaseType.World,
                "SELECT TOP 1 * FROM Character_Options WHERE ID=@pID",
                new SqlParameter("@pID", Owner.Info.CharacterID));


            if (!Result.HasValues)
                return false;

            if (!KeyMap.RefreshFromSQL(Result, 0))
                return false;

            if (!GameSettings.RefreshFromSQL(Result, 0))
                return false;


            return true;
        }

        public void Dispose()
        {
            KeyMap.Dispose();
            GameSettings.Dispose();

            Owner = null;
        }

    }
}
