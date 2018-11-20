using DragonFiesta.Database.Models;
using DragonFiesta.Game.Characters;
using DragonFiesta.Networking.Helpers;
using DragonFiesta.World.Network;

namespace DragonFiesta.World.Game.Character
{
    public class CharacterCollection : CharacterCollection<WorldCharacter>
    {
        public WorldSession Session { get; private set; }

        public CharacterCollection(WorldSession session)
        {
            Session = session;
        }

        public bool Refresh()
        {
	        using (var we = EDM.GetWorldEntity())
	        {
		        lock (ThreadLocker)
		        {
			        foreach (var character in we.DBCharacters)
			        {
						if (!WorldCharacterManager.Instance.GetCharacterByCharacterID(character.ID, out var mCharacter, out var result))
				        {
							_SH04Helpers.SendCharacterError(Session, result);
				        }

				        Add(mCharacter);
					}
		        }
	        }
            return true;
        }

        protected override void FinalizeCharacterAdd(WorldCharacter character)
        {
            if (CharactersByID.TryAdd(character.Info.CharacterID, character)
         && CharactersByName.TryAdd(character.Info.Name, character)
         && CharactersBySlot.TryAdd(character.Info.Slot, character))
            {
                character.Info.OnNameChanged += On_Character_NameChanged;
            }
        }

        protected override void FinalizeCharacterRemove(WorldCharacter character)
        {
            character.Info.OnNameChanged -= On_Character_NameChanged;
            CharactersByID.TryRemove(character.Info.CharacterID, out character);
            CharactersByName.TryRemove(character.Info.Name, out character);
            CharactersBySlot.TryRemove(character.Info.Slot, out character);
        }

        private void On_Character_NameChanged(object sender, WorldCharacterNameChangedEventArgs args)
        {
            lock (ThreadLocker)
            {
	            if (args.OldName == null || args.OldName == args.Character.Info.Name) return;
	            if (WorldCharacterManager.Instance.ChangeCharacterNameById(args.Character, args.Character.Info.Name))
	            {
		            CharactersByName.TryRemove(args.OldName, out var pChar);
		            CharactersByName.TryAdd(args.Character.Info.Name, args.Character);
	            }
	            else
	            {
		            args.Character.Info.Name = args.OldName;
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