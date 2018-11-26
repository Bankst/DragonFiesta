using System.Collections.Generic;

namespace DFEngine
{
	/// <summary>
	/// Class that extends the use of the List class.
	/// </summary>
	public static class DictionaryExtensions
	{
		/// <summary>
		/// The object to lock for list access.
		/// </summary>
		private static readonly object LockObject = new object();

		/// <summary>
		/// Adds an object to the list in a thread-safe, exception-safe
		/// manner.
		/// </summary>
		/// <param name="source">The source list.</param>
		/// <param name="value">The value to add.</param>
		public static void AddSafe<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key, TValue value)
		{
			if (key == null || value == null)
			{
				return;
			}

			lock (LockObject)
			{
				if (source.ContainsKey(key))
				{
					source[key] = value;
					return;
				}

				source.Add(key, value);
			}
		}

		/// <summary>
		/// Tries to get the object at the specified key. Returns default if it does not exist,
		/// instead of throwing an exception.
		/// </summary>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="source">The dictionary being searched.</param>
		/// <param name="key">The key of the value.</param>
		public static TValue GetSafe<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key)
		{
			return source.ContainsKey(key) ? source[key] : default(TValue);
		}
	}
}
