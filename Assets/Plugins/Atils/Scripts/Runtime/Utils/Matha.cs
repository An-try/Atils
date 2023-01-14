namespace Atils.Runtime.Utils
{
	public static class Matha
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value">Value in range between min and max</param>
		/// <param name="min">Min value</param>
		/// <param name="max">Max value</param>
		/// <returns>Normalized value from 0 to 1</returns>
		public static float NormalizeTo01(float value, float min, float max)
		{
			return (value - min) / (max - min);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value">Value between 0 and 1</param>
		/// <param name="min">Min value</param>
		/// <param name="max">Max value</param>
		/// <returns>Value between min and max</returns>
		public static float NormalizeFrom01(float value, float min, float max)
		{
			return min + value * (max - min);
		}
	}
}
