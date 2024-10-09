using System;

namespace Atils.Runtime.Utils
{
	public static class DateTimeExtensions
    {
		public static DateTime StartOfWeek(this DateTime source, DayOfWeek startOfWeek = DayOfWeek.Monday)
		{
			int diff = (7 + (source.DayOfWeek - startOfWeek)) % 7;
			return source.AddDays(-1 * diff).Date;
		}

		public static DateTime EndOfWeek(this DateTime source, DayOfWeek startOfWeek = DayOfWeek.Monday)
		{
			int diff = (7 + (source.DayOfWeek - startOfWeek)) % 7;
			return source.AddDays(-1 * diff + 6).Date.AddDays(1).AddTicks(-1);
		}
	}
}
