using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using DFEngine.Accounts;
using DFEngine.Logging;
using DFEngine.Network.Protocols;
using DFEngine.Utils;

namespace DFEngine.Network
{
	/// <summary>
	/// Class that sends and receives data to and from a server.
	/// </summary>
	public class NetworkConnection : Object
	{
		/// <summary>
		/// The maximum buffer size allowed.
		/// </summary>
		private const int ReceiveBufferSize = ushort.MaxValue;

		/// <summary>
		/// Array of xor values.
		/// </summary>
		private static readonly byte[] XorTable =
		{
			7, 89, 105, 74, 148, 17, 148, 133, 140, 136, 5, 203, 160, 158, 205, 88, 58, 54, 91, 26, 106, 22, 254, 189,
			223, 148, 2, 248, 33, 150, 200, 233, 158, 247, 191, 189, 207, 205, 178, 122, 0, 159, 64, 34, 252, 17, 249,
			12, 46, 18, 251, 167, 116, 10, 125, 120, 64, 30, 44, 160, 45, 6, 203, 168, 185, 126, 239, 222, 73, 234, 78,
			19, 22, 22, 128, 244, 61, 194, 154, 212, 134, 215, 148, 36, 23, 244, 214, 101, 189, 63, 219, 228, 225, 15,
			80, 246, 236, 122, 154, 12, 39, 61, 36, 102, 211, 34, 104, 156, 154, 82, 11, 224, 249, 165, 11, 37, 218,
			128, 73, 13, 253, 62, 119, 209, 86, 168, 183, 244, 15, 155, 232, 15, 82, 71, 245, 111, 131, 32, 34, 219, 15,
			11, 177, 67, 133, 193, 203, 164, 11, 2, 25, 223, 240, 139, 236, 219, 108, 109, 102, 173, 69, 190, 137, 20,
			126, 47, 137, 16, 184, 147, 96, 216, 96, 222, 246, 254, 110, 155, 202, 6, 193, 117, 149, 51, 207, 192, 178,
			224, 204, 165, 206, 18, 246, 229, 181, 180, 38, 197, 178, 24, 79, 42, 93, 38, 27, 101, 77, 245, 69, 201,
			132, 20, 220, 124, 18, 75, 24, 156, 199, 36, 231, 60, 100, 255, 214, 58, 44, 238, 140, 129, 73, 57, 108,
			183, 220, 189, 148, 226, 50, 247, 221, 10, 252, 2, 1, 100, 236, 76, 148, 10, 177, 86, 245, 201, 169, 52,
			222, 15, 56, 39, 188, 129, 48, 15, 123, 56, 37, 254, 232, 62, 41, 186, 85, 67, 191, 107, 159, 31, 138, 73,
			82, 24, 127, 138, 248, 136, 36, 92, 79, 225, 168, 48, 135, 142, 80, 31, 47, 209, 12, 180, 253, 10, 188, 220,
			18, 133, 226, 82, 238, 74, 88, 56, 171, 255, 198, 61, 185, 96, 100, 10, 180, 80, 213, 64, 137, 23, 154, 213,
			133, 207, 236, 13, 126, 129, 127, 227, 195, 4, 1, 34, 236, 39, 204, 250, 62, 33, 166, 84, 200, 222, 0, 182,
			223, 39, 159, 246, 37, 52, 7, 133, 191, 167, 165, 165, 224, 131, 12, 61, 93, 32, 64, 175, 96, 163, 100, 86,
			243, 5, 196, 28, 125, 55, 152, 195, 232, 90, 110, 88, 133, 164, 154, 107, 106, 244, 163, 123, 97, 155, 9,
			64, 30, 96, 75, 50, 217, 81, 164, 254, 249, 93, 78, 74, 251, 74, 212, 124, 51, 2, 51, 213, 157, 206, 91,
			170, 90, 124, 216, 248, 5, 250, 31, 43, 140, 114, 87, 80, 174, 108, 25, 137, 202, 1, 252, 252, 41, 155, 97,
			18, 104, 99, 101, 70, 38, 196, 91, 80, 170, 43, 190, 239, 154, 121, 2, 35, 117, 44, 32, 19, 253, 217, 90,
			118, 35, 241, 11, 181, 184, 89, 249, 159, 122, 230, 6, 233, 165, 58, 180, 80, 191, 22, 88, 152, 179, 154,
			110, 54, 238, 141, 235
		};
		
