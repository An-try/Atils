using System.Collections;

namespace Atils.Runtime.Extensions
{
	public static class IListExtensions
	{
		public static void AddRange(this IList source, IEnumerable items)
		{
			foreach (object item in items)
			{
				source.Add(item);
			}
		}
	}
}
