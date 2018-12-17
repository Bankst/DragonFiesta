using System;
using System.Collections.Generic;
using System.Linq;

namespace DFEngine
{
	/// <summary>
	/// Class that extends the use of the List class.
	/// </summary>
	public static class ListExtensions
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
		public static void AddSafe<T>(this List<T> source, T value)
		{
			if (value == null)
			{
				return;
			}

			lock (LockObject)
			{
				if (!source.Contains(value))
				{
					source.Add(value);
				}
			}
		}

		/// <summary>
		/// Copies the elements from the source list to the output list.
		/// </summary>
		/// <param name="source">The source list.</param>
		/// <param name="output">The output list.</param>
		public static void Copy<T>(this List<T> source, out List<T> output)
		{
			lock (LockObject)
			{
				output = new List<T>(source);
			}
		}

		/// <summary>
		/// Returns a new list filtered using the predicate provided.
		/// </summary>
		public static List<T> Filter<T>(this List<T> source, Func<T, bool> predicate)
		{
			lock (LockObject)
			{
				return new List<T>(source.Where(predicate));
			}
		}

		public static int GetUpperBound<T>(this List<T> source)
		{
			if (source.Count <= 0)
				return -1;
			return source.Count - 1;
		}

		/// <summary>
		/// Performs an action on the filtered list of items in the list.
		/// </summary>
		/// <param name="source">The source list.</param>
		/// <param name="predicate">The filter specifications.</param>
		/// <param name="action">The action to perform on the filtered items.</param>
		public static void FilteredAction<T>(this List<T> source, Func<T, bool> predicate, Action<T> action)
		{
			lock (LockObject)
			{
				source.Filter(predicate).ForEach(action);
			}
		}

		/// <summary>
		/// Returns the first matching element in the list.
		/// </summary>
		/// <param name="source">The source list.</param>
		/// <param name="predicate">The filter specifications.</param>
		public static T First<T>(this List<T> source, Func<T, bool> predicate)
		{
			lock (LockObject)
			{
				var list = source.Filter(predicate);
				return list.Count <= 0 ? default(T) : list[0];
			}
		}


		/// <summary>
		/// Runs a for loop on list in a thread-safe manner.
		/// </summary>
		/// <param name="source">The source list.</param>
		/// <param name="action">The action to perform on the item.</param>
		public static void For<T>(this List<T> source, Action<T> action)
		{
			lock (LockObject)
			{
				for (var i = 0; i < source.Count; i++)
				{
					action(source[i]);
				}
			}
		}

		/// <summary>
		/// Runs a for loop on list, backwards, in a thread-safe manner.
		/// </summary>
		/// <param name="source">The source list.</param>
		/// <param name="action">The action to perform on the item.</param>
		public static void ForBackwards<T>(this List<T> source, Action<T> action)
		{
			lock (LockObject)
			{
				for (var i = source.Count - 1; i >= 0; i--)
				{
					action(source[i]);
				}
			}
		}

		/// <summary>
		/// Removes an object from a list in a thread-safe, exception-safe
		/// manner.
		/// </summary>
		/// <param name="source">The source list.</param>
		/// <param name="value">The value to remove.</param>
		public static void RemoveSafe<T>(this List<T> source, T value)
		{
			if (value == null)
			{
				return;
			}

			lock (LockObject)
			{
				if (source.Contains(value))
				{
					source.Remove(value);
				}
			}
		}
	}
}
