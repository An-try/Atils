namespace Atils.Runtime.Timers
{
	public static class TimeUtilities
	{
		/// <summary>
		/// Convert seconds to minutes, seconds, and milliseconds.
		/// </summary>
		/// <param name="inputSeconds">Current number of seconds</param>
		/// <param name="showMilliSec">Whether to show milliseconds</param>
		/// <returns></returns>
		public static string ConvertTimeToMinAndSec(double inputSeconds, bool showMilliSec = false)
		{
			int min = ((int)inputSeconds / 60) % 60;
			int sec = (int)inputSeconds % 60;
			double milliSeconds = (inputSeconds % 1) * 1000;

			return showMilliSec ? $"{min:D2}:{sec:D2}:{milliSeconds:00}" : $"{min:D2}:{sec:D2}";
		}

		public static string ConvertIntSecToStringMinAndSec(float seconds)
        {
			int min = ((int)seconds / 60) % 60;
            int sec = (int)seconds % 60;

			if (min > 0 && sec > 0)
			{
				return $"{min} min {sec} sec";
			}
			else if (min > 0 && sec == 0)
			{
				return $"{min} min";
			}
			else
			{
				return $"{sec} sec";
			}
        }
	}
}