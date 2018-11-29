namespace DFEngine.Server
{
	public enum ConnectionError : ushort
	{
		FailedToConnectToWorldServer = 321,
		FailedToConnectToMapServer = 323,
		ThereIsNoCharacterInTheSlot = 324,
		MapUnderMaintenace = 325,
		ClientManipulation = 327,
		ClientDataError = 328,
	}
}
