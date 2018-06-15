using System;
using System.Collections.Concurrent;

namespace DragonFiesta.Networking.Network
{
    public class SessionBaseManager<TSession> where TSession : SessionBase
    {
        public int CountOfSessions => Sessions.Count;

        private ConcurrentQueue<ushort> SessionIds;

        internal ConcurrentDictionary<ushort, TSession> Sessions { get; set; }

        internal object ThreadLocker { get; set; }

        public static SessionBaseManager<TSession> Instance
        {
            get => _Instance;
            set => _Instance = value;
        }

        protected static SessionBaseManager<TSession> _Instance { get; set; }
        public SessionBaseManager(ushort MaxSessions)
        {
            SessionIds = new ConcurrentQueue<ushort>();
            SessionIds.Fill(ushort.MinValue, MaxSessions);
            Sessions = new ConcurrentDictionary<ushort, TSession>();
            ThreadLocker = new object();



        }

        public virtual void Broadcast<T>(T Packet) where T : class => ClientAction((client) => client.SendPacket(Packet));

        public virtual void Broadcast<T>(T Packet, Predicate<TSession> Match) where T : class => ClientAction((client) => client.SendPacket(Packet), Match);

        public bool AddSession(TSession mSession)
        {
            if (SessionIds.TryDequeue(out ushort SessionId))
            {
                if (Sessions.TryAdd(SessionId, mSession))
                {
                    mSession.BaseStateInfo.SessionId = SessionId;

                    return true;
                }
            }
            return false;
        }

        public virtual bool AllowConnect(TSession Session)
        {
            if (!AddSession(Session))
                return false;

            return true;
        }

        public virtual bool RemoveSession(ushort SessionId)
        {
            if (Sessions.TryRemove(SessionId, out TSession mSession))
            {
                SessionIds.Enqueue(SessionId);

                return true;
            }

            return false;
        }

        public bool RemoveSession(TSession mSession) => RemoveSession(mSession.BaseStateInfo.SessionId);

        public bool GetSessionById(ushort Id, out TSession mSession) => Sessions.TryGetValue(Id, out mSession);

        public void ClientAction(Action<TSession> Action, Predicate<TSession> Match) =>
            ClientAction((client) =>
            {
                if (Match.Invoke(client))
                {
                    Action.Invoke(client);
                }
            });

        public void ClientAction(Action<TSession> Action)
        {
            lock (ThreadLocker)
            {
                foreach (var mClient in Sessions.Values)
                {
                    try
                    {
                        Action.Invoke(mClient);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
        }
    }
}