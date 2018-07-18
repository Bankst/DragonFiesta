using DragonFiesta.Game.Characters;
using DragonFiesta.Game.Chat;
using DragonFiesta.Game.CommandAccess;
using DragonFiesta.Messages.Command;
using DragonFiesta.Utils.Config.Section.Chat;
using DragonFiesta.Zone.Game.Character;
using DragonFiesta.Zone.Network;
using DragonFiesta.Zone.Network.FiestaHandler.Server;
using System;
using System.Linq;
using DragonFiesta.Networking.Network.Session;

namespace DragonFiesta.Zone.Game.Chat
{
    public abstract class ChatBase : ChatManager
    {
        public ChatBase(ChatSection config) : base(config)
        {
        }

        public sealed override void Chat(CharacterBase Base, string message)
        {
            ThreadPool.AddCall(() =>
            {
                var pChar = (Base as ZoneCharacter);

                switch (PerfomChat(pChar, message))
                {
                    case ChatResult.LevelTooLow:
                    case ChatResult.Spamming:
                        SH02Handler.SendChatBlock(pChar?.Session, Config.Delay);
                        break;

                    case ChatResult.Success:
                        BroadcastMessage(pChar?.Session, message);
                        break;
                }
            });
        }

        protected override void DisposeInternal()
        {
        }

        public sealed override bool ExecuteCommand(CharacterBase character, string message)
        {
            var args = message.Split(' ');

            if (!((ZoneCharacter) character).Session.Ingame)
                return false;

            if (args.Length < 2)
                return false;

            if (!character.IsGM)
                return false;

            var category = args[0].Replace("&", "").ToUpper();

            var command = args[1].ToUpper();

            var commandArgs = args.Skip(2).ToArray();

            if (!GameCommandManager.GetGameCommand(category, command, out GameCommand cmd))
            {
                ZoneChat.CharacterNote(((ZoneCharacter) character), $"Command { message} not found!");
                return false;
            }

            if (!cmd.RoleInfo.ContainsKey(((ZoneCharacter) character).LoginInfo.RoleId))
            {
                ZoneChat.CharacterNote(((ZoneCharacter) character), "Access Denied!!");
                return false;
            }

            if (cmd.Method != null)
            {
	            if (!(bool) cmd.Method.Invoke(this, new object[] {character, commandArgs})) return false;
	            CommandLog.Write(CommandLogLevel.Execute, "Character {0} Execute Command {1}", character.Info.Name, message);
	            return true;
            }
            else
            {
                InternConnector.Instance.SendMessage(new GameCommandToServer
                {
                    Id = Guid.NewGuid(),
                    Args = commandArgs,
                    CharacterId = character.Info.CharacterID,
                    Category = category,
                    Command = command,
                });

                return true;
            }
        }

        public abstract void BroadcastMessage(ZoneSession session, string message);
    }
}