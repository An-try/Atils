using System;
using System.Linq;

namespace BrainBasedVR.Runtime.Utils
{
	public static class StringExtensions
	{
		public static float CentimetersToMeters(this string source)
		{
			if (!int.TryParse(source, out int centimeters))
			{
				throw new Exception(); // TODO make correct exception
			}

			return (float)centimeters / 100f;
		}

		public static bool Contains(this string source, string toCheck, StringComparison comp)
		{
			return source?.IndexOf(toCheck, comp) >= 0;
		}

		public static bool IsAll(this string source, char check)
		{
			return string.Concat(source.Distinct()).Equals(check);
		}
	}
}
