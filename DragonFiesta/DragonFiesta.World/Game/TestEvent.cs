using DragonFiesta.Game.Characters.Event;
using DragonFiesta.World.Game.Character;
using DragonFiesta.World.Game.Maps;
using System;

namespace DragonFiesta.Zone.Game.Character
{

    [GameServerModule(ServerType.World, GameInitalStage.NPC)]
    public class TestEvent
    {
        [InitializerMethod]
        public static bool Initial()
        {
            WorldCharacterManager.Instance.OnCharacterDelete += Instance_OnCharacterDelete;
            WorldCharacterManager.Instance.OnCharacterMapChanged += Instance_OnCharacterMapChanged;
            WorldCharacterManager.Instance.OnCharacterLogin += Instance_OnCharacterLogin;
            WorldCharacterManager.Instance.OnCharacterLogout += Instance_OnCharacterLogout;
            WorldCharacterManager.Instance.OnCharacterLevelChanged += Instance_OnCharacterLevelChanged;
            return true;
        }

        private static void Instance_OnCharacterDelete(object sender, CharacterDeleteEventArgs<WorldCharacter> e)
        {
            Console.WriteLine("Delete"+e.Character.Info.Name);
        }

        private static void Instance_OnCharacterLevelChanged(object sender, CharacterLevelChangedEventArgs<WorldCharacter> e)
        {
            Console.WriteLine("Level chaged");
        }

        private static void Instance_OnCharacterLogout(object sender, CharacterEventArgs<WorldCharacter> e)
        {
            Console.WriteLine("Charavzer Logout");
        }

        private static void Instance_OnCharacterLogin(object sender, CharacterEventArgs<WorldCharacter> e)
        {
            Console.WriteLine("Character Login");
        }

        private static void Instance_OnCharacterMapChanged(object sender, CharacterMapEventArgs<WorldCharacter, IMap> e)
        {
            Console.WriteLine("Character MapChanged");
        }

        private static void Instance_OnCharacterMapLogout(object sender, CharacterMapEventArgs<WorldCharacter, WorldServerMap> e)
        {
            Console.WriteLine("CharacterMapLogout");
        }

        private static void Instance_OnCharacterMapLogin(object sender, CharacterMapEventArgs<WorldCharacter, WorldServerMap> e)
        {
            Console.WriteLine("Character MapLogin");
        }
    }
}
