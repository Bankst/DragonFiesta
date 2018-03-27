using DragonFiesta.Game.Characters.Data;

namespace DragonFiesta.Zone.Game.Character
{
    public class ZoneClientLoginInfo : ClientLoginInfo
    {
        private ZoneCharacter Character { get; set; }

        public byte RoleId { get; set; }

        public ZoneClientLoginInfo(ZoneCharacter Character) : base()
        {
            this.Character = Character;
        }

        public override bool RefreshFromSQL(SQLResult pRes, int i)
        {
            return base.RefreshFromSQL(pRes, i);
        }

        ~ZoneClientLoginInfo()
        {
            Character = null;
        }
    }
}