using DragonFiesta.Game.Characters.Data;
using DragonFiesta.Game.CommandAccess;
using System;
using DragonFiesta.Database.SQL;

namespace DragonFiesta.World.Game.Character
{
    public class WorldClientLoginInfo : ClientLoginInfo,IDisposable
    {
        public GameRole GameRole { get; set; }

        private WorldCharacter Character { get; set; }

        public WorldClientLoginInfo(WorldCharacter Character)
        {
            this.Character = Character;
        }



        //Here ClientSettteings
        //ShortsCutts
        //Quickbars etc

        public override bool RefreshFromSQL(SQLResult pRes, int i)
        {
            if (!base.RefreshFromSQL(pRes, i))
                return false;

            return true;
        }

        public void Dispose()
        {
            GameRole = null;
            Character = null;
        }

        ~WorldClientLoginInfo()
        {
            Dispose();
        }
    }
}