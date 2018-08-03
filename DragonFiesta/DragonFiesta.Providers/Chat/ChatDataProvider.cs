using DragonFiesta.Utils.IO.SHN;

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
			var watch = System.Diagnostics.Stopwatch.StartNew();
			BadNames = new SecureCollection<string>();

            SHNResult pResult = SHNManager.Load(SHNType.BadNameFilter);
            DataLog.WriteProgressBar(">> Load BadNameFilter");

            using (var mBar = new ProgressBar(pResult.Count))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    var badName = pResult.Read<string>(i, "BadName");
                    if (!BadNames.Add(badName))
                    {
                        DataLog.Write(DataLogLevel.Warning, $"Duplicate BadName found {badName}");
                    }
                    mBar.Step();
                }
	            watch.Stop();
                DataLog.WriteProgressBar($">> Loaded {BadNames.Count} BadNames from SHN in {(double)watch.ElapsedMilliseconds/1000}s");
            }
        }

        public static bool GetBadName(string name)
        {
            return BadNames.Contains(name.ToUpper());
        }
    }
}