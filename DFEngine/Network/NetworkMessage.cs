using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DFEngine.Content.GameObjects;

namespace DFEngine.Network
{
	/// <summary>
	/// Class that represents a network message sent to and from a
	/// <see cref="NetworkConnection"/>.
	/// </summary>
	public class NetworkMessage : Object
	{
		/// <summary>
		/// The type of the message.
		/// </summary>
		public NetworkCommand Command { get; set; }

		/// <summary>
		/// Object to read data from the message.
		/// </summary>
		private readonly BinaryReader reader;

		/// <summary>
		/// The stream that contains the message data.
		/// </summary>
		private readonly MemoryStream stream;

		/// <summary>
		/// Object to write data to the message.
		/// </summary>
		private readonly BinaryWriter writer;

		/// <summary>
		/// Creates a new instance of the <see cref="NetworkMessage"/> class.
		/// </summary>
		/// <param name="buffer">The message data.</param>
		public NetworkMessage(byte[] buffer)
		{
			stream = new MemoryStream(buffer);
			reader = new BinaryReader(stream);
			Command = (NetworkCommand)ReadUInt16();
		}

		/// <summary>
		/// Creates a new instance of the <see cref="NetworkMessage"/> class.
		/// </summary>
		/// <param name="command">The type of message.</param>
		public NetworkMessage(NetworkCommand command)
		{
			stream = new MemoryStream();
			writer = new BinaryWriter(stream);
			Command = command;

			Write((ushort)command);
		}

		/// <summary>
		/// Sends multiple messages as a chunk.
		/// </summary>
		public static void SendChunk(NetworkConnection connection, params NetworkMessage[] messages)
		{
			connection.SendChunk = true;

			for (var i = 0; i < messages.Length; i++)
			{
				messages[i].Send(connection);
			}

			connection.SendChunk = false;
		}

		/// <summary>
		/// Fills the number of bytes with the value.
		/// </summary>
		/// <param name="count">The number of bytes to fill.</param>
		/// <param name="value">The value to fill with.</param>
		public void Fill(int count, byte value)
		{
			for (var i = 0; i < count; i++)
			{
				Write(value);
			}
		}

		/// <summary>
		/// The number of unread bytes in the message.
		/// </summary>
		public int RemainingBytes
		{
			get
			{
				var length = stream?.Length;
				var position = stream?.Position;
				return (int)((length.HasValue & position.HasValue ? length.GetValueOrDefault() - position.GetValueOrDefault() : new long?()) ?? 0L);
			}
		}

		/// <summary>
		/// Reads a boolean value from the current stream.
		/// </summary>
		public bool ReadBoolean()
		{
			return reader.ReadBoolean();
		}

		/// <summary>
		/// Reads a byte value from the current stream.
		/// </summary>
		public byte ReadByte()
		{
			return reader.ReadByte();
		}

		/// <summary>
		/// Reads an array of bytes from the current stream.
		/// </summary>
		/// <param name="count">The number of bytes to read.</param>
		public byte[] ReadBytes(int count)
		{
			return reader.ReadBytes(count);
		}

		/// <summary>
		/// Reads a character value from the current stream.
		/// </summary>
		public char ReadChar()
		{
			return reader.ReadChar();
		}

		/// <summary>
		/// Reads a decimal value from the current stream.
		/// </summary>
		public decimal ReadDecimal()
		{
			return reader.ReadDecimal();
		}

		/// <summary>
		/// Reads a double value from the current stream.
		/// </summary>
		public double ReadDouble()
		{
			return reader.ReadDouble();
		}

		/// <summary>
		/// Reads a 16-bit integer value from the current stream.
		/// </summary>
		public short ReadInt16()
		{
			return reader.ReadInt16();
		}

		/// <summary>
		/// Reads a 32-bit integer value from the current stream.
		/// </summary>
		public int ReadInt32()
		{
			return reader.ReadInt32();
		}

		/// <summary>
		/// Reads a 64-bit integer value from the current stream.
		/// </summary>
		/// <returns></returns>
		public long ReadInt64()
		{
			return reader.ReadInt64();
		}

		/// <summary>
		/// Reads a float value from the current stream.
		/// </summary>
		/// <returns></returns>
		public float ReadSingle()
		{
			return reader.ReadSingle();
		}

		/// <summary>
		/// Reads a string value from the current stream.
		/// </summary>
		public string ReadString()
		{
			return ReadString(ReadByte());
		}

		/// <summary>
		/// Reads a string value from the current stream.
		/// </summary>
		/// <param name="length">The length of the stream.</param>
		public string ReadString(int length)
		{
			var ret = string.Empty;
			var buffer = new byte[length];
			var count = 0;

			stream.Read(buffer, 0, buffer.Length);

			if (buffer[length - 1] != 0)
			{
				count = length;
			}
			else
			{
				while (buffer[count] != 0 && count < length)
				{
					count++;
				}
			}

			if (count > 0)
			{
				ret = Encoding.ASCII.GetString(buffer, 0, count);
			}

			return ret;
		}

		/// <summary>
		/// Reads an unsigned 16-bit integer value from the current stream.
		/// </summary>
		public ushort ReadUInt16()
		{
			return reader.ReadUInt16();
		}

		/// <summary>
		/// Reads an unsigned 32-bit integer value from the current stream.
		/// </summary>
		public uint ReadUInt32()
		{
			return reader.ReadUInt32();
		}

		/// <summary>
		/// Reads an unsigned 64-bit integer value from the current stream.
		/// </summary>
		public ulong ReadUInt64()
		{
			return reader.ReadUInt64();
		}

