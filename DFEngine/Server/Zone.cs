using System;
using System.Collections.Generic;
using System.Text;
using DFEngine.Network;

namespace DFEngine.Server
{
	/// <summary>
	/// Class that represents a zone that
	/// players can join.
	/// </summary>
	public class Zone
	{
		/// <summary>
		/// This is the zone's network connection. It allows us to send data
		/// to the world for things like transfers and such.
		/// </summary>
		public NetworkConnection Connection { get; set; }

		/// <summary>
		/// The IP of the zone. Players connect to this IP Address.
		/// </summary>
		public string IP { get; set; }

		/// <summary>
		/// The zone's number.
		/// </summary>
		public byte Number { get; set; }

		/// <summary>
		/// The port of the zone. Players connect to this port.
		/// </summary>
		public ushort Port { get; set; }


		public Zone(NetworkConnection connection, byte number, string ip, ushort port)
		{
			Connection = connection;
			Number = number;
			IP = ip;
			Port = port;
		}
	}
}
