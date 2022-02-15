namespace Atils.Runtime.Utils
{
	public static class Matha
	{
		public static float Normalize(float value, float min, float max)
		{
			return (value - min) / (max - min);
		}
	}
}
