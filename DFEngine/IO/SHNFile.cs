using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using DFEngine.Logging;

namespace DFEngine.IO
{
	/// <summary>
	/// Class to load and access SHN files.
	/// </summary>
	public class SHNFile : IDisposable
	{
		private static bool _useParallelism = false;

		/// <summary>
		/// Objects representing the contents of loaded SHN files.
		/// </summary>
		private static readonly Dictionary<string, dynamic> FileObjects = new Dictionary<string, dynamic>();

		/// <summary>
		/// Tries to get the objects for a file.
		/// </summary>
		/// <typeparam name="T">The type of objects.</typeparam>
		/// <param name="name">The name of the file.</param>
		/// <param name="objects">The objects.</param>
		/// <returns>The file's objects.</returns>
		public static bool TryGetObjects<T>(string name, out ObjectCollection<T> objects)
		{
			objects = new ObjectCollection<T>();

			if (!FileObjects.ContainsKey(name))
			{
				return false;
			}

			objects = (ObjectCollection<T>)FileObjects[name];
			Count++;
			return true;
		}

		/// <summary>
		/// The path of the file.
		/// </summary>
		private readonly string _fileName;

		/// <summary>
		/// The underlying DataTable, which holds all of the file's
		/// data.
		/// </summary>
		private readonly DataTable _table;

		/// <summary>
		/// The number of fully loaded SHN files.
		/// </summary>
		public static int Count = 0;

		/// <summary>
		/// Creates a new instance of the <see cref="SHNFile"/> class.
		/// </summary>
		private SHNFile(string path)
		{
			_fileName = path;
			_table = new DataTable(Path.GetFileName(path));

			PopulateTable();
		}

		/// <summary>
		/// Loads all, or specified SHN files from a folder, if they are defined.
		/// </summary>
		/// <param name="directory">The folder to load the files from.</param>
		/// <param name="files">Specific files to load. If set to null, all files in the folder will be loaded.</param>
		public static void LoadFromFolder(string directory, params string[] files)
		{
			if (!Directory.Exists(directory))
			{
				EngineLog.Write(EngineLogLevel.Exception, $"The directory '{directory}' could not be found.");
				return;
			}

			// Load file definitions first.
			var fileDefinitions = System.Reflection.Assembly.GetExecutingAssembly().GetTypes().Where(type => type.GetCustomAttributes(typeof(Definition), true).Length > 0).ToArray();

			if (fileDefinitions.Length <= 0)
			{
				EngineLog.Write(EngineLogLevel.Exception, "No file definitions were found.");
				return;
			}

			// Converts all file names in the array to a full file path,
			// if the length of the array is greater than 0.
			for (var i = 0; i < files.Length; i++)
			{
				files[i] = Path.Combine(directory, files[i]);
			}

			var fileNames = files.Length > 0 ? files : Directory.GetFiles(directory, "*.shn");

			if (_useParallelism)
			{
				Parallel.ForEach(fileNames, fileName =>
				{
					var shortName = Path.GetFileNameWithoutExtension(fileName);
					var definition = fileDefinitions.FirstOrDefault(def => def.Name == shortName);

					if (definition == null) return;
					var genericClass = typeof(ObjectCollection<>);
					var collection = genericClass.MakeGenericType(definition);
					var created = (dynamic)Activator.CreateInstance(collection);

					var mutex = new Mutex(false, shortName);

					mutex.WaitOne();

					FileObjects.Remove(shortName);
					FileObjects.Add(shortName, created);

					mutex.ReleaseMutex();

					using (var file = new SHNFile(fileName))
					using (var reader = new DataTableReader(file._table))
					{
						while (reader.Read())
						{
							created.Add(reader);
						}
					}
				});
			}
			else
			{
				foreach (var fileName in fileNames)
				{
					var shortName = Path.GetFileNameWithoutExtension(fileName);
					var definition = fileDefinitions.FirstOrDefault(def => def.Name == shortName);

					if (definition == null) continue;
					var genericClass = typeof(ObjectCollection<>);
					var collection = genericClass.MakeGenericType(definition);
					var created = (dynamic)Activator.CreateInstance(collection);

//					var mutex = new Mutex(false, shortName);

//					mutex.WaitOne();

					FileObjects.Remove(shortName);
					FileObjects.Add(shortName, created);

//					mutex.ReleaseMutex();

					using (var file = new SHNFile(fileName))
					using (var reader = new DataTableReader(file._table))
					{
						while (reader.Read())
						{
							created.Add(reader);
						}
					}
				}
			}

			
		}

