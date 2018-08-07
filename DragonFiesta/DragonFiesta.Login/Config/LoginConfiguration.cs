using DragonFiesta.Utils.Config;
using DragonFiesta.Utils.Config.Section.Network;
using System;

namespace DragonFiesta.Login.Config
{
    public class LoginConfiguration : Configuration<LoginConfiguration>
    {
        public ServerInfo InternServerInfo { get; set; } = new ServerInfo();
        public ExternServerInfo GameServerInfo { get; set; } = new ExternServerInfo();
        public LoginDatabaseSection LoginDatabaseSettings { get; set; } = new LoginDatabaseSection();

        public int ThreadTaskPool { get; set; } = 4;

        public static LoginConfiguration Instance { get; set; }

        public static bool Initialize()
        {
            try
            {
                Instance = ReadXml();

                if (Instance != null)
                {
                    EngineLog.Write(EngineLogLevel.Startup, "Successfully read Login config.");
					EngineLog.Write(EngineLogLevel.Info, $"Login External IP: {Instance.GameServerInfo.ExternalIP}");
                    return true;
                }
                else
                {
                    if (Write(out LoginConfiguration pConfig))
                    {
                        pConfig.WriteXml();
                        EngineLog.Write(EngineLogLevel.Startup, "Successfully created Login config.");
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

        public static bool Write(out LoginConfiguration pConfig)
        {
            pConfig = null;
            try
            {
                pConfig = new LoginConfiguration();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}