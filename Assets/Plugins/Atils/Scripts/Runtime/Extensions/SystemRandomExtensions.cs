using Random = System.Random;

namespace Atils.Runtime.Extensions
{
	public static class SystemRandomExtensions
	{
		public static float Next(this Random source, float minValue, float maxValue)
		{
			return (float)source.Next((double)minValue, (double)maxValue);
		}

		public static double Next(this Random source, double minValue, double maxValue)
		{
			double range = maxValue - minValue;
			double sample = source.NextDouble();
			double scaled = (sample * range) + minValue;

			return scaled;
		}
	}
}