		/// <summary>
		/// Decrypts an array of bytes.
		/// </summary>
		/// <param name="input">The byte array being decrypted.</param>
		/// <param name="length">The length of the byte array.</param>
		/// <param name="output">The result of the decryption.</param>
		private static void DecryptBuffer(byte[] input, int length, out byte[] output)
		{
			output = input;

			var xor = (byte)length;

			for (var i = length - 1; i >= 0; i--)
			{
				output[i] ^= xor;

				var nextXor = (byte)i;

				nextXor &= 0xF;
				nextXor += 0x55;
				nextXor ^= (byte)(i * 0xB);
				nextXor ^= xor;
				nextXor ^= 0xAA;

				xor = nextXor;
			}
		}

		/// <summary>
		/// Gets the data type for a column.
		/// </summary>
		/// <param name="type">The byte value in the file.</param>
		/// <returns>The column's data type.</returns>
		private static Type GetColumnDataType(uint type)
		{
			switch (type)
			{
				case 1:
				case 12:
				case 16:
					return typeof(byte);
				case 2:
					return typeof(ushort);
				case 3:
				case 11:
				case 18:
				case 27:
					return typeof(uint);
				case 5:
					return typeof(float);
				case 13:
				case 21:
					return typeof(short);
				case 20:
					return typeof(sbyte);
				case 22:
					return typeof(int);
				case 9:
				case 24:
				case 26:
					return typeof(string);
				default:
					return typeof(object);
			}
		}

		/// <summary>
		/// Disposes the file handle.
		/// </summary>
		public void Dispose()
		{
			_table.Dispose();
		}

		/// <summary>
		/// Populates the SHNFile's table with the file's data.
		/// </summary>
		private void PopulateTable()
		{
			if (!File.Exists(_fileName))
			{
				EngineLog.Write(EngineLogLevel.Exception, $"The file '{_fileName}' could not be found.");
				return;
			}

			byte[] buffer;

			using (var file = File.OpenRead(_fileName))
			using (var reader = new BinaryReader(file))
			{
				reader.ReadBytes(32); // Crypt header, unused.

				var length = reader.ReadInt32();
				var bytes = reader.ReadBytes(length - 36);

				DecryptBuffer(bytes, bytes.Length, out buffer);
			}

			using (var stream = new MemoryStream(buffer))
			using (var reader = new BinaryReader(stream))
			{
				reader.ReadBytes(4); // Header, unused.

				var rowCount = reader.ReadUInt32();
				var defaultRowLength = reader.ReadUInt32(); // Default row length, unused.
				var columnCount = reader.ReadUInt32();

				var columnTypes = new uint[columnCount];
				var columnLengths = new int[columnCount];

				var unkColumnCount = 0;

				for (var i = 0; i < columnCount; i++)
				{
					var name = reader.ReadString(48);
					var type = reader.ReadUInt32();
					var length = reader.ReadInt32();

					var column = new DataColumn(name, GetColumnDataType(type));

					if (name.Trim().Length < 2)
					{
						column.ColumnName = $"UnkCol{unkColumnCount++}";
					}

					columnTypes[i] = type;
					columnLengths[i] = length;

					_table.Columns.Add(column);
				}

				var row = new object[columnCount];

				for (var i = 0; i < rowCount; i++)
				{
					var rowLength = reader.ReadUInt16();

					for (var j = 0; j < columnCount; j++)
					{
						switch (columnTypes[j])
						{
							case 1:
							case 12:
							case 16:
								row[j] = reader.ReadByte();
								break;
							case 2:
								row[j] = reader.ReadUInt16();
								break;
							case 3:
							case 11:
							case 18:
							case 27:
								row[j] = reader.ReadUInt32();
								break;
							case 5:
								row[j] = reader.ReadSingle();
								break;
							case 9:
							case 24:
								row[j] = reader.ReadString(columnLengths[j]);
								break;
							case 13:
							case 21:
								row[j] = reader.ReadInt16();
								break;
							case 20:
								row[j] = reader.ReadSByte();
								break;
							case 22:
								row[j] = reader.ReadInt32();
								break;
							case 26:
								row[j] = reader.ReadString(rowLength - (int)defaultRowLength + 1);
								break;
							case 29:
								var bytes = reader.ReadBytes(columnLengths[j]);

								var val1 = BitConverter.ToUInt32(bytes, 0);
								var val2 = BitConverter.ToUInt32(bytes, 4);

								row[j] = new Tuple<uint, uint>(val1, val2);
								break;
						}
					}

					_table.Rows.Add(row);
				}
			}
		}
	}
}
