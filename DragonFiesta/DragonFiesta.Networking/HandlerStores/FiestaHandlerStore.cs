using DragonFiesta.Networking.Network;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DragonFiesta.Networking.HandlerStores
{
    [ServerModule(ServerType.Zone, InitializationStage.Networking)]
    [ServerModule(ServerType.World, InitializationStage.Networking)]
    [ServerModule(ServerType.Login, InitializationStage.Networking)]
    public class FiestaHandlerStore
    {
        public static FiestaHandlerStore Instance { get; private set; }
        private Dictionary<byte, Dictionary<ushort, Dictionary<ClientRegion, MethodInfo>>> packetHandlers;
        private Dictionary<byte, Dictionary<UInt16, string>> AvabileOpocdes;

        public delegate void PacketHandler(FiestaPacket reader);

        public event PacketHandler UnknownPacket;

        [InitializerMethod]
        public static bool Initialize()
        {
            Instance = new FiestaHandlerStore
            {
                packetHandlers = NetworkReflector.GivePacketMethods(),
                AvabileOpocdes = NetworkReflector.GetAvabileOpcodes()
            };

            EngineLog.Write(EngineLogLevel.Startup, "Load {0} Fiesta Packet Handlers", Instance.packetHandlers.Count);
            return true;
        }

        public void PrintALLOpcodes()
        {
            foreach (var opHandler in AvabileOpocdes.Values)
            {
                foreach (var op in opHandler)
                {
                    EngineLog.Write(EngineLogLevel.Info, "HEADER : {0} TYPE : { 1 }", op.Key, op.Value);
                }
            }
        }

        public object[] GetOpcode(byte Header, byte pType)
        {
            return new object[] { AvabileOpocdes[Header][pType] };
        }

        public Dictionary<UInt16, string> GetHandler(byte Header)
        {
            return AvabileOpocdes[Header];
        }

        public FiestaHandlerStore()
        {
            packetHandlers = new Dictionary<byte, Dictionary<ushort, Dictionary<ClientRegion, MethodInfo>>>();
            AvabileOpocdes = new Dictionary<byte, Dictionary<UInt16, string>>();
            UnknownPacket += UnknownPacketHandler;
        }

        protected void UnknownPacketHandler(FiestaPacket pPacket)
        {
            if (!AvabileOpocdes.ContainsKey(pPacket.Header))
            {
                SocketLog.Write(SocketLogLevel.Warning, "No Packet Handler for {0} found", pPacket.Header);
                SocketLog.Write(SocketLogLevel.Warning, pPacket.ToString());
            }
            else if (!AvabileOpocdes[pPacket.Header].ContainsKey(pPacket.Type))
            {
                SocketLog.Write(SocketLogLevel.Warning, "No Packet Type found {0} for Handler {1} ", pPacket.Type, pPacket.Header);
                SocketLog.Write(SocketLogLevel.Warning, pPacket.ToString());
            }
            else
            {
                SocketLog.Write(SocketLogLevel.Warning, $"Unknown packet H{pPacket?.Header} T{pPacket?.Type}");
                SocketLog.Write(SocketLogLevel.Warning, pPacket.ToString());
            }
        }

        public void HandlePacket<T>(FiestaPacket pPacket, FiestaSession<T> pSession)
            where T : FiestaSession<T>
        {
            CallMethod(pPacket.Header, pPacket.Type, pPacket, pSession);
        }

        protected void CallMethod<T>(byte pHeader, UInt16 pType, FiestaPacket pPacket, FiestaSession<T> pSession) 
            where T: FiestaSession<T>
        {
            try
            {
                if (packetHandlers.ContainsKey(pHeader)
                    && packetHandlers[pHeader].ContainsKey(pType))
                {
                    //region
                    if (packetHandlers[pHeader][pType].ContainsKey(pSession.GameStates.Region))
                    {
                        packetHandlers[pHeader][pType][pSession.GameStates.Region].Invoke(this, new object[] { pSession, pPacket });
                    }
                    else if (packetHandlers[pHeader][pType].ContainsKey(ClientRegion.None))
                    {
                        packetHandlers[pHeader][pType][ClientRegion.None].Invoke(this, new object[] { pSession, pPacket });
                    }
                }
                else
                {
                    UnknownPacket?.Invoke(pPacket);
                }
            }
            catch (Exception ex)
            {
                EngineLog.Write(EngineLogLevel.Exception, "Error Handling {0} : {1} {2}", pHeader, pType, ex.ToString());
            }
        }
    }
}