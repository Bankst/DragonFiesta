using DragonFiesta.Utils.Config.Section.Game;
using System;
using System.Text;
using System.Xml.Serialization;

namespace DragonFiesta.Utils.Config
{
    [ServerModule(ServerType.World, InitializationStage.Logic)]
    [ServerModule(ServerType.Zone, InitializationStage.PreData)]
    public class GameConfiguration : Configuration<GameConfiguration>
    {
        public static GameConfiguration Instance { get; set; }

        public ushort TutorialMap { get; set; } = 150; //RouN
        public bool UseTutorial { get; set; } = true;

        public byte MaxCharacterPerWorld { get; set; } = 6;
        public bool ShowWeaponsAtCharacterSelect { get; set; } = false;
        public ushort DefaultSpawnMapId { get; set; } = 124;//PortalField

        public ExpSection ExpSetting { get; set; } = new ExpSection();
        public WalkSection WalkSetting { get; set; } = new WalkSection();
        public LimitSection LimitSetting { get; set; } = new LimitSection();

        public FriendSection FriendSettings { get; set; } = new FriendSection();
        [XmlIgnore]
        public Encoding GameCharsSet = Encoding.UTF8;

        [InitializerMethod]
        public static bool Initialize()
        {
            try
            {
                Instance = ReadXml();

                if (Instance != null)
                {
                    EngineLog.Write(EngineLogLevel.Startup, "Successfully read GameConfiguration config.");
                    return true;
                }
                else
                {
                    if (Write(out GameConfiguration pConfig))
                    {
                        pConfig.WriteXml();
                        EngineLog.Write(EngineLogLevel.Startup, "Successfully created  GameConfiguration.");
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

        public static bool Write(out GameConfiguration pConfig)
        {
            pConfig = null;
            try
            {
                pConfig = new GameConfiguration();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}