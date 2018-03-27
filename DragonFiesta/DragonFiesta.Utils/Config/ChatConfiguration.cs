using DragonFiesta.Utils.Config.Section.Chat;
using System;

namespace DragonFiesta.Utils.Config
{
    [ServerModule(ServerType.World, InitializationStage.PreData)]
    [ServerModule(ServerType.Zone, InitializationStage.PreData)]
    public class ChatConfiguration : Configuration<ChatConfiguration>
    {
        public static ChatConfiguration Instance { get; set; }
        public ChatGuildAcademySection GuildAcademyChatSettings { get; set; }
        public ChatGuildSection GuildChatSettings { get; set; }
        public ChatNormalSection ChatNormalSettings { get; set; }
        public ChatPartySection PartyChatSettings { get; set; }
        public ChatShoutSection ShoutChatSettings { get; set; }
        public WisperChatSection WisperChatSettings { get; set; }

        [InitializerMethod]
        public static bool Initialize()
        {
            try
            {
                Instance = ReadXml();

                if (Instance != null)
                {
                    EngineLog.Write(EngineLogLevel.Startup, "Successfully read Chat config.");
                    return true;
                }
                else
                {
                    ChatConfiguration pConfig;
                    if (Write(out pConfig))
                    {
                        pConfig.WriteXml();
                        EngineLog.Write(EngineLogLevel.Startup, "Successfully created Chat config.");
                        return false;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                EngineLog.Write(EngineLogLevel.Exception, "Failed to Load config {0}", ex);
                return false;
            }
        }

        public static bool Write(out ChatConfiguration pConfig)
        {
            pConfig = null;
            try
            {
                pConfig = new ChatConfiguration();
                pConfig.ChatNormalSettings = new ChatNormalSection();
                pConfig.GuildAcademyChatSettings = new ChatGuildAcademySection();
                pConfig.GuildChatSettings = new ChatGuildSection();
                pConfig.PartyChatSettings = new ChatPartySection();
                pConfig.ShoutChatSettings = new ChatShoutSection();
                pConfig.WisperChatSettings = new WisperChatSection();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}