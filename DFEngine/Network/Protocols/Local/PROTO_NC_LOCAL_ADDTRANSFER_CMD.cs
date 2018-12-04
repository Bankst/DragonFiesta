namespace DFEngine.Network.Protocols
{
	public class PROTO_NC_LOCAL_ADDTRANSFER_CMD : NetworkMessage
	{
		public PROTO_NC_LOCAL_ADDTRANSFER_CMD(string validateNew) : base(NetworkCommand.NC_LOCAL_ADDTRANSFER_CMD)
		{
			Write(validateNew, 64);
		}
	}
}
