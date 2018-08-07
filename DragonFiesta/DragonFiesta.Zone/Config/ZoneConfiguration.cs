using DragonFiesta.Utils.Config;
using DragonFiesta.Utils.Config.Section;
using System;

namespace DragonFiesta.Zone.Config
{
    public class ZoneConfiguration : Configuration<ZoneConfiguration>
    {
        public WorldDatabaseSection WorldDatabaseSettings { get; set; } = new WorldDatabaseSection();

        public DataDatabaseSection DataDatabaseSettings { get; set; } = new DataDatabaseSection();

        public WorldConnectInfo WorldConnectInfo { get; set; } = new WorldConnectInfo();
        public ZoneServerInfo ZoneServerInfo { get; set; } = new ZoneServerInfo();

        public static ZoneConfiguration Instance { get; set; }

        public ushort WorkThreadCount { get; set; }

        public byte ZoneID { get; set; }

        public static bool Initialize(byte ZoneId)
        {
            try
            {
                Instance = ReadXml($"Zone{ZoneId}Configuration");

                if (Instance != null)
                {
                    EngineLog.Write(EngineLogLevel.Startup, "Successfully read Zone config.");
					EngineLog.Write(EngineLogLevel.Info, $"Zone NAT IP: {Instance.ZoneServerInfo.ExternalIP}");

					return true;
                }
                else
                {
                    Instance = new ZoneConfiguration();
                    Instance.WriteXml($"Zone{ZoneId}Configuration");
                    EngineLog.Write(EngineLogLevel.Startup, "Successfully created Zone config.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                EngineLog.Write(EngineLogLevel.Exception, "Failed to Load config {0}", ex);
                return false;
            }
        }

        public static bool Write(out ZoneConfiguration pConfig)
        {
            pConfig = null;
            try
            {
                pConfig = new ZoneConfiguration();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}