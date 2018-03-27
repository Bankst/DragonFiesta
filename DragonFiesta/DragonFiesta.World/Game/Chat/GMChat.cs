using DragonFiesta.World.Game.Character;
using DragonFiesta.World.Network;
using DragonFiesta.Utils.Config;

namespace DragonFiesta.World.Game.Chat
{
    [GameServerModule(ServerType.World,GameInitalStage.Command)]
    public class GMChat : ChatBase
    {
        private static GMChat Instance { get; set; }

        public GMChat() : 
            base(ChatConfiguration.Instance.ShoutChatSettings)//tmp
        {

        }
        [InitializerMethod]
        public static bool InitialGMChat()
        {
            Instance = new GMChat();
            WorldCharacterManager.Instance.OnCharacterMapRefresh += Instance_OnCharacterMapRefresh;
            return true;
        }

        private static void Instance_OnCharacterMapRefresh(object sender, DragonFiesta.Game.Characters.Event.CharacterMapRefreshEventArgs<WorldCharacter> args)
        {
            //GM Note
            if (args.Character.IsGM)
            {
                if (args.Character.Map is IInstanceMap e)
                    ZoneChat.CharacterNote(args.Character, $"Instance {e.InstanceId } AccessRole : {args.Character.LoginInfo.GameRole.Name }");
                else
                    ZoneChat.CharacterNote(args.Character, $"AccessRole {args.Character.LoginInfo.GameRole.Name} ");
            }
        }

        public override void BroadcastMessage(WorldSession Session, string Message)
        {
           //TODO :)
        }
    }
}
