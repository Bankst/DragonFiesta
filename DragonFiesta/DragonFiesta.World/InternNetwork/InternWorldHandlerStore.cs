using DragonFiesta.Messages.Utils;
using DragonFiesta.Networking.HandlerStores;
using DragonFiesta.Networking.Network;
using DragonFiesta.World.Game.Zone;
using System;
using System.Collections.Concurrent;

namespace DragonFiesta.World.InternNetwork
{
    [ServerModule(ServerType.World, InitializationStage.InternNetwork)]
    public class InternWorldHandlerStore : InternHandlerStore
    {

        [InitializerMethod]
        public static new bool Initialize()
        {
            Instance = new InternWorldHandlerStore
            {
                _callbacks = new ConcurrentDictionary<Guid, IExpectAnAnswer>(),
                _handlers = new ConcurrentDictionary<Type, Action<IMessage, InternSession>>(),
                ReponseManager = new ExpireExpectManager((int)ServerTaskTimes.SERVER_INTERN_MSG_RESPONSE_CHECK),
               
            };

            InitializeHandlers();


            return true;
        }

      

        #region ZoneHandleTypes


        private void HandleZoneToAll(IZoneToAll AllMessage, InternZoneSession pSession)
        {
            if (!pSession.IsAuthenticated)
                return;

            if (_handlers.ContainsKey(AllMessage.GetType()))
            {
                _handlers[AllMessage.GetType()](AllMessage, pSession);
            }

            ZoneManager.Broadcast(AllMessage, pSession.Zone.ID);
        }

        private void HandleZoneToZoneMessage(IZoneToZoneMessage Message, InternZoneSession pSession)
        {
            if (!ZoneManager.GetZoneByID(Message.DestZone, out ZoneServer mZone))
            {
                GameLog.Write(GameLogLevel.Warning, $"Can not Send {Message.GetType()} Zone {Message.DestZone } is not Exis..");
                return;
            }
            else if (!mZone.IsConnected)
            {
                GameLog.Write(GameLogLevel.Warning, $"Can not Send {Message.GetType() } Zone {Message.DestZone } is not Connectet");
                return;
            }

            if (_handlers.ContainsKey(Message.GetType()))
            {
                _handlers[Message.GetType()](Message, pSession);
            }

            mZone.Send(Message);
        }

        #endregion ZoneHandleTypes

       
        //HMM God idea???
        public override void HandleMessage<TSession>(TSession pSession, IMessage pMessage)
        {
            switch (pMessage)
            {
                case IZoneToZoneMessage Message:
                    HandleZoneToZoneMessage(Message, pSession as InternZoneSession);//throwed exp when not call by internzone...^^
                    break;

                case IZoneToAll Message:
                    HandleZoneToAll(Message, pSession as InternZoneSession);
                    break;

                default:
                    base.HandleMessage(pSession, pMessage);
                    break;
            }
        }
    }
}