using DragonFiesta.Networking.Helpers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DragonFiesta.Networking.Network.Session;

namespace DragonFiesta.Networking.Network.Managers
{
    public class AccountSessionManager<TSession> : FiestaSessionManagerBase<TSession>
           where TSession : FiestaSession<TSession>
    {

        public static new AccountSessionManager<TSession> Instance
        {
            get => _Instance as AccountSessionManager<TSession>;
        }
        private ConcurrentDictionary<int, TSession> SessionByAccountId { get; set; }

        public AccountSessionManager(ushort MaxSessions)
            : base(MaxSessions)
        {
            SessionByAccountId = new ConcurrentDictionary<int, TSession>();

        }

        public bool AddAccount(int AccountId, TSession Session)
        {
            if (SessionByAccountId.TryGetValue(AccountId, out TSession OnlineSession))
            {

                _SH03Helpers.SendDuplicateLogin(OnlineSession);
                OnlineSession.Dispose();

                return SessionByAccountId.TryAdd(AccountId, Session);
            }

            return SessionByAccountId.TryAdd(AccountId, Session);
        }

        public bool RemoveAccount(int AccountId, out TSession Session)
             => SessionByAccountId.TryRemove(AccountId, out Session);


        public bool GetAccount(int AccountId, out TSession Session)
            => SessionByAccountId.TryRemove(AccountId, out Session);

        public List<int> GetAccountList() => SessionByAccountId.Keys.ToList();

    }
}
