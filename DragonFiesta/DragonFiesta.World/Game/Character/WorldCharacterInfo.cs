using DragonFiesta.Game.Characters.Data;
using System;
using DragonFiesta.Database.SQL;

namespace DragonFiesta.World.Game.Character
{
    public class WorldCharacterInfo : CharacterInfo
    {
        private WorldCharacter Character { get; set; }

        public override ulong Money { get; set; }

        public override string Name
        {
            get => _Name;
	        set
            {
                var oldName = _Name;
                _Name = value;
                OnNameChanged?.Invoke(this, new WorldCharacterNameChangedEventArgs(Character, oldName));
            }
        }


        public new ushort FriendPoints
        {
            get => base.FriendPoints;
            set => base.FriendPoints = Math.Min(value, ushort.MaxValue);
        }


        protected string _Name;

        public event EventHandler<WorldCharacterNameChangedEventArgs> OnNameChanged;

        public WorldCharacterInfo()
        {
        }


        public WorldCharacterInfo(WorldCharacter Character) : base()
        {
            this.Character = Character;
        }

        public override bool RefreshFromSQL(SQLResult pRes, int i)
        {

            FriendPoints = pRes.Read<ushort>(i, "FriendPoints");

            return base.RefreshFromSQL(pRes, i);
        }
        public override void Dispose()
        {
            base.Dispose();

            Character = null;
            OnNameChanged = null;
        }
    }
}