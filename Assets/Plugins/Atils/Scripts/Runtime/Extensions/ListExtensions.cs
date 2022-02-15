using System;
using System.Collections.Generic;

namespace Atils.Runtime.Extensions
{
	public static class ListExtensions
	{
		#region Adding items

		/// <summary>
		/// Add a new item type of T to the source if there is no one already. New item cannot be null. If the item already exists, nothing will happen.
		/// </summary>
		/// <typeparam name="T">Specifies the item type</typeparam>
		/// <param name="source">The source object to add a new item</param>
		/// <param name="item">Item to add. Cannot be null</param>
		/// <returns>True if item is successfully added. Otherwise, false</returns>
		public static bool AddIfNotExist<T>(this List<T> source, T item)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (item == null) throw new ArgumentNullException(nameof(item));

			if (!source.Contains(item))
			{
				source.Add(item);
				return true;
			}

			return false;
		}

		#endregion
	}
}
