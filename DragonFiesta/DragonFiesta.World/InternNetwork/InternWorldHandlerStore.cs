using DragonFiesta.Messages.Utils;
using DragonFiesta.Networking.HandlerStores;
using DragonFiesta.Networking.Network;
using DragonFiesta.World.Game.Zone;
using System;
using System.Collections.Concurrent;
using DragonFiesta.Utils.Logging;

namespace DragonFiesta.World.InternNetwork
{
    [ServerModule(ServerType.World, InitializationStage.InternNetwork)]
    public class InternWorldHandlerStore : InternHandlerStore
    {

        [InitializerMethod]
        public new static bool Initialize()
        {
            Instance = new InternWorldHandlerStore
            {
                Callbacks = new ConcurrentDictionary<Guid, IExpectAnAnswer>(),
                Handlers = new ConcurrentDictionary<Type, Action<IMessage, InternSession>>(),
                ReponseManager = new ExpireExpectManager((int)ServerTaskTimes.SERVER_INTERN_MSG_RESPONSE_CHECK),
               
            };

            InitializeHandlers();


            return true;
        }

      

        #region ZoneHandleTypes


        private void HandleZoneToAll(IZoneToAll allMessage, InternZoneSession pSession)
        {
	        if (allMessage == null) throw new ArgumentNullException(nameof(allMessage));
	        if (!pSession.IsAuthenticated)
                return;

            if (Handlers.ContainsKey(allMessage.GetType()))
            {
                Handlers[allMessage.GetType()](allMessage, pSession);
            }

            ZoneManager.Broadcast(allMessage, pSession.Zone.ID);
        }

        private void HandleZoneToZoneMessage(IZoneToZoneMessage message, InternZoneSession pSession)
        {
	        if (pSession == null) throw new ArgumentNullException(nameof(pSession));
	        if (!ZoneManager.GetZoneByID(message.DestZone, out var mZone))
            {
                GameLog.Write(GameLogLevel.Warning, $"Can't Send {message.GetType()} Zone {message.DestZone } is not Exis..");
                return;
            }
            else if (!mZone.IsConnected)
            {
                GameLog.Write(GameLogLevel.Warning, $"Can't Send {message.GetType() } Zone {message.DestZone } is not Connectet");
                return;
            }

            if (Handlers.ContainsKey(message.GetType()))
            {
                Handlers[message.GetType()](message, pSession);
            }

            mZone.Send(message);
        }

        #endregion ZoneHandleTypes

       
        //HMM God idea???
        public override void HandleMessage<TSession>(TSession pSession, IMessage pMessage)
        {
            switch (pMessage)
            {
                case IZoneToZoneMessage message:
                    HandleZoneToZoneMessage(message, pSession as InternZoneSession);//throwed exp when not call by internzone...^^
                    break;

                case IZoneToAll message:
                    HandleZoneToAll(message, pSession as InternZoneSession);
                    break;

                default:
                    base.HandleMessage(pSession, pMessage);
                    break;
            }
        }
    }
}