		/// <summary>
		/// The connection's account.
		/// </summary>
		public Account Account { get; set; }

		/// <summary>
		/// The connection's unique dentifier.
		/// </summary>
		public string Guid { get; set; }

		/// <summary>
		/// The client's handle.
		/// </summary>
		public ushort Handle { get; set; }

		/// <summary>
		/// Returns true if the connection exists.
		/// </summary>
		public bool IsConnected => GetConnectionState();

		/// <summary>
		/// Returns true if the handshake has been completed.
		/// </summary>
		public bool IsEstablished { get; set; }

		/// <summary>
		/// Returns true if the connection is currently being sent data in chunks.
		/// </summary>
		public bool SendChunk
		{
			get => sendChunk;
			set
			{
				sendChunk = value;

				if (!value)
				{
					SendAwaitingBuffers();
				}
			}
		}

		/// <summary>
		/// The server that accepted the connection
		/// </summary>
		public NetworkServer Server { get; }

		/// <summary>
		/// The connection's type.
		/// </summary>
		public NetworkConnectionType Type { get; }

		/// <summary>
		/// List of buffers waiting to be sent.
		/// </summary>
		private volatile List<byte[]> awaitingBuffers;

		/// <summary>
		/// The buffer used for incoming data.
		/// </summary>
		private byte[] receiveBuffer;

		/// <summary>
		/// The stream used to read from the incoming data buffer.
		/// </summary>
		private MemoryStream receiveStream;

		/// <summary>
		/// Returns true if the connection is currently being sent data in chunks.
		/// </summary>
		private bool sendChunk;

		/// <summary>
		/// The connection's current index in the xor table.
		/// </summary>
		private ushort seed;

		/// <summary>
		/// Returns true if the seed was set.
		/// </summary>
		private bool seedSet;

		/// <summary>
		/// Creates a new instance of the <see cref="NetworkConnection"/> class.
		/// </summary>
		public NetworkConnection(NetworkConnectionType type)
		{
			socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			Type = type;
			IsEstablished = true;
			Guid = System.Guid.NewGuid().ToString().Replace("-", "");
			Handle = (ushort)Mathd.Random(ushort.MaxValue);
			receiveStream = new MemoryStream();
			awaitingBuffers = new List<byte[]>();
		}

		/// <summary>
		/// Creates a new instance of the <see cref="NetworkConnection"/> class.
		/// </summary>
		/// <param name="server">The server that accepted the connection.</param>
		/// <param name="socket">The socket to use for data transferring.</param>
		/// <param name="type">The connection's type.</param>
		public NetworkConnection(NetworkServer server, Socket socket, NetworkConnectionType type)
		{
			Server = server;
			this.socket = socket;
			Type = type;
			Guid = System.Guid.NewGuid().ToString().Replace("-", "");
			Handle = (ushort)Mathd.Random(ushort.MaxValue);
			receiveStream = new MemoryStream();
			awaitingBuffers = new List<byte[]>();

			SetSeed((ushort)Mathd.Random(498));

			BeginReceivingData();
			new PROTO_NC_MISC_SEED_ACK(seed).Send(this);
			IsEstablished = true;

			if (Type != NetworkConnectionType.NCT_CLIENT)
			{
				new PROTO_NC_MISC_S2SCONNECTION_RDY().Send(this);
			}
		}

		/// <summary>
		/// The socket to use for data transferring.
		/// </summary>
		private Socket socket;

		/// <summary>
		/// Gets the sockets remote endpoint IP address
		/// </summary>
		/// <returns>Remote endpoint IP address</returns>
		public string GetRemoteIP => (socket.RemoteEndPoint as IPEndPoint)?.Address.ToString();

