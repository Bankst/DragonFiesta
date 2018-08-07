using DragonFiesta.Utils.Config;
using DragonFiesta.Utils.Config.Section;
using DragonFiesta.Utils.Config.Section.Network;
using System;
using System.Xml.Serialization;

namespace DragonFiesta.World.Config
{
    public class WorldConfiguration : Configuration<WorldConfiguration>
    {
        public ServerInfo InternalServerInfo { get; set; } = new ServerInfo();
        public WorldDatabaseSection WorldDatabaseSettings { get; set; } = new WorldDatabaseSection();
        public DataDatabaseSection DataDatabaseSettings { get; set; } = new DataDatabaseSection();
        public ConnectInfo ConnectToInfo { get; set; } = new ConnectInfo();
        public ExternServerInfo ServerInfo { get; set; } = new ExternServerInfo();

        public byte WorldID { get; set; } = 0;

        public static WorldConfiguration Instance { get; set; }

        public int WorkTaskThreadCount { get; set; } = 2;

        [XmlIgnore]
        public ClientRegion ServerRegion { get; set; } = ClientRegion.None;

        public static bool Initialize()
        {
            try
            {
                Instance = ReadXml();

                if (Instance != null)
                {
                    EngineLog.Write(EngineLogLevel.Startup, "Successfully read World config.");
					EngineLog.Write(EngineLogLevel.Info, $"World External IP: {Instance.ServerInfo.ExternalIP}");
                    return true;
                }
                else
                {
	                if (!Write(out var pConfig)) return false;
	                pConfig.WriteXml();
	                EngineLog.Write(EngineLogLevel.Startup, "Successfully created World config.");
	                return false;
                }
            }
            catch (Exception ex)
            {
                EngineLog.Write(EngineLogLevel.Exception, "Failed to Load config {0}", ex);
                return false;
            }
        }

        public static bool Write(out WorldConfiguration pConfig)
        {
            pConfig = null;
            try
            {
                pConfig = new WorldConfiguration();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}