using System;
using UnityEngine;

namespace BidOnGamesUtils.Runtime.Timers
{
	public class CountdownTimer : ITimer, IDisposable
	{
		private int startSeconds = default;

		private bool isRunning;
		private float startTimeStamp = default;
		private float pauseTimeStamp = default;

		public int StartTime => startSeconds;

		/// <summary>
		/// Time left in seconds.
		/// </summary>
		public float RemainingTime => isRunning ?
									  startSeconds - (Time.realtimeSinceStartup - startTimeStamp) :
									  startSeconds - (pauseTimeStamp - startTimeStamp);

		/// <summary>
		/// Time elapsed since the start of the timer 
		/// </summary>
		public float ElapsedTime => startSeconds - RemainingTime;

		public void Start(int startSeconds)
		{
			this.startSeconds = startSeconds;
			isRunning = true;
			startTimeStamp = Time.realtimeSinceStartup;
		}

		public void Stop()
		{
			Pause();
		}

		public void Pause()
		{
			if (isRunning)
			{
				isRunning = false;
				// pauseTimeStamp must be set to correctly return the time after this timer has paused or stopped.
				pauseTimeStamp = Time.realtimeSinceStartup;
			}
		}

		public void UnPause()
		{
			if (!isRunning)
			{
				isRunning = true;
				startTimeStamp += Time.realtimeSinceStartup - pauseTimeStamp;
			}
		}

		/// <summary>
		/// Decrease the time elapsed by this timer, but you cannot decrease the elapsed time less than 0.
		/// </summary>
		/// <param name="addSeconds"></param>
		public void DecreaseTime(int addSeconds)
		{
			this.startTimeStamp += addSeconds;

			if (this.startTimeStamp > Time.realtimeSinceStartup)
			{
				this.startTimeStamp = Time.realtimeSinceStartup;
			}	
		}

		public void Dispose()
		{
			Stop();
		}
	}
}