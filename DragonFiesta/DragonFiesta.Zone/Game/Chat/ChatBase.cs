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

namespace DragonFiesta.Zone.Game.Chat
{
    public abstract class ChatBase : ChatManager
    {
        public ChatBase(ChatSection Config) : base(Config)
        {
        }

        public sealed override void Chat(CharacterBase Base, string Message)
        {
            ThreadPool.AddCall(() =>
            {
                var pChar = (Base as ZoneCharacter);

                switch (PerfomChat(pChar, Message))
                {
                    case ChatResult.LevelTooLow:
                    case ChatResult.Spamming:
                        SH02Handler.SendChatBlock(pChar.Session, Config.Delay);
                        break;

                    case ChatResult.Success:
                        BroadcastMessage(pChar.Session, Message);
                        break;
                }
            });
        }

        protected override void DisposeInternal()
        {
        }

        public sealed override bool ExecuteCommand(CharacterBase Character, string Message)
        {
            string[] Args = Message.Split(' ');

            if (!(Character as ZoneCharacter).Session.Ingame)
                return false;

            if (Args.Length < 2)
                return false;

            if (!Character.IsGM)
                return false;

            string Category = Args[0].Replace("&", "").ToUpper();

            string Command = Args[1].ToUpper();

            string[] CommandArgs = Args.Skip(2).ToArray();

            if (!GameCommandManager.GetGameCommand(Category, Command, out GameCommand Cmd))
            {
                ZoneChat.CharacterNote((Character as ZoneCharacter), $"Command { Message} not found!");
                return false;
            }

            if (!Cmd.RoleInfo.ContainsKey((Character as ZoneCharacter).LoginInfo.RoleId))
            {
                ZoneChat.CharacterNote((Character as ZoneCharacter), "Access Denied!!");
                return false;
            }

            if (Cmd.Method != null)
            {
                if ((bool)Cmd.Method.Invoke(this, new object[] { Character, CommandArgs }))
                {
                    CommandLog.Write(CommandLogLevel.Execute, "Character {0} Execute Command {1}", Character.Info.Name, Message);
                    return true;
                }
            }
            else
            {
                InternWorldConnector.Instance.SendMessage(new GameCommandToServer
                {
                    Id = Guid.NewGuid(),
                    Args = CommandArgs,
                    CharacterId = Character.Info.CharacterID,
                    Category = Category,
                    Command = Command,
                });

                return true;
            }

            return false;
        }

        public abstract void BroadcastMessage(ZoneSession Session, string Message);
    }
}