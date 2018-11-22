using DFEngine.Network;

namespace DFEngine.Server
{
	/// <summary>
	/// Class that represents an available world that
	/// players can join.
	/// </summary>
	public class World
	{
		/// <summary>
		/// This is the world's network connection. It allows us to send data
		/// to the world for things like transfers and such.
		/// </summary>
		public NetworkConnection Connection { get; set; }

		/// <summary>
		/// The IP of the world. Players connect to this IP Address.
		/// </summary>
		public string IP { get; set; }

		/// <summary>
		/// The name of the world.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The world's number.
		/// </summary>
		public byte Number { get; set; }

		/// <summary>
		/// The port of the world. Players connect to this port.
		/// </summary>
		public ushort Port { get; set; }

		/// <summary>
		/// The world's current status.
		/// </summary>
		public byte Status { get; set; }

		/// <summary>
		/// Creates a new instance of the <see cref="World"/> class.
		/// </summary>
		public World(NetworkConnection connection, string name, byte number, string ip, ushort port)
		{
			Connection = connection;
			Name = name;
			Number = number;
			IP = ip;
			Port = port;

			// TODO
			/* Because this is an emulator, we're not really concerned
            about the status of a world server. Can be re-evaluated after
            some testing has been done concerning stress. */
			Status = 0x06; // Low
		}
	}
}
