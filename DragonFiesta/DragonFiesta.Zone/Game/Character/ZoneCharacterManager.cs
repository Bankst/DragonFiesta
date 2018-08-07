using DragonFiesta.Game.Characters;
using DragonFiesta.Zone.Core;
using DragonFiesta.Zone.Game.Maps.Types;
using DragonFiesta.Zone.InternNetwork.InternHandler.Server.Character;
using DragonFiesta.Zone.Network;

namespace DragonFiesta.Zone.Game.Character
{
    [GameServerModule(ServerType.Zone, GameInitalStage.CharacterData)]
    public class ZoneCharacterManager : CharacterManagerBase<ZoneCharacter>
    {
        public static ZoneCharacterManager Instance { get; set; }

        [InitializerMethod]
        public static bool Initial()
        {
            Instance = new ZoneCharacterManager();

            return true;
        }



        public ZoneCharacterManager() : base()
        {

            //Testing Event
        }


        protected override void FinalizeCharacterLevelChanged(ZoneCharacter Character, byte NewLevel, ushort MobId = ushort.MaxValue)
        {



            base.FinalizeCharacterLevelChanged(Character, NewLevel);
        }
        public override void CharacterMapRefreshed(ZoneCharacter Character)
        {

            if (!(Character.Map as LocalMap).AddObject(Character))
            {
                Character.Session.Dispose();
                return;
            }


            base.CharacterMapRefreshed(Character);
        }
        protected override void FinalizeLogCharacterIn(ZoneCharacter Character)
        {
            if (Character.IsOnThisZone)
            {
                ZonePingManager.Instance.RegisterClient(Character.Session);
            }

            //no login when zone reconnect
            if (ServerMain.InternalInstance.ServerIsReady)
            {
                base.FinalizeLogCharacterIn(Character);
            }
        }

      
        protected override void FinalizeCharacterLogOut(ZoneCharacter Character)
        {
            if (Character.IsConnected)
            {
                if (Character.Map is LocalMap Map)
                {
                    Map.RemoveObject(Character);

                    ZonePingManager.Instance.RevokeClient(Character.Session);

                }
                CharacterMethods.SendCharacterLoggedOut(Character);
            }
            base.FinalizeCharacterLogOut(Character);
        }
        public sealed override void CharacterChangeMap(ZoneCharacter Character, IMap NewMap)
        {
            if (Character.IsOnThisZone)
            {
                //Remove Character from Map..
                if (!(Character.Map as LocalMap).RemoveObject(Character))
                {
                    Character.Session.Dispose();
                    return;
                }
            }

            base.CharacterChangeMap(Character, NewMap);
        }
    }
}