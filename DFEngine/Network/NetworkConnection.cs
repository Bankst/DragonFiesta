using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using DFEngine.Accounts;
using DFEngine.Content.GameObjects;
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
			0x07, 0x59, 0x69, 0x4A, 0x94, 0x11, 0x94, 0x85, 0x8C, 0x88, 0x05, 0xCB,
			0xA0, 0x9E, 0xCD, 0x58, 0x3A, 0x36, 0x5B, 0x1A, 0x6A, 0x16, 0xFE, 0xBD,
			0xDF, 0x94, 0x02, 0xF8, 0x21, 0x96, 0xC8, 0xE9, 0x9E, 0xF7, 0xBF, 0xBD,
			0xCF, 0xCD, 0xB2, 0x7A, 0x00, 0x9F, 0x40, 0x22, 0xFC, 0x11, 0xF9, 0x0C,
			0x2E, 0x12, 0xFB, 0xA7, 0x74, 0x0A, 0x7D, 0x78, 0x40, 0x1E, 0x2C, 0xA0,
			0x2D, 0x06, 0xCB, 0xA8, 0xB9, 0x7E, 0xEF, 0xDE, 0x49, 0xEA, 0x4E, 0x13,
			0x16, 0x16, 0x80, 0xF4, 0x3D, 0xC2, 0x9A, 0xD4, 0x86, 0xD7, 0x94, 0x24,
			0x17, 0xF4, 0xD6, 0x65, 0xBD, 0x3F, 0xDB, 0xE4, 0xE1, 0x0F, 0x50, 0xF6,
			0xEC, 0x7A, 0x9A, 0x0C, 0x27, 0x3D, 0x24, 0x66, 0xD3, 0x22, 0x68, 0x9C,
			0x9A, 0x52, 0x0B, 0xE0, 0xF9, 0xA5, 0x0B, 0x25, 0xDA, 0x80, 0x49, 0x0D,
			0xFD, 0x3E, 0x77, 0xD1, 0x56, 0xA8, 0xB7, 0xF4, 0x0F, 0x9B, 0xE8, 0x0F,
			0x52, 0x47, 0xF5, 0x6F, 0x83, 0x20, 0x22, 0xDB, 0x0F, 0x0B, 0xB1, 0x43,
			0x85, 0xC1, 0xCB, 0xA4, 0x0B, 0x02, 0x19, 0xDF, 0xF0, 0x8B, 0xEC, 0xDB,
			0x6C, 0x6D, 0x66, 0xAD, 0x45, 0xBE, 0x89, 0x14, 0x7E, 0x2F, 0x89, 0x10,
			0xB8, 0x93, 0x60, 0xD8, 0x60, 0xDE, 0xF6, 0xFE, 0x6E, 0x9B, 0xCA, 0x06,
			0xC1, 0x75, 0x95, 0x33, 0xCF, 0xC0, 0xB2, 0xE0, 0xCC, 0xA5, 0xCE, 0x12,
			0xF6, 0xE5, 0xB5, 0xB4, 0x26, 0xC5, 0xB2, 0x18, 0x4F, 0x2A, 0x5D, 0x26,
			0x1B, 0x65, 0x4D, 0xF5, 0x45, 0xC9, 0x84, 0x14, 0xDC, 0x7C, 0x12, 0x4B,
			0x18, 0x9C, 0xC7, 0x24, 0xE7, 0x3C, 0x64, 0xFF, 0xD6, 0x3A, 0x2C, 0xEE,
			0x8C, 0x81, 0x49, 0x39, 0x6C, 0xB7, 0xDC, 0xBD, 0x94, 0xE2, 0x32, 0xF7,
			0xDD, 0x0A, 0xFC, 0x02, 0x01, 0x64, 0xEC, 0x4C, 0x94, 0x0A, 0xB1, 0x56,
			0xF5, 0xC9, 0xA9, 0x34, 0xDE, 0x0F, 0x38, 0x27, 0xBC, 0x81, 0x30, 0x0F,
			0x7B, 0x38, 0x25, 0xFE, 0xE8, 0x3E, 0x29, 0xBA, 0x55, 0x43, 0xBF, 0x6B,
			0x9F, 0x1F, 0x8A, 0x49, 0x52, 0x18, 0x7F, 0x8A, 0xF8, 0x88, 0x24, 0x5C,
			0x4F, 0xE1, 0xA8, 0x30, 0x87, 0x8E, 0x50, 0x1F, 0x2F, 0xD1, 0x0C, 0xB4,
			0xFD, 0x0A, 0xBC, 0xDC, 0x12, 0x85, 0xE2, 0x52, 0xEE, 0x4A, 0x58, 0x38,
			0xAB, 0xFF, 0xC6, 0x3D, 0xB9, 0x60, 0x64, 0x0A, 0xB4, 0x50, 0xD5, 0x40,
			0x89, 0x17, 0x9A, 0xD5, 0x85, 0xCF, 0xEC, 0x0D, 0x7E, 0x81, 0x7F, 0xE3,
			0xC3, 0x04, 0x01, 0x22, 0xEC, 0x27, 0xCC, 0xFA, 0x3E, 0x21, 0xA6, 0x54,
			0xC8, 0xDE, 0x00, 0xB6, 0xDF, 0x27, 0x9F, 0xF6, 0x25, 0x34, 0x07, 0x85,
			0xBF, 0xA7, 0xA5, 0xA5, 0xE0, 0x83, 0x0C, 0x3D, 0x5D, 0x20, 0x40, 0xAF,
			0x60, 0xA3, 0x64, 0x56, 0xF3, 0x05, 0xC4, 0x1C, 0x7D, 0x37, 0x98, 0xC3,
			0xE8, 0x5A, 0x6E, 0x58, 0x85, 0xA4, 0x9A, 0x6B, 0x6A, 0xF4, 0xA3, 0x7B,
			0x61, 0x9B, 0x09, 0x40, 0x1E, 0x60, 0x4B, 0x32, 0xD9, 0x51, 0xA4, 0xFE,
			0xF9, 0x5D, 0x4E, 0x4A, 0xFB, 0x4A, 0xD4, 0x7C, 0x33, 0x02, 0x33, 0xD5,
			0x9D, 0xCE, 0x5B, 0xAA, 0x5A, 0x7C, 0xD8, 0xF8, 0x05, 0xFA, 0x1F, 0x2B,
			0x8C, 0x72, 0x57, 0x50, 0xAE, 0x6C, 0x19, 0x89, 0xCA, 0x01, 0xFC, 0xFC,
			0x29, 0x9B, 0x61, 0x12, 0x68, 0x63, 0x65, 0x46, 0x26, 0xC4, 0x5B, 0x50,
			0xAA, 0x2B, 0xBE, 0xEF, 0x9A, 0x79, 0x02, 0x23, 0x75, 0x2C, 0x20, 0x13,
			0xFD, 0xD9, 0x5A, 0x76, 0x23, 0xF1, 0x0B, 0xB5, 0xB8, 0x59, 0xF9, 0x9F,
			0x7A, 0xE6, 0x06, 0xE9, 0xA5, 0x3A, 0xB4, 0x50, 0xBF, 0x16, 0x58, 0x98,
			0xB3, 0x9A, 0x6E, 0x36, 0xEE, 0x8D, 0xEB
		};

		private static int[] ClientTable { get; set; }
		private static int[] ServerTable { get; set; }

		/// <summary>
		/// The connection's account.
		/// </summary>
		public Account Account { get; set; }

		/// <summary>
		/// The connection's avatar.
		/// </summary>
		public Avatar Avatar { get; set; }

		/// <summary>
		/// The connection's character.
		/// </summary>
		public Character Character { get; set; }

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
		/// Time since the connection's last heartbeat.
		/// </summary>
		public long LastPing { get; set; }

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

			ClientTable = XorTable.Select(x => (int)x).ToArray();
			ServerTable = XorTable.Select(x => (int)x).ToArray();
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
			if (Type == NetworkConnectionType.NCT_CLIENT)
			{
				var indx = 0;
				while (indx < 499)
				{
					ClientTable[indx] = 3 * XorTable[indx];
					indx++;
				}

				for (var i = 0; i < length; i++)
				{
					buffer[offset + i] = (byte) (buffer[offset + i] ^ ClientTable[seed]);
					seed++;

					if (seed >= 499)
					{
						seed = 0;
					}
				}
			}
			else
			{
				for (var i = 0; i < length; i++)
				{
					buffer[offset + i] = (byte)(buffer[offset + i] ^ ServerTable[seed]);
					seed++;

					if (seed >= 499)
					{
						seed = 0;
					}
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
