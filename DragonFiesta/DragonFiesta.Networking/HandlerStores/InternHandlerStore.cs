using DragonFiesta.Messages.Utils;
using DragonFiesta.Networking.Network;
using DragonFiesta.Utils.Core;
using System;
using System.Collections.Concurrent;

namespace DragonFiesta.Networking.HandlerStores
{
    [ServerModule(ServerType.Zone, InitializationStage.InternNetwork)]
    [ServerModule(ServerType.Login, InitializationStage.InternNetwork)]
    public class InternHandlerStore
    {
        public static InternHandlerStore Instance { get; set; }
        protected ConcurrentDictionary<Type, Action<IMessage, InternSession>> _handlers;
        protected ConcurrentDictionary<Guid, IExpectAnAnswer> _callbacks;
        protected ExpireExpectManager ReponseManager;

        [InitializerMethod]
        public static bool Initialize()
        {
            Instance = new InternHandlerStore
            {
                _callbacks = new ConcurrentDictionary<Guid, IExpectAnAnswer>(),
                _handlers = new ConcurrentDictionary<Type, Action<IMessage, InternSession>>(),
                ReponseManager = new ExpireExpectManager((int)ServerTaskTimes.SERVER_INTERN_MSG_RESPONSE_CHECK),
            };
            ThreadPool.AddUpdateAbleServer(Instance.ReponseManager);
            InitializeHandlers();
            return true;
        }

        public static void InitializeHandlers()
        {
            var pairs = Reflector.Global.GetMethodsWithAttribute<InternMessageHandlerAttribute>();
            foreach (var pair in pairs)
            {
                Action<dynamic, InternSession> del = (message, client) => pair.Item2.Invoke(null, new object[] { message, client });
                if (!Instance._handlers.TryAdd(pair.Item1.Type, del))
                {
                    EngineLog.Write(EngineLogLevel.Warning, "Dublicate InternHandler {0} Found!!", pair.Item1.Type.ToString());
                }
            }

            EngineLog.Write(EngineLogLevel.Startup, "Loaded {0} InterHandlers.", Instance._handlers.Count);
        }

        public void AddCallBack(IMessage msg)
        {
            if (!_callbacks.ContainsKey(msg.Id))
            {
                _callbacks.TryAdd(msg.Id, (IExpectAnAnswer)msg);
                ReponseManager.AddObject((IExpectAnAnswer)msg);
            }
            else
            {
                SocketLog.Write(SocketLogLevel.Warning, "Dublicate Callback Detect msg: {0} Id: {1}", msg.GetType(), msg.Id);
                _callbacks.TryRemove(msg.Id, out IExpectAnAnswer val);
                _callbacks.TryAdd(msg.Id, (IExpectAnAnswer)msg);
                ReponseManager.RemoveObject(msg.Id);
            }
        }

        public bool IsCallbackContains(Guid Id) => _callbacks.ContainsKey(Id);

        public bool CallMessage<Tsession>(IMessage pMessage, InternSession mSession)
            where Tsession : InternSession
        {
            if (Instance._handlers.ContainsKey(pMessage.GetType()))
            {
                Instance._handlers[pMessage.GetType()](pMessage, mSession);
                return true;
            }
            return false;
        }

        public virtual void HandleMessage<TSession>(TSession pSession, IMessage pMessage)
            where TSession : InternSession
        {
            if (ServerMainDebug.DebugPackets)
                SocketLog.Write(SocketLogLevel.Debug, "Got message of type {0}", pMessage.GetType());

            if (Instance._callbacks.ContainsKey(pMessage.Id)
                && ReponseManager.MessageObjects.TryGetValue(pMessage.Id, out IExpectAnAnswer mRequest))
            {
                ReponseManager.RemoveObject(pMessage.Id);
            }

            if (Instance._callbacks.ContainsKey(pMessage.Id))
            {
                Instance._callbacks[pMessage.Id]?.Callback(pMessage);
                Instance._callbacks.TryRemove(pMessage.Id, out IExpectAnAnswer val);
            }
            else if (Instance._handlers.ContainsKey(pMessage.GetType()))
            {
                Instance._handlers[pMessage.GetType()](pMessage, pSession);
            }
            else
            {
                SocketLog.Write(SocketLogLevel.Debug, "Unkown message found of type {0} \n {1}", pMessage.GetType(), pMessage.ToString());
            }
        }
    }
}