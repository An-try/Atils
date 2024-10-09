using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Atils.Runtime.Utils
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

		/// <summary>
		/// Safely truncates the string to the specified maximum length.
		/// If the string is shorter than the maximum length, it returns the original string.
		/// </summary>
		/// <param name="str">The input string to truncate.</par</param>
		/// <param name="maxLength">The maximum length of the resulting string.</param>
		/// <returns>The truncated string if the original string length exceeds the maximum length,
		/// or the original string if it is shorter. If the input string is null, it returns null.</returns>
		public static string SubstringSafely(this string str, int maxLength)
		{
			if (string.IsNullOrEmpty(str))
			{
				return str;
			}

			return str.Length > maxLength ? str.Substring(0, maxLength) : str;
		}

		/// <summary>
		/// Checks whether the string is null, empty, or consists only of white-space characters.
		/// </summary>
		/// <param name="str">The input string to check.</param>
		/// <returns>true if the string is null, empty, or contains only white-space characters, otherwise, false.</returns>
		public static bool IsNullOrEmptyOrWhiteSpace(this string str)
		{
			return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
		}
	}
}
