using System;
using System.Collections.Concurrent;
using System.Data.SqlClient;
using DragonFiesta.Database.SQL;

namespace DragonFiesta.Login.Game.Authentication
{
    [GameServerModule(ServerType.Login, GameInitalStage.Logic)]
    public class VersionsManager
    {
        private static ConcurrentDictionary<string, Version> VersionsByHash { get; set; }
        private static ConcurrentDictionary<DateTime, Version> VersionsByDate { get; set; }

        [InitializerMethod]
        public static bool InitalStart()
        {
            LoadVersions();
            return true;
        }

        public static void LoadVersions()
        {
            VersionsByHash = new ConcurrentDictionary<string, Version>();
            VersionsByDate = new ConcurrentDictionary<DateTime, Version>();

            var pResult = DB.Select(DatabaseType.Login, "SELECT * FROM Versions");

            DatabaseLog.WriteProgressBar(">> Load Versions");

            using (var mBar = new ProgressBar((pResult.Count)))
            {
                for (var i = 0; i < pResult.Count; i++)
                {
                    var mVersion = new Version(pResult, i);
                    if (!VersionsByHash.TryAdd(mVersion.mHash.ToUpper(), mVersion)
                        || !VersionsByDate.TryAdd(mVersion.Date, mVersion))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate Versions Date {0} Hash {1} found!", mVersion.Date, mVersion.mHash);
                    }
                    mBar.Step();
                }
                DatabaseLog.WriteProgressBar(">> Loaded {0} Versions", VersionsByHash.Count);
            }
        }

        public static bool AddVersion(string hash, DateTime date)
        {
            var newVersion = new Version()
            {
                mHash = hash,
                Date = date,
            };

            if (!VersionsByHash.TryAdd(newVersion.mHash.ToUpper(), newVersion) || !VersionsByDate.TryAdd(newVersion.Date, newVersion))
                return false;

            const string cmd = "INSERT INTO Versions (ClientDate, ClientHash) VALUES (@Date,@Hash)";

            DB.RunSQL(DatabaseType.Login, cmd,
                new SqlParameter("@Date", newVersion.Date),
                new SqlParameter("@Hash", hash.ToUpper()));

            return true;
        }

        public static bool GetVersionByHash(string hash, out Version version)
        {
            return VersionsByHash.TryGetValue(hash.ToUpper(), out version);
        }

        public static bool GetVersionByDate(DateTime dt, out Version version)
        {
            return VersionsByDate.TryGetValue(dt, out version);
        }
    }
}