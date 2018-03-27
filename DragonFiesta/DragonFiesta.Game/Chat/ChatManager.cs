using DragonFiesta.Game.Characters;
using DragonFiesta.Utils.Config.Section.Chat;
using DragonFiesta.Utils.Core;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace DragonFiesta.Game.Chat
{
    public abstract class ChatManager : IDisposable
    {
        private int IsDisposedInt;

        protected ChatSection Config { get; private set; }

        private ConcurrentDictionary<int, ChatInfo> ChatInfosByCharacterId;

        public ChatManager(ChatSection Config)
        {
            this.Config = Config;
            ChatInfosByCharacterId = new ConcurrentDictionary<int, ChatInfo>();
        }

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref IsDisposedInt, 1, 0) == 0)
            {

                DisposeInternal();

                ChatInfosByCharacterId.Clear();
                ChatInfosByCharacterId = null;


                Config = null;


            }
        }

        protected internal virtual ChatResult PerfomChat(CharacterBase Character, string Message)
        {
            var now = ServerMainBase.InternalInstance.CurrentTime.Time;

            ChatInfo info;
            if (!ChatInfosByCharacterId.TryGetValue(Character.Info.CharacterID, out info))
            {
                info = new ChatInfo()
                {
                    CharacterID = Character.Info.CharacterID,
                    LastMessage = DateTime.MinValue,
                };

                ChatInfosByCharacterId.TryAdd(Character.Info.CharacterID, info);
            }

            if (Message.StartsWith("&") && Character.IsGM)
            {
                ExecuteCommand(Character, Message);
                return ChatResult.Success;
            }

            //check level
            if (Config.MinLevel > Character.Info.Level)
            {
                return ChatResult.LevelTooLow;
            }
            //check for spam
            if (now.Subtract(info.LastMessage).TotalSeconds < Config.Delay)
            {
                return ChatResult.Spamming;
            }

            info.LastMessage = now;

            return ChatResult.Success;
        }

        public virtual bool ExecuteCommand(CharacterBase Character, string Message)
        {
            return true;
        }

        protected abstract void DisposeInternal();

        public abstract void Chat(CharacterBase Character, string Message);
    }
}