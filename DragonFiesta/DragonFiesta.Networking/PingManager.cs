using DragonFiesta.Networking.Helpers;
using DragonFiesta.Networking.Network;
using System;
using System.Collections.Generic;
using DragonFiesta.Networking.Network.Session;

namespace DragonFiesta.Networking
{
    public class PingManager<TSession> : IServerTask where TSession : FiestaSession<TSession>
    {
        public static PingManager<TSession> Instance { get; set; }

        ServerTaskTimes IServerTask.Interval => ServerTaskTimes.SESSION_GAME_PING_SYNC;

        GameTime IServerTask.LastUpdate { get; set; }

        private readonly SecureWriteCollection<FiestaSession<TSession>> _clientList;
        private readonly Func<FiestaSession<TSession>, bool> _clientListFuncAdd;
        private readonly Func<FiestaSession<TSession>, bool> _clientListFuncRemove;
        private Action _clientListFuncClear;

        private readonly object _threadLocker;

        protected PingManager()
        {
            _clientList = new SecureWriteCollection<FiestaSession<TSession>>(out _clientListFuncAdd, out _clientListFuncRemove, out _clientListFuncClear);

            _threadLocker = new object();
        }

        public bool RegisterClient(FiestaSession<TSession> client)
        {
            lock (_threadLocker)
            {
                return _clientListFuncAdd.Invoke(client);
            }
        }

        public bool RevokeClient(FiestaSession<TSession> client)
        {
            lock (_threadLocker)
            {
                return _clientListFuncRemove.Invoke(client);
            }
        }

        public bool CanCheck(FiestaSession<TSession> mSession)
        {
            return mSession.GameStates.IsReady;
        }

        public void Dispose()
        {
        }

        bool IServerTask.Update(GameTime gameTime)
        {
            var now = DateTime.Now;

            lock (_threadLocker)
            {
                var timedOut = new List<FiestaSession<TSession>>();

                foreach (var client in _clientList)
                {
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
	                }
                }

                foreach (var client in timedOut)
                {
	                try
	                {
		                _clientListFuncRemove.Invoke(client);
		                client.Dispose();
	                }
	                catch (Exception)
	                {
		                // ignored
	                }
                }

                timedOut.Clear();
                timedOut = null;
            }
            return true;
        }
    }
}