namespace DragonFiesta.Networking.Network
{
    public class FiestaServer<TSession> : ServerBase
        where TSession : FiestaSession<TSession>
    {
        protected ClientRegion Region { get; set; }

        public FiestaServer(ClientRegion mRegion, int port) : base(port)
        {
            this.Region = mRegion;
        }
    }
}