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
        protected ConcurrentDictionary<Type, Action<IMessage, InternSession>> Handlers;
        protected ConcurrentDictionary<Guid, IExpectAnAnswer> Callbacks;
        protected ExpireExpectManager ReponseManager;

        [InitializerMethod]
        public static bool Initialize()
        {
            Instance = new InternHandlerStore
            {
                Callbacks = new ConcurrentDictionary<Guid, IExpectAnAnswer>(),
                Handlers = new ConcurrentDictionary<Type, Action<IMessage, InternSession>>(),
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
	            void Del(dynamic message, InternSession client) => pair.Item2.Invoke(null, new object[] {message, client});
	            if (!Instance.Handlers.TryAdd(pair.Item1.Type, Del))
                {
                    EngineLog.Write(EngineLogLevel.Warning, "Duplicate InternHandler {0} Found!!", pair.Item1.Type.ToString());
                }
            }

            EngineLog.Write(EngineLogLevel.Startup, "Loaded {0} InterHandlers.", Instance.Handlers.Count);
        }

        public void AddCallBack(IMessage msg)
        {
            if (!Callbacks.ContainsKey(msg.Id))
            {
                Callbacks.TryAdd(msg.Id, (IExpectAnAnswer)msg);
                ReponseManager.AddObject((IExpectAnAnswer)msg);
            }
            else
            {
                SocketLog.Write(SocketLogLevel.Warning, "Duplicate Callback Detect msg: {0} Id: {1}", msg.GetType(), msg.Id);
                Callbacks.TryRemove(msg.Id, out var val);
                Callbacks.TryAdd(msg.Id, (IExpectAnAnswer)msg);
                ReponseManager.RemoveObject(msg.Id);
            }
        }

        public bool IsCallbackContains(Guid id) => Callbacks.ContainsKey(id);

        public bool CallMessage<TSession>(IMessage pMessage, InternSession mSession)
            where TSession : InternSession
        {
	        if (!Instance.Handlers.ContainsKey(pMessage.GetType())) return false;
	        Instance.Handlers[pMessage.GetType()](pMessage, mSession);
	        return true;
        }

        public virtual void HandleMessage<TSession>(TSession pSession, IMessage pMessage)
            where TSession : InternSession
        {
            if (ServerMainDebug.DebugPackets)
                SocketLog.Write(SocketLogLevel.Debug, "Got message of type {0}", pMessage.GetType());

            if (Instance.Callbacks.ContainsKey(pMessage.Id)
                && ReponseManager.MessageObjects.TryGetValue(pMessage.Id, out var mRequest))
            {
                ReponseManager.RemoveObject(pMessage.Id);
            }

            if (Instance.Callbacks.ContainsKey(pMessage.Id))
            {
                Instance.Callbacks[pMessage.Id]?.Callback(pMessage);
                Instance.Callbacks.TryRemove(pMessage.Id, out var val);
            }
            else if (Instance.Handlers.ContainsKey(pMessage.GetType()))
            {
                Instance.Handlers[pMessage.GetType()](pMessage, pSession);
            }
            else
            {
                SocketLog.Write(SocketLogLevel.Debug, "Unknown message found of type {0} \n {1}", pMessage.GetType(), pMessage.ToString());
            }
        }
    }
}