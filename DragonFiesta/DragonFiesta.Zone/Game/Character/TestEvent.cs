using DragonFiesta.Zone.Game.Maps.Types;
using DragonFiesta.Game.Characters.Event;
using System;

namespace DragonFiesta.Zone.Game.Character
{

    [GameServerModule(ServerType.Zone, GameInitalStage.Network)]
    public class TestEvent
    {
        [InitializerMethod]
        public static bool Test()
        {
            ZoneCharacterManager.Instance.OnCharacterMapChanged += Instance_OnCharacterMapChanged;
            ZoneCharacterManager.Instance.OnCharacterLogin += Instance_OnCharacterLogin;
            ZoneCharacterManager.Instance.OnCharacterLogout += Instance_OnCharacterLogout;
            ZoneCharacterManager.Instance.OnCharacterLevelChanged += Instance_OnCharacterLevelChanged;
            return true;
        }

        private static void Instance_OnCharacterLevelChanged(object sender, CharacterLevelChangedEventArgs<ZoneCharacter> e)
        {
            Console.WriteLine("Level Changed");
        }

        private static void Instance_OnCharacterLogout(object sender, CharacterEventArgs<ZoneCharacter> e)
        {
            Console.WriteLine("Charavzer Logout");
        }

        private static void Instance_OnCharacterLogin(object sender, CharacterEventArgs<ZoneCharacter> e)
        {
            Console.WriteLine("Character Login");
        }

        private static void Instance_OnCharacterMapChanged(object sender, CharacterMapEventArgs<ZoneCharacter, IMap> e)
        {
            Console.WriteLine($"Character Changed map to {e.Map.MapId } from {e.Character.Map.MapInfo.ID} ");
        }

        private static void Instance_OnCharacterMapLogout(object sender, CharacterMapEventArgs<ZoneCharacter, ZoneServerMap> e)
        {
            Console.WriteLine("CharacterMapLogout");
        }

        private static void Instance_OnCharacterMapLogin(object sender, CharacterMapEventArgs<ZoneCharacter, ZoneServerMap> e)
        {
            Console.WriteLine("Character MapLogin");
        }
    }
}
