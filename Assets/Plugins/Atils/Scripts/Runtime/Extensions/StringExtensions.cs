using System;

namespace BrainBasedVR.Runtime.Utils
{
	public static class StringExtensions
	{
		public static float CentimetersToMeters(this string source)
		{
			// TODO make null checkings

			if (!int.TryParse(source, out int centimeters))
			{
				throw new Exception(); // TODO make correct exception
			}

			return (float)centimeters / 100f;
		}

		public static float CentimetersToMeters(this float source) // this is not a string extension
		{
			// TODO make null checkings

			return source / 100f;
		}
	}
}
