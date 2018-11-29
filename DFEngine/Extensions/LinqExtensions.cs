using System;
using System.Collections;
using System.Collections.Generic;

namespace DFEngine
{
	public static class LinqExtensions
	{
		public static IEnumerable<T> SkipExceptions<T>(this IEnumerable<T> values)
		{
			using (var enumerator = values.GetEnumerator())
			{
				var next = true;
				while (next)
				{
					try
					{
						next = enumerator.MoveNext();
					}
					catch
					{
						continue;
					}

					if (next) yield return enumerator.Current;
				}
			}
		}
	}
}
