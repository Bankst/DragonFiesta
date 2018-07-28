using DragonFiesta.Database.Models;
using DragonFiesta.Database.SQL;
using DragonFiesta.Game.Characters;
using DragonFiesta.Networking.Helpers;
using DragonFiesta.World.Network;

namespace DragonFiesta.World.Game.Character
{
    public class CharacterCollection : CharacterCollection<WorldCharacter>
    {
        public WorldSession Session { get; private set; }

        public CharacterCollection(WorldSession Session) : base()
        {
            this.Session = Session;
        }

        public bool Refresh()
        {
	        using (var we = EDM.GetWorldEntity())
	        {
		        lock (ThreadLocker)
		        {
			        foreach (var character in we.DBCharacters)
			        {
						if (!WorldCharacterManager.Instance.GetCharacterFromEntity(character.ID, out WorldCharacter mCharacter, out CharacterErrors result))
				        {
							_SH04Helpers.SendCharacterError(Session, result);
				        }
			        }
		        }
	        }
				SQLResult Result = DB.Select(DatabaseType.World, $"SELECT * FROM Characters WHERE AccountID = { Session.UserAccount.ID } ");
            lock (ThreadLocker)
            {
                for (int i = 0; i < Result.Count; i++)
                {
                    if (!WorldCharacterManager.Instance.GetCharacterFromSQL(Result, i, out WorldCharacter mCharacter, out CharacterErrors LoadResult))
                    {
                        _SH04Helpers.SendCharacterError(Session, LoadResult);
                        return false;
                    }

                    Add(mCharacter);
                }
            }
            return true;
        }

        protected override void FinalizeCharacterAdd(WorldCharacter Character)
        {
            if (CharactersByID.TryAdd(Character.Info.CharacterID, Character)
         && CharactersByName.TryAdd(Character.Info.Name, Character)
         && CharactersBySlot.TryAdd(Character.Info.Slot, Character))
            {
                Character.Info.OnNameChanged += On_Character_NameChanged;
            }
        }

        protected override void FinalizeCharacterRemove(WorldCharacter Character)
        {
            Character.Info.OnNameChanged -= On_Character_NameChanged;
            CharactersByID.TryRemove(Character.Info.CharacterID, out Character);
            CharactersByName.TryRemove(Character.Info.Name, out Character);
            CharactersBySlot.TryRemove(Character.Info.Slot, out Character);
        }

        private void On_Character_NameChanged(object sender, WorldCharacterNameChangedEventArgs args)
        {
            lock (ThreadLocker)
            {
                if (args.OldName != null
                    && args.OldName != args.Character.Info.Name)
                {
                    if (WorldCharacterManager.Instance.ChangeCharacterNameById(args.Character, args.Character.Info.Name))
                    {
                        CharactersByName.TryRemove(args.OldName, out WorldCharacter pChar);
                        CharactersByName.TryAdd(args.Character.Info.Name, args.Character);
                    }
                    else
                    {
                        args.Character.Info.Name = args.OldName;
                    }
                }
            }
        }


        protected override void DisposeInternal()
        {
            base.DisposeInternal();

            Session = null;
        }


    }
}