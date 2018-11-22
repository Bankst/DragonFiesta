using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using DFEngine.Logging;

namespace DFEngine.Network
{
	/// <summary>
	/// Class that listens for incoming network connections using the
	/// TCP socket protocol.
	/// </summary>
	public class NetworkServer : Object
	{
		/// <summary>
		/// The default backlog size, which will be used if none is provided.
		/// </summary>
		public const int BacklogSize = 100;

		/// <summary>
		/// A list of the active connections to the <see cref="NetworkServer"/>.
		/// </summary>
		public List<NetworkConnection> Connections { get; }

		/// <summary>
		/// Returns true if this server listens for local (server-to-server) connections.
		/// </summary>
		public bool IsInterService => _connectionType != NetworkConnectionType.NCT_CLIENT;

		/// <summary>
		/// The type of connections the server is listening for.
		/// </summary>
		private readonly NetworkConnectionType _connectionType;

		/// <summary>
		/// The socket that listens for connections.
		/// </summary>
		private readonly Socket _socket;

		/// <summary>
		/// Creates a new instance of the <see cref="NetworkServer"/> class.
		/// </summary>
		public NetworkServer(NetworkConnectionType connectionType)
		{
			Connections = new List<NetworkConnection>();
			_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			this._connectionType = connectionType;
		}

		/// <summary>
		/// Places the server in the listening state.
		/// </summary>
		/// <param name="ipAddress">The IP address to listen for connection on.</param>
		/// <param name="port">The port to listen for connections on.</param>
		/// <param name="backlog">The maximum number of pending connections in the queue.</param>
		public void Listen(string ipAddress, ushort port, int backlog = BacklogSize)
		{
			if (!IPAddress.TryParse(ipAddress, out var address))
			{
				SocketLog.Write(SocketLogLevel.Exception, $"The IPAddress {ipAddress} is invalid.");
				return;
			}

			_socket.Bind(new IPEndPoint(address, port));
			_socket.Listen(backlog);
			_socket.BeginAccept(AcceptConnection, _socket);
		}

		/// <summary>
		/// Accepts a pending connection.
		/// </summary>
		/// <param name="e">The status of the operation.</param>
		private void AcceptConnection(IAsyncResult e)
		{
			if (e.AsyncState == null)
			{
				SocketLog.Write(SocketLogLevel.Exception, "NetworkServer::AcceptConnection : AsyncState is null");
				return;
			}

			var serverSocket = (Socket)e.AsyncState;
			var clientSocket = serverSocket.EndAccept(e);

			if (clientSocket != null)
			{
				var connection = new NetworkConnection(this, clientSocket, _connectionType);
				if (connection.IsConnected)
				{
					Connections.Add(connection);
				}
			}

			// Begin accepting connections again.
			serverSocket.BeginAccept(AcceptConnection, serverSocket);
		}

		/// <summary>
		/// Destroys the <see cref="NetworkServer"/> instance.
		/// </summary>
		protected override void Destroy()
		{
			// Close() calls the dispose method for us.
			_socket.Close();
		}
	}
}