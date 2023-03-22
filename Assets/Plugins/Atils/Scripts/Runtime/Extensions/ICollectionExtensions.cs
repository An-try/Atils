using System;
using System.Collections.Generic;

namespace Atils.Runtime.Extensions
{
	public static class ICollectionExtensions
    {
		public static ICollection<T> Shuffle<T>(this ICollection<T> list)
		{
			List<T> new_list = new List<T>(list);
			Random rnd = new Random();

			int n = new_list.Count;
			while (n > 1)
			{
				n--;
				int k = rnd.Next(n + 1);
				T value = new_list[k];
				new_list[k] = new_list[n];
				new_list[n] = value;
			}

			return new_list;
		}
	}
}
