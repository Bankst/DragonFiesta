using DragonFiesta.Providers.Characters;

namespace DragonFiesta.World.Data.Characters
{
    [GameServerModule(ServerType.World, GameInitalStage.CharacterData)]
    public class CharacterDataProvider : CharacterDataProviderBase
    {
        [InitializerMethod]
        public static bool InitialData()
        {
            LoadExpTable();

            return true;
        }
    }
}