		/// <summary>
		/// Sends the message to the connection.
		/// </summary>
		/// <param name="connection">The connection to send the message to.</param>
		public void Send(NetworkConnection connection)
		{
			connection?.SendData(ToArray(connection));
			Destroy(this);
		}

		public void Send(Character character)
		{
			Send(character?.Client);
		}


		public void Broadcast(GameObject from, params NetworkConnection[] except)
		{
			if (from == null)
			{
				return;
			}

			if (except == null)
			{
				except = new NetworkConnection[0];
			}

			Character character;
			if ((character = from as Character) != null && !except.Contains(character.Client))
			{
				Send(character);
			}
			for (var upperBound = from.VisibleObjects.GetUpperBound(); upperBound >= 0; --upperBound)
			{
				var visibleCharacter = from.VisibleCharacters[upperBound];
				if (!except.Contains(visibleCharacter.Client))
				{
					Send(visibleCharacter);
				}
					
			}
		}


		/// <summary>
		/// Returns a byte array representing the message.
		/// </summary>
		/// <returns></returns>
		public byte[] ToArray(NetworkConnection connection = null)
		{
			byte[] ret;
			var buffer = stream.ToArray();

			if (connection != null)
			{
				if (connection.Type != NetworkConnectionType.NCT_CLIENT && connection.IsEstablished)
				{
					connection.DecryptBuffer(buffer, 0, buffer.Length);
				}
			}

			if (buffer.Length <= 0xff)
			{
				ret = new byte[buffer.Length + 1];

				Buffer.BlockCopy(buffer, 0, ret, 1, buffer.Length);
				ret[0] = (byte)buffer.Length;
			}
			else
			{
				ret = new byte[buffer.Length + 3];

				Buffer.BlockCopy(buffer, 0, ret, 3, buffer.Length);
				Buffer.BlockCopy(BitConverter.GetBytes((ushort)buffer.Length), 0, ret, 1, 2);
			}

			return ret;
		}

		/// <summary>
		/// Returns a string representing the message.
		/// </summary>
		public override string ToString()
		{
			return $"Command=0x{Command:X} ({Command}), Length={stream.Length - 2}";
		}

		/// <summary>
		/// Writes a boolean value to the current stream.
		/// </summary>
		/// <param name="value">The value to write.</param>
		public void Write(bool value)
		{
			writer.Write(value);
		}

		/// <summary>
		/// Writes a byte value to the current stream.
		/// </summary>
		/// <param name="value">The value to write.</param>
		public void Write(byte value)
		{
			writer.Write(value);
		}

		/// <summary>
		/// Writes a signed byte value to the current stream.
		/// </summary>
		/// <param name="value">The value to write.</param>
		public void Write(sbyte value)
		{
			writer.Write(value);
		}

		/// <summary>
		/// Writes a byte array to the current stream.
		/// </summary>
		/// <param name="value">The value to write.</param>
		public void Write(byte[] value)
		{
			writer.Write(value);
		}

		/// <summary>
		/// Writes a 16-bit integer value to the current stream.
		/// </summary>
		/// <param name="value">The value to write.</param>
		public void Write(short value)
		{
			writer.Write(value);
		}

		/// <summary>
		/// Writes a 32-bit integer value to the current stream.
		/// </summary>
		/// <param name="value">The value to write.</param>
		public void Write(int value)
		{
			writer.Write(value);
		}

		/// <summary>
		/// Writes a 64-bit integer value to the current stream.
		/// </summary>
		/// <param name="value">The value to write.</param>
		public void Write(long value)
		{
			writer.Write(value);
		}

		/// <summary>
		/// Writes an unsigned 16-bit integer value to the current stream.
		/// </summary>
		/// <param name="value">The value to write.</param>
		public void Write(ushort value)
		{
			writer.Write(value);
		}

		/// <summary>
		/// Writes an unsigned 32-bit integer value to the current stream.
		/// </summary>
		/// <param name="value">The value to write.</param>
		public void Write(uint value)
		{
			writer.Write(value);
		}

		/// <summary>
		/// Writes an unsigned 64-bit integer value to the current stream.
		/// </summary>
		/// <param name="value">The value to write.</param>
		public void Write(ulong value)
		{
			writer.Write(value);
		}

		/// <summary>
		/// Writes a double value to the current stream.
		/// </summary>
		/// <param name="value">The value to write.</param>
		public void Write(double value)
		{
			writer.Write(value);
		}

		/// <summary>
		/// Writes a decimal value to the current stream.
		/// </summary>
		/// <param name="value">The value to write.</param>
		public void Write(decimal value)
		{
			writer.Write(value);
		}

		/// <summary>
		/// Writes a float value to the current stream.
		/// </summary>
		/// <param name="value">The value to write.</param>
		public void Write(float value)
		{
			writer.Write(value);
		}

		/// <summary>
		/// Writes a string value to the current stream.
		/// </summary>
		/// <param name="value">The value to write.</param>
		public void Write(string value)
		{
			Write(value, value.Length);
		}

		/// <summary>
		/// Writes a string value to the current stream.
		/// </summary>
		/// <param name="value">The value to write.</param>
		/// <param name="length">The length of the string.</param>
		public void Write(string value, int length)
		{
			var buffer = Encoding.ASCII.GetBytes(value);

			Write(buffer);

			for (var i = 0; i < length - buffer.Length; i++)
			{
				Write((byte)0);
			}
		}

		/// <summary>
		/// Destroys the <see cref="NetworkMessage"/> instance.
		/// </summary>
		protected override void Destroy()
		{
			// Reader and writer are not always initialized, so we need to check
			// for null before attempting to close them.
			reader?.Close();
			writer?.Close();
			stream.Close();
		}
	}
}
