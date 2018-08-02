using System;
using System.Collections.Concurrent;
using System.Data.SqlClient;
using System.Diagnostics;
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
            var watch = Stopwatch.StartNew();
            VersionsByHash = new ConcurrentDictionary<string, Version>();
            VersionsByDate = new ConcurrentDictionary<DateTime, Version>();

            SQLResult pResult = DB.Select(DatabaseType.Login, "SELECT * FROM Versions");

            DatabaseLog.WriteProgressBar(">> Load Versions");

            using (ProgressBar mBar = new ProgressBar((pResult.Count)))
            {
                for (int i = 0; i < pResult.Count; i++)
                {
                    Version mVersion = new Version(pResult, i);
                    if (!VersionsByHash.TryAdd(mVersion.mHash.ToUpper(), mVersion)
                        || !VersionsByDate.TryAdd(mVersion.Date, mVersion))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate Versions Date {0} Hash {1} found!", mVersion.Date, mVersion.mHash);
                    }
                    mBar.Step();
                }
                watch.Stop();
                DatabaseLog.WriteProgressBar($">> Loaded {VersionsByHash.Count} Versions from Database in {(double)watch.ElapsedMilliseconds / 1000}s");
            }
        }

        public static bool AddVersion(string Hash, DateTime Date)
        {
            Version NewVesion = new Version()
            {
                mHash = Hash,
                Date = Date,
            };

            if (!VersionsByHash.TryAdd(NewVesion.mHash.ToUpper(), NewVesion) || !VersionsByDate.TryAdd(NewVesion.Date, NewVesion))
                return false;

            string SQL = "INSERT INTO Versions (ClientDate, ClientHash) VALUES (@Date,@Hash)";

            DB.RunSQL(DatabaseType.Login, SQL,
                new SqlParameter("@Date", NewVesion.Date),
                new SqlParameter("@Hash", Hash.ToUpper()));

            return true;
        }

        public static bool GetVersionByHash(string hash, out Version Version)
        {
            return VersionsByHash.TryGetValue(hash.ToUpper(), out Version);
        }

        public static bool GetVersionByDate(DateTime Dt, out Version Version)
        {
            return VersionsByDate.TryGetValue(Dt, out Version);
        }
    }
}