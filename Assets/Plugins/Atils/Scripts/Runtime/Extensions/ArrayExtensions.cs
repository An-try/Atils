using System;
using System.Collections.Generic;

namespace Atils.Runtime.Extensions
{
	public static class ArrayExtensions
	{
		#region Finding items

		/// <summary>
		/// Find the item that matches the predicate.
		/// </summary>
		/// <typeparam name="T">Specifies the item type</typeparam>
		/// <param name="source">The source object from which to find an item</param>
		/// <param name="match">The method that defines a set of criteria and determines whether the specified object meets those criteria</param>
		/// <returns>The first item that matches the conditions defined by the specified predicate, if found. Otherwise, the default value for type T</returns>
		public static T Find<T>(this T[] source, Predicate<T> match)
		{
			return Array.Find(source, match);
		}

		#endregion

		#region Getting random items

		/// <summary>
		/// Get random item type of T from the source.
		/// </summary>
		/// <typeparam name="T">Specifies the item type</typeparam>
		/// <param name="source">The source object from which to take a random item</param>
		/// <returns>Random item, if found. Otherwise, the default value for type T</returns>
		public static T GetRandom<T>(this T[] source)
		{
			return (source as IEnumerable<T>).GetRandom();
		}

		/// <summary>
		/// Get random item type of T from the source.
		/// </summary>
		/// <typeparam name="T">Specifies the item type</typeparam>
		/// <param name="source">The source object from which to take a random item</param>
		/// <param name="index">The index of the randomly selected item</param>
		/// <returns>Random item, if found. Otherwise, the default value for type T</returns>
		public static T GetRandom<T>(this T[] source, out int index)
		{
			return (source as IEnumerable<T>).GetRandom(out index);
		}

		/// <summary>
		/// Get random item type of T that matches the predicate from the source.
		/// </summary>
		/// <typeparam name="T">Specifies the item type</typeparam>
		/// <param name="source">The source object from which to take a random item</param>
		/// <param name="match">The method that defines a set of criteria and determines whether the specified object meets those criteria</param>
		/// <returns>Random item, if found. Otherwise, the default value for type T</returns>
		public static T GetRandom<T>(this T[] source, Predicate<T> match)
		{
			return (source as IEnumerable<T>).GetRandom(match);
		}

		/// <summary>
		/// Get random item type of T from the source.
		/// </summary>
		/// <typeparam name="T">Specifies the item type</typeparam>
		/// <param name="source">The source object from which to take a random item</param>
		/// <returns>Random item, if found. Otherwise, the default value for type T</returns>
		public static T GetRandom<T>(this Array source)
		{
			return (source as IEnumerable<T>).GetRandom();
		}

		/// <summary>
		/// Get random item type of T from the source.
		/// </summary>
		/// <typeparam name="T">Specifies the item type</typeparam>
		/// <param name="source">The source object from which to take a random item</param>
		/// <param name="index">The index of the randomly selected item</param>
		/// <returns>Random item, if found. Otherwise, the default value for type T</returns>
		public static T GetRandom<T>(this Array source, out int index)
		{
			return (source as IEnumerable<T>).GetRandom(out index);
		}

		/// <summary>
		/// Get random item type of T that matches the predicate from the source.
		/// </summary>
		/// <typeparam name="T">Specifies the item type</typeparam>
		/// <param name="source">The source object from which to take a random item</param>
		/// <param name="match">The method that defines a set of criteria and determines whether the specified object meets those criteria</param>
		/// <returns>Random item, if found. Otherwise, the default value for type T</returns>
		public static T GetRandom<T>(this Array source, Predicate<T> match)
		{
			return (source as IEnumerable<T>).GetRandom(match);
		}

		#endregion

		#region Enumeration of items

		/// <summary>
		/// Enumerate items in the source array and perform an action on each of them.
		/// </summary>
		/// <typeparam name="T">Specifies the item type</typeparam>
		/// <param name="source">The source object in which the items are enumerated</param>
		/// <param name="action">The action to perform on each item of source array</param>
		public static void ForEach<T>(this T[] source, Action<T> action)
		{
			Array.ForEach(source, action);
		}

		#endregion

		#region Shuffling

		public static T[] Shuffle<T>(this T[] source)
		{
			int randomIndex;
			for (int i = 0; i < source.Length - 1; i++)
			{
				randomIndex = UnityEngine.Random.Range(i + 1, source.Length);
				T value = source[i];
				source[i] = source[randomIndex];
				source[randomIndex] = value;
			}

			return source;
		}

		#endregion
	}
}
