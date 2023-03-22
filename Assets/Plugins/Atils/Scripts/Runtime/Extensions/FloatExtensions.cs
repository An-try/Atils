namespace BrainBasedVR.Runtime.Utils
{
	public static class FloatExtensions
    {
		public static float CentimetersToMeters(this float source)
		{
			return source / 100f;
		}
	}
}