		/// <summary>
		/// Gets the socket's connection state.
		/// </summary>
		/// <returns>True if the socket is connected.</returns>
		private bool GetConnectionState()
		{
			// If the socket is already null, we've already called
			// Disconnect().
			if (socket == null || !socket.Connected)
			{
				return false;
			}

			var blocking = socket.Blocking;

			try
			{
				var tempBuffer = new byte[1];

				socket.Blocking = false;
				socket.Send(tempBuffer, 0, 0);

				// If the send fails, this line won't be reached because an exception
				// will be thrown.
				return true;
			}
			catch (SocketException e)
			{
				// 10035 == WSAEWOULDBLOCK
				var ret = e.NativeErrorCode == 10035;

				if (!ret)
				{
					Disconnect();
				}

				return ret;
			}
			finally
			{
				if (socket != null)
				{
					socket.Blocking = blocking;
				}
			}
		}

		/// <summary>
		/// Decrypts a buffer.
		/// </summary>
		/// <param name="buffer">The buffer to decrypt.</param>
		/// <param name="offset">The offset to start decrypting.</param>
		/// <param name="length">The length to decrypt.</param>
		public void DecryptBuffer(byte[] buffer, int offset, int length)
		{
			for (var i = 0; i < length; i++)
			{
				buffer[offset + i] ^= XorTable[seed];
				seed++;

				if (seed >= 499)
				{
					seed = 0;
				}
			}
		}

		/// <summary>
		/// Destroys the connection.
		/// </summary>
		public void Disconnect()
		{
			// May not want to remove here, might want to handle disconnections
			// individually per server by checking the list for disconnected
			// clients.
			Server?.Connections.Remove(this);

			socket?.Close(); // Close() will call Dispose() automatically for us.
			socket = null;
		}

		/// <summary>
		/// Sends data to the connection.
		/// </summary>
		/// <param name="buffer">The data to send.</param>
		public void SendData(byte[] buffer)
		{
			if (!IsConnected)
			{
				return;
			}

			if (sendChunk)
			{
				awaitingBuffers.Add(buffer);
				return;
			}

			var bytesToSend = buffer.Length;
			var bytesSent = 0;

			if (bytesToSend >= ReceiveBufferSize)
			{
				SocketLog.Write(SocketLogLevel.Debug, "Exceeded max message size while sending data to a connection.");
			}

			while (bytesSent < bytesToSend)
			{
				bytesSent += socket.Send(buffer, bytesSent, bytesToSend - bytesSent, SocketFlags.None);

				if (bytesSent <= bytesToSend) continue;
				SocketLog.Write(SocketLogLevel.Warning, $"BUFFER OVERFLOW OCCURRED - Sent {bytesSent - bytesToSend} bytes more than expected.");
				break;
			}
		}

		/// <summary>
		/// Sets the client's current seed.
		/// </summary>
		/// <param name="value">The value to set the seed to.</param>
		public void SetSeed(ushort value)
		{
			seed = value;
			seedSet = true;
		}

		/// <summary>
		/// Begins to accept data from the connection.
		/// </summary>
		private void BeginReceivingData()
		{
			if (!IsConnected)
			{
				return;
			}

			receiveBuffer = new byte[ReceiveBufferSize];
			socket.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, ReceivedData, null);
		}

		/// <summary>
		/// Attempts to connect to the specified target endpoint.
		/// </summary>
		/// <param name="targetIP">The target IP Address.</param>
		/// <param name="targetPort">The target port.</param>
		public void Connect(string targetIP, ushort targetPort)
		{
			socket.BeginConnect(new IPEndPoint(IPAddress.Parse(targetIP), targetPort), ConnectedToTarget, new object[] { targetIP, targetPort });
		}

