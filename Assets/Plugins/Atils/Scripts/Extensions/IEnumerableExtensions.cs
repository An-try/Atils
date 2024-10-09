using System;
using System.Collections.Generic;
using System.Linq;

namespace Atils.Runtime.Extensions
{
	public static class IEnumerableExtensions
	{
		public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
		{
			return source == null || source.Count() <= 0;
		}

		#region Getting random items

		/// <summary>
		/// Get random item type of T from the source.
		/// </summary>
		/// <typeparam name="T">Specifies the item type</typeparam>
		/// <param name="source">The source object from which to take a random item</param>
		/// <returns>Random item, if found. Otherwise, the default value for type T</returns>
		public static T GetRandom<T>(this IEnumerable<T> source)
		{
			return GetRandom(source, out int index);
		}

		/// <summary>
		/// Get random item type of T from the source.
		/// </summary>
		/// <typeparam name="T">Specifies the item type</typeparam>
		/// <param name="source">The source object from which to take a random item</param>
		/// <param name="index">The index of the randomly selected item</param>
		/// <returns>Random item, if found. Otherwise, the default value for type T</returns>
		public static T GetRandom<T>(this IEnumerable<T> source, out int index)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));

			index = -1;

			if (source.Count() == 0)
			{
				return default;
			}

			Random random = new Random();
			index = random.Next(0, source.Count());
			return source.ElementAt(index);
		}

		/// <summary>
		/// Get random item type of T that matches the predicate from the source.
		/// </summary>
		/// <typeparam name="T">Specifies the item type</typeparam>
		/// <param name="source">The source object from which to take a random item</param>
		/// <param name="match">The method that defines a set of criteria and determines whether the specified object meets those criteria</param>
		/// <returns>Random item, if found. Otherwise, the default value for type T</returns>
		public static T GetRandom<T>(this IEnumerable<T> source, Predicate<T> match)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (match == null) throw new ArgumentNullException(nameof(match));

			Func<T, bool> func = new Func<T, bool>(match);
			return source.Where(func).GetRandom();
		}

		/// <summary>
		/// Get random item type of T that matches the predicate from the source.
		/// </summary>
		/// <typeparam name="T">Specifies the item type</typeparam>
		/// <param name="source">The source object from which to take a random item</param>
		/// <param name="match">The method that defines a set of criteria and determines whether the specified object meets those criteria</param>
		/// <param name="index">The index of the randomly selected item</param>
		/// <returns>Random item, if found. Otherwise, the default value for type T</returns>
		public static T GetRandom<T>(this IEnumerable<T> source, Predicate<T> match, out int index)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (match == null) throw new ArgumentNullException(nameof(match));

			index = -1;

			Tuple<T, int> tuple = source.FindAllWithIndexes(match).GetRandom();

			if (tuple == null)
			{
				return default;
			}

			index = tuple.Item2;
			return tuple.Item1;
		}

		#endregion

		#region Getting index

		/// <summary>
		/// Searches for an element that matches the conditions defined by the specified predicate,
		/// and returns the zero-based index of the first occurrence within the entire <see cref="IEnumerable{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
		/// <param name="source">The <see cref="IEnumerable{T}"/> to search.</param>
		/// <param name="match">The predicate that defines the conditions of the element to search for.</param>
		/// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref name="match"/>, 
		/// if found; otherwise, -1.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static int GetIndex<T>(this IEnumerable<T> source, Func<T, bool> match)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (match == null) throw new ArgumentNullException(nameof(match));

			int index = 0;

			foreach (var element in source)
			{
				if (match(element))
				{
					return index;
				}
				index++;
			}

			return -1;
		}

		#endregion

		#region Finding items

		/// <summary>
		/// Get all items type of T that matches the predicate from the source.
		/// </summary>
		/// <typeparam name="T">Specifies the item type</typeparam>
		/// <param name="source">The source object in which to find items</param>
		/// <param name="match">The method that defines a set of criteria and determines whether the specified object meets those criteria</param>
		/// <returns>List of tuples with elements and their indexes</returns>
		public static List<Tuple<T, int>> FindAllWithIndexes<T>(this IEnumerable<T> source, Predicate<T> match)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (match == null) throw new ArgumentNullException(nameof(match));

			List<Tuple<T, int>> listTuples = new List<Tuple<T, int>>();

			for (int i = 0; i < source.Count(); i++)
			{
				T element = source.ElementAt(i);
				if (match(element))
				{
					listTuples.Add(new Tuple<T, int>(element, i));
				}
			}

			return listTuples;
		}

		#endregion
	}
}
