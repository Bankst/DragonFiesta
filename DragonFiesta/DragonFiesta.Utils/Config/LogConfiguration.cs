using DragonFiesta.Utils.Config.Section.Log;
using System;

namespace DragonFiesta.Utils.Config
{
    [ServerModule(ServerType.Zone, InitializationStage.PreData)]
    [ServerModule(ServerType.World, InitializationStage.PreData)]
    [ServerModule(ServerType.Login, InitializationStage.PreData)]
    public class LogConfiguration : Configuration<LogConfiguration>
    {
        public static LogConfiguration Instance { get; set; }

        public CommandLogSection LogCommand { get; set; } = new CommandLogSection();

        public DatabaseLogSection LogDb { get; set; } = new DatabaseLogSection();

        public EngineLogSection LogEngine { get; set; } = new EngineLogSection();

        public GameLogSection LogGame { get; set; } = new GameLogSection();

        public SocketLogSection LogSocket { get; set; } = new SocketLogSection();

        public void SetupConsole()
        {
            CommandLog.SetupLevels(LogCommand.ConsoleLogLevel, LogCommand.FileLogLevel);
            DatabaseLog.SetupLevels(LogDb.ConsoleLogLevel, LogDb.FileLogLevel);
            EngineLog.SetupLevels(LogEngine.ConsoleLogLevel, LogEngine.FileLogLevel);
            GameLog.SetupLevels(LogGame.ConsoleLogLevel, LogGame.FileLogLevel);
            SocketLog.SetupLevels(LogSocket.ConsoleLogLevel, LogSocket.FileLogLevel);
        }

        public static bool Write(out LogConfiguration pConfig)
        {
            pConfig = null;
            try
            {
                pConfig = new LogConfiguration();
                return true;
            }
            catch
            {
                return false;
            }
        }

        [InitializerMethod]
        public static bool Initialize()
        {
            try
            {
                Instance = ReadXml();

                if (Instance != null)
                {
                    Instance.SetupConsole();
                    EngineLog.Write(EngineLogLevel.Startup, "Successfully read Log config.");
                    return true;
                }

	            if (!Write(out var pConfig)) return false;
	            pConfig.WriteXml();
	            EngineLog.Write(EngineLogLevel.Startup, "Successfully created Log config.");
	            return false;
            }
            catch (Exception ex)
            {
                EngineLog.Write(EngineLogLevel.Exception, "Failed to Load config {0}", ex);
                return false;
            }
        }
    }
}