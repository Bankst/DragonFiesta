using DragonFiesta.World.Game.Character;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace DragonFiesta.World.Game.Settings
{
    public class GameSettingManager : IDisposable
    {
        protected ConcurrentDictionary<GameSettingType, IGameSetting> SettingsList { get; private set; }

        public bool IsDisposed { get { return (IsDisposedInt > 0); } }
        private int IsDisposedInt;

        private WorldCharacter Owner;

        public GameSettingManager(WorldCharacter Owner)
        {
            this.Owner = Owner;
            SettingsList = new ConcurrentDictionary<GameSettingType, IGameSetting>();
        }
        ~GameSettingManager()
        {
            Dispose();
        }

        public void LoadDefaultGameSettings()
        {
            for (ushort i = 0; i < (ushort)GameSettingType.SHINE_GAME_OPT_MAX; i++)
            {
                if (!Enum.IsDefined(typeof(GameSettingType), i))
                    continue;

                UpdateSettings((GameSettingType)i, true);
            }
        }

        public bool UpdateSettings(GameSettingType Type, bool Enable)
        {
            if (Contains(Type))
            {
                SettingsList[Type].Enable = Enable;
                return true;
            }
            //TODO Create CustomSettings..
            switch (Type)
            {
                case GameSettingType.AUTOHIDE_CHATWIN:

                case GameSettingType.AUTOHIDE_SYSTEMWIN:
                case GameSettingType.ENABLE_SKILLLOCK:
                case GameSettingType.INIT_INTERFACEPOS:
                case GameSettingType.NOTICE_LOGIN_GUILDMEMBER:
                case GameSettingType.REFUSE_CONFIRM_MSG:
                case GameSettingType.REFUSE_GUILD_INVITE:
                case GameSettingType.REFUSE_PARTY_INVITE:
                case GameSettingType.REFUSE_SYSTEM_MSG:
                case GameSettingType.REFUSE_TRADE:
                case GameSettingType.REFUSE_WHISPER:
                case GameSettingType.SHINE_GAME_OPT_MAX:
                case GameSettingType.SHOW_AUTOSTACK:
                case GameSettingType.SHOW_BASICINFO_TIP:
                case GameSettingType.SHOW_BILLBOARD_NAMEPANEL:
                case GameSettingType.SHOW_HP_BAR:
                case GameSettingType.SHOW_INTERFACE:
                case GameSettingType.SHOW_ITEM_NAME:
                case GameSettingType.SHOW_MONSTER_NAME:
                case GameSettingType.SHOW_MY_CHAR_NAME:
                case GameSettingType.SHOW_NPC_NAME:
                case GameSettingType.SHOW_OTHERS_CHAR_NAME:
                case GameSettingType.SHOW_PLAYGUIDE:
                case GameSettingType.SHOW_SPEECH_BUBBLE:
                case GameSettingType.SHOW_SP_BAR:
                case GameSettingType.START_CHATTING_ENTER:
                    SettingsList.TryAdd(Type, new ClientGameSetting(Type, Enable));
                    return true;
                default:
                    GameLog.Write(GameLogLevel.Warning, "Unkown GameSettings...");
                    return false;
            }
        }
        public IGameSetting this[GameSettingType Index]
        {
            get { return SettingsList[Index]; }
        }
        public bool Contains(GameSettingType Key)
        {
            return SettingsList.Values.FirstOrDefault(m => m.SettingType == Key) != null;
        }

        public bool IsEnable(GameSettingType Type)
        {
            if (!Contains(Type))
                return false;

            return SettingsList[Type].Enable;
        }

        public byte[] GetGameSettingData()
        {
            using (MemoryStream Stream = new MemoryStream())
            {
                using (var Writer = new BinaryWriter(Stream))
                {
                    Writer.Write((ushort)(SettingsList.Count));

                    foreach (var SettingOption in SettingsList)
                    {

                        Writer.Write((ushort)SettingOption.Value.SettingType);
                        Writer.Write(SettingOption.Value.Enable);

                    }

                    return Stream.ToArray();
                }
            }

        }

        public bool LoadGameSettingsFromData(byte[] Data)
        {
            try
            {
                using (var Reader = new BinaryReader(new MemoryStream(Data), Encoding.ASCII))
                {
                    ushort UpdateCount = Reader.ReadUInt16();

                    for (byte i = 0; i < UpdateCount; i++)
                    {
                        if (!Enum.IsDefined(typeof(GameSettingType), i))
                            continue;

                        GameSettingType Type = (GameSettingType)Reader.ReadUInt16();
                        bool Enable = Reader.ReadBoolean();

                        if (!UpdateSettings(Type, Enable))
                        {
                            GameLog.Write(GameLogLevel.Warning, "Failed to Update GameSettings  for Character {0}", Owner.Info.Name);
                            return false;
                        }
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                GameLog.Write(ex, "Failed Load KeyMap For Character {0}", Owner.Info.Name);
                return false;
            }
        }

        public bool RefreshFromSQL(SQLResult Result, int i)
        {
            try
            {

                byte[] GameSettingData = Result.GetBytes(i, "Game");


                if (GameSettingData.Length == 1 && GameSettingData[0] == 0x00)
                {
                    LoadDefaultGameSettings();
                    return true;
                }
                else
                {
                    return LoadGameSettingsFromData(GameSettingData);
                }
            }
            catch (Exception ex)
            {
                GameLog.Write(ex, "Failed Load Character GameSettings from Character {0}", Owner.Info.CharacterID);
                return false;
            }
        }

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref IsDisposedInt, 1, 0) == 0)
            {
                SettingsList.Clear();
                SettingsList = null;
                Owner = null;
            }
        }
    }
}
