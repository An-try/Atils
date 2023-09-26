using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Atils.Runtime.Extensions
{
	public static class EnumExtensions
	{
		public static T Random<T>(this T source) where T : Enum
		{
			Type sourceType = source.GetType();
			int currentIndex = (int)(object)source;

			if (!Enum.IsDefined(sourceType, currentIndex))
			{
				throw new InvalidEnumArgumentException(nameof(source), currentIndex, sourceType);
			}

			return Enum.GetValues(sourceType).GetRandom<T>();
		}

		public static T Next<T>(this T source) where T : Enum
		{
			Type sourceType = source.GetType();
			int currentIndex = (int)(object)source;

			if (!Enum.IsDefined(sourceType, currentIndex))
			{
				throw new InvalidEnumArgumentException(nameof(source), currentIndex, sourceType);
			}

			int enumCount = Enum.GetNames(sourceType).Length;
			int nextIndex = currentIndex + 1;

			if (nextIndex >= enumCount)
			{
				nextIndex = 0;
			}

			return (T)Enum.Parse(sourceType, nextIndex.ToString());
		}

		public static T Previous<T>(this T source) where T : Enum
		{
			Type sourceType = source.GetType();
			int currentIndex = (int)(object)source;

			if (!Enum.IsDefined(sourceType, currentIndex))
			{
				throw new InvalidEnumArgumentException(nameof(source), currentIndex, sourceType);
			}

			int enumCount = Enum.GetNames(sourceType).Length;
			int previousIndex = currentIndex - 1;

			if (previousIndex < 0)
			{
				previousIndex = enumCount - 1;
			}

			return (T)Enum.Parse(sourceType, previousIndex.ToString());
		}

		public static int TryGetIntFromName<T>(this T source) where T : Enum
		{
			Type sourceType = source.GetType();
			int currentIndex = (int)(object)source;

			if (!Enum.IsDefined(sourceType, currentIndex))
			{
				throw new InvalidEnumArgumentException(nameof(source), currentIndex, sourceType);
			}

			string enumName = source.ToString();

			// Use regular expression to match and keep only numeric characters
			string numericPart = Regex.Replace(enumName, "[^0-9]", "");

			if (!int.TryParse(numericPart, out int numericValue))
			{
				Debug.LogError($"{numericPart} is not a number.");
				return 0;
			}

			return numericValue;
		}
	}
}