		/// <summary>
		/// This callback is called when we have connected to the requested
		/// target endpoint.
		/// </summary>
		/// <param name="e">The result object.</param>
		private void ConnectedToTarget(IAsyncResult e)
		{
			try
			{
				socket.EndConnect(e);
				BeginReceivingData();
			}
			catch
			{
				SocketLog.Write(SocketLogLevel.Warning, "Remote socket connection attempt failed. Trying again...");
				Thread.Sleep(3000); // 3 seconds.
				Connect((string)((object[])e.AsyncState)[0], (ushort)((object[])e.AsyncState)[1]);
			}
		}

		/// <summary>
		/// Destroys the <see cref="NetworkConnection"/> instance.
		/// </summary>
		protected override void Destroy()
		{
			socket?.Close();
		}

		/// <summary>
		/// Gets all messages from the buffer.
		/// </summary>
		/// <param name="buffer">The buffer to get message from.</param>
		private void GetMessagesFromBuffer(byte[] buffer)
		{
			if (!IsConnected)
			{
				return;
			}

			receiveStream.Write(buffer, 0, buffer.Length);

			while (TryParseMessage())
			{

			}
		}

		/// <summary>
		/// Called when data was received from the connection.
		/// </summary>
		/// <param name="e">The status of the operation.</param>
		private void ReceivedData(IAsyncResult e)
		{
			if (!IsConnected)
			{
				return;
			}

			var count = socket.EndReceive(e);
			var buffer = new byte[count];

			if (count <= 0)
			{
				Disconnect();
				return;
			}

			Array.Copy(receiveBuffer, 0, buffer, 0, count);
			GetMessagesFromBuffer(buffer);

			BeginReceivingData();
		}

		/// <summary>
		/// Sends the awaiting buffers as one big chunk.
		/// </summary>
		private void SendAwaitingBuffers()
		{
			if (!IsConnected)
			{
				return;
			}

			awaitingBuffers.Copy(out var bufferList);
			awaitingBuffers.Clear();

			var size = bufferList.Sum(b => b.Length);
			var chunk = new byte[size];
			var pointer = 0;

			for (var i = 0; i < bufferList.Count; i++)
			{
				var buffer = bufferList[i];

				Array.Copy(buffer, 0, chunk, pointer, buffer.Length);
				pointer += buffer.Length;
			}

			SendData(chunk);
		}

		/// <summary>
		/// Tries to parse a message from the receive stream.
		/// </summary>
		private bool TryParseMessage()
		{
			if (!IsConnected)
			{
				return false;
			}

			receiveStream.Position = 0;

			if (receiveStream.Length < 1)
			{
				return false;
			}

			ushort messageSize;
			var sizeBuffer = new byte[1];

			receiveStream.Read(sizeBuffer, 0, 1);

			if (sizeBuffer[0] != 0)
			{
				messageSize = sizeBuffer[0];
			}
			else
			{
				if (receiveStream.Length - receiveStream.Position < 2)
				{
					return false;
				}

				sizeBuffer = new byte[2];
				receiveStream.Read(sizeBuffer, 0, 2);

				messageSize = BitConverter.ToUInt16(sizeBuffer, 0);
			}

			if (receiveStream.Length - receiveStream.Position < messageSize)
			{
				return false;
			}

			var messageBuffer = new byte[messageSize];
			receiveStream.Read(messageBuffer, 0, messageSize);

			if (seedSet)
			{
				DecryptBuffer(messageBuffer, 0, messageBuffer.Length);
			}

			var message = new NetworkMessage(messageBuffer);
			if (!NetworkMessageHandler.TryFetch(message.Command, out var handler) || handler == null)
			{
				SocketLog.Write(SocketLogLevel.Debug, $"Unhandled command: {message.Command}");
			}

			NetworkMessageHandler.Invoke(handler, message, this);

			// Trims the receive stream.
			var remainingByteCount = new byte[receiveStream.Length - receiveStream.Position];
			receiveStream.Read(remainingByteCount, 0, remainingByteCount.Length);
			receiveStream = new MemoryStream();
			receiveStream.Write(remainingByteCount, 0, remainingByteCount.Length);

			return true;
		}
	}
}
