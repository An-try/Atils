using System;
using System.Collections.Generic;
using System.Linq;

namespace Atils.Runtime.Extensions
{
	public static class QueueExtensions
	{
		// TODO summary, tests
		public static bool Remove<T>(this Queue<T> source, T item)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (item == null) throw new ArgumentNullException(nameof(item));

			if (source.Contains(item))
			{
				source = new Queue<T>(source.Where(x => !x.Equals(item)));
				return true;
			}

			return false;
		}

		public static Queue<T> Copy<T>(this Queue<T> source)
		{
			return new Queue<T>(source.ToList());
		}
	}
}
