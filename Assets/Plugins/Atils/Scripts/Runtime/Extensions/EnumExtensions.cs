using System;
using System.ComponentModel;

namespace Atils.Runtime.Extensions
{
	public static class EnumExtensions
	{
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
	}
}
