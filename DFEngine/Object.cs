using System;
using System.Collections.Generic;

namespace DFEngine
{
	/// <summary>
	/// Class for objects that can be referenced.
	/// </summary>
	public class Object : IDisposable
	{
		/// <summary>
		/// The name of the object.
		/// </summary>
		public string Name => GetName();

		/// <summary>
		/// Keeps track of the number of all object types.
		/// </summary>
		private static readonly Dictionary<Type, long> TypeCounts = new Dictionary<Type, long>();

		/// <summary>
		/// An object to lock for TypeCounts access.
		/// </summary>
		private static readonly object TypeCountsLock = new object();

		/// <summary>
		/// Creates a new instance of the <see cref="Object"/> class.
		/// </summary>
		public Object()
		{
			var type = GetType();

			EnsureTypeExists(type);

			lock (TypeCountsLock)
			{
				TypeCounts[type]++;
			}
		}

		/// <summary>
		/// This method is called whenever an <see cref="Object"/> is deconstructed.
		/// </summary>
		~Object()
		{
			var type = GetType();

			EnsureTypeExists(type);

			lock (TypeCountsLock)
			{
				TypeCounts[type]--;
			}
		}

		/// <summary>
		/// Destroys the object.
		/// </summary>
		/// <param name="obj">The object to destroy.</param>
		public static void Destroy(Object obj)
		{
			obj.Dispose();
		}

		/// <summary>
		/// Determines if the object exists.
		/// </summary>
		/// <param name="obj">The object to check.</param>
		public static implicit operator bool(Object obj)
		{
			return obj != null;
		}

		/// <summary>
		/// Ensures that the TypeCounts dictionary contains the type.
		/// If the type is not found, it is added to the dictionary with a count of 0.
		/// </summary>
		/// <param name="type">The type to check for.</param>
		private static void EnsureTypeExists(Type type)
		{
			lock (TypeCountsLock)
			{
				if (!TypeCounts.ContainsKey(type))
				{
					TypeCounts.Add(type, 0);
				}
			}
		}

		/// <summary>
		/// Use the static method Object.Destroy() instead.
		/// </summary>
		public void Dispose()
		{
			Destroy();
		}

		/// <summary>
		/// Gets the name of the object instance. The name is based on how
		/// many instances of the object there are.
		/// </summary>
		/// <returns>The name of the object.</returns>
		public string GetName()
		{
			var type = GetType();

			EnsureTypeExists(type);

			lock (TypeCountsLock)
			{
				return $"{type.Name} {TypeCounts[type]}";
			}
		}

		/// <summary>
		/// Returns the name of the object.
		/// </summary>
		/// <returns>The name of the object.</returns>
		public override string ToString()
		{
			return Name;
		}

		/// <summary>
		/// This method does nothing unless it is overidded by the top-level
		/// implementation of the <see cref="Object"/> class.
		/// </summary>
		protected virtual void Destroy()
		{

		}
	}
}
