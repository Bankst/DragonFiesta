namespace DragonFiesta.Providers.Chat
{
    [GameServerModule(ServerType.Zone, GameInitalStage.Text)]
    [GameServerModule(ServerType.World, GameInitalStage.Text)]
    public class ChatDataProvider
    {
        private static SecureCollection<string> BadNames { get; set; }

        [InitializerMethod]
        public static bool OnStart()
        {
            try
            {
                LoadBadNames();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void LoadBadNames()
        {
            BadNames = new SecureCollection<string>();

            SQLResult pResult = DB.Select(DatabaseType.Data, "SELECT * FROM BadNameFilter");

            DatabaseLog.WriteProgressBar(">> Load BadNameFilter");

            using (ProgressBar mBar = new ProgressBar(pResult.Count))
            {
                for (int i = 0; i < pResult.Count; i++)
                {
                    string BadName = pResult.Read<string>(i, "BadName");
                    if (!BadNames.Add(BadName))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate BadName found {0}", BadName);
                    }
                    mBar.Step();
                }
                DatabaseLog.WriteProgressBar(">> Loaded {0} BadNames", BadNames.Count);
            }
        }

        public static bool GetBadName(string name)
        {
            return BadNames.Contains(name.ToUpper());
        }
    }
}