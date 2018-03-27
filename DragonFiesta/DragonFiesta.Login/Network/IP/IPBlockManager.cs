using DragonFiesta.Login.Core;
using System;
using System.Collections.Concurrent;
using System.Data.SqlClient;

namespace DragonFiesta.Login.Network
{
    [ServerModule(ServerType.Login, InitializationStage.Data)]
    public static class IPBlockManager
    {
        private static ConcurrentDictionary<string, IPBlockEntry> BlocksByIP;

        private static object ThreadLocker;

        [InitializerMethod]
        public static bool OnGameStart()
        {
            BlocksByIP = new ConcurrentDictionary<string, IPBlockEntry>();

            ThreadLocker = new object();
            return true;
        }

        public static bool RemoveIPBlock(string IP)
        {
            lock (ThreadLocker)
            {
                using (var cmd = DB.GetDatabaseClient(DatabaseType.Login))
                {
                    cmd.CreateStoreProzedure("dbo.IPBlock_Remove");

                    cmd.SetParameter("@pBlockedIP", IP);
                    switch ((int)cmd.ExecuteScalar())
                    {
                        case 0:
                            return BlocksByIP.TryRemove(IP, out IPBlockEntry e);

                        case -1://IP Not Exis
                        case -2: //Transact fail
                        default:
                            return false;
                    }
                }
            }
        }

        public static bool GetIPBlockByIP(string IP, out IPBlockEntry Block)
        {
            Block = null;

            try
            {
                lock (ThreadLocker)
                {
                    if (BlocksByIP.TryGetValue(IP, out Block))
                        return true;

                    using (var cmd = DB.Select(DatabaseType.Login, QueryDefines.SELECT_IP_BLOCK, new SqlParameter("@pIP", IP)))
                    {
                        if (cmd.Count <= 0)
                            return false;

                        Block = new IPBlockEntry(cmd.Read<string>(0, "BlockedIP"),
                            cmd.Read<DateTime>(0, "BlockDate"),
                            cmd.Read<string>(0, "BlockReason"));

                        BlocksByIP.TryAdd(IP, Block);

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                GameLog.Write(ex, "Error checking if IP '{0}' is blocked:", IP);
                return false;
            }
        }

        public static bool BlockIP(string IP, DateTime? BlockDate = null, string BlockReason = "")
        {
            try
            {
                BlockDate = (BlockDate ?? ServerMain.InternalInstance.CurrentTime.Time);
                BlockReason = (BlockReason ?? "");

                lock (ThreadLocker)
                {
                    using (var cmd = DB.GetDatabaseClient(DatabaseType.Login))
                    {
                        cmd.CreateStoreProzedure("dbo.IPBlock_Insert");

                        cmd.SetParameter("@pBlockedIP", IP);
                        cmd.SetParameter("@pBlockDate", BlockDate.Value);
                        cmd.SetParameter("@pBlockReason", BlockReason);

                        switch ((int)cmd.ExecuteScalar())
                        {
                            case 0:
                                return BlocksByIP.TryAdd(IP, new IPBlockEntry(IP, BlockDate.Value, BlockReason));

                            case -1:
                            case -2:
                            default:
                                return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GameLog.Write(ex, "Error blocking IP '{0}':", IP);
                return false;
            }
        }
    }
}