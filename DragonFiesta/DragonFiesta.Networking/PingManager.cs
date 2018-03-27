using DragonFiesta.Networking.Helpers;
using DragonFiesta.Networking.Network;
using System;
using System.Collections.Generic;

namespace DragonFiesta.Networking
{
    public class PingManager<TSession> : IServerTask where TSession : FiestaSession<TSession>
    {
        public static PingManager<TSession> Instance { get; set; }

        ServerTaskTimes IServerTask.Interval => ServerTaskTimes.SESSION_GAME_PING_SYNC;

        GameTime IServerTask.LastUpdate { get; set; }

        private SecureWriteCollection<FiestaSession<TSession>> ClientList;
        private Func<FiestaSession<TSession>, bool> ClientListFunc_Add;
        private Func<FiestaSession<TSession>, bool> ClientListFunc_Remove;
        private Action ClientListFunc_Clear;

        private object ThreadLocker;

        protected PingManager()
        {
            ClientList = new SecureWriteCollection<FiestaSession<TSession>>(out ClientListFunc_Add, out ClientListFunc_Remove, out ClientListFunc_Clear);

            ThreadLocker = new object();
        }

        public bool RegisterClient(FiestaSession<TSession> Client)
        {
            lock (ThreadLocker)
            {
                return ClientListFunc_Add.Invoke(Client);
            }
        }

        public bool RevokeClient(FiestaSession<TSession> Client)
        {
            lock (ThreadLocker)
            {
                return ClientListFunc_Remove.Invoke(Client);
            }
        }

        public bool CanCheck(FiestaSession<TSession> mSession)
        {
            return mSession.GameStates.IsReady;
        }

        public void Dispose()
        {
        }

        bool IServerTask.Update(GameTime Now)
        {
            var now = DateTime.Now;

            lock (ThreadLocker)
            {
                var timedOut = new List<FiestaSession<TSession>>();

                for (int i = 0; i < ClientList.Count; i++)
                {
                    var client = ClientList[i];

                    try
                    {
                        //we check the clients every 30 secs
                        if (now.Subtract(client.GameStates.LastPing).TotalSeconds < 30
                            || !CanCheck(client))
                            continue;

                        if (client.GameStates.HasPong)
                        {
                            client.GameStates.HasPong = false;
                            client.GameStates.LastPing = now;
                            _SH02Helpers.SendPing(client);
                        }
                        else
                        {
                            timedOut.Add(client);
                        }
                    }
                    catch (Exception)
                    {
                        timedOut.Add(client);

                        continue;
                    }
                }

                for (int i = 0; i < timedOut.Count; i++)
                {
                    var client = timedOut[i];

                    try
                    {
                        ClientListFunc_Remove.Invoke(client);
                        client.Dispose();
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }

                timedOut.Clear();
                timedOut = null;
            }
            return true;
        }
    }
}