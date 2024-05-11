using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

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

		/// <summary>
		/// Converts "_someName" to "Some Name", etc.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static string ToHumanReadable(this string name)
		{
			// Remove leading underscores (for snake_case)
			name = name.TrimStart('_');

			// Insert a space before each uppercase letter, except at the start
			name = Regex.Replace(name, "(?<!^)([A-Z])", " $1");

			// Capitalize each word
			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());
		}
	}
}
