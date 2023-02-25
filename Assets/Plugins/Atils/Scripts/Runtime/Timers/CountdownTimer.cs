using System;
using UnityEngine;

namespace Atils.Runtime.Timers
{
	public class CountdownTimer : ITimer, IDisposable
	{
		private int _startSeconds = 0;

		private bool _isRunning = false;
		private float _startTimeStamp = 0;
		private float _pauseTimeStamp = 0;

		public int StartTime => _startSeconds;

		/// <summary>
		/// Time left in seconds.
		/// </summary>
		public float RemainingTime => _isRunning ?
									  _startSeconds - (Time.realtimeSinceStartup - _startTimeStamp) :
									  _startSeconds - (_pauseTimeStamp - _startTimeStamp);

		/// <summary>
		/// Time elapsed since the start of the timer 
		/// </summary>
		public float ElapsedTime => _startSeconds - RemainingTime;

		public void Start(int startSeconds)
		{
			_startSeconds = startSeconds;
			_isRunning = true;
			_startTimeStamp = Time.realtimeSinceStartup;
		}

		public void Stop()
		{
			Pause();
		}

		public void Pause()
		{
			if (_isRunning)
			{
				_isRunning = false;
				// pauseTimeStamp must be set to correctly return the time after this timer has paused or stopped.
				_pauseTimeStamp = Time.realtimeSinceStartup;
			}
		}

		public void UnPause()
		{
			if (!_isRunning)
			{
				_isRunning = true;
				_startTimeStamp += Time.realtimeSinceStartup - _pauseTimeStamp;
			}
		}

		public void AddStartTime(int addSeconds)
		{
			_startSeconds += addSeconds;
		}

		public void Dispose()
		{
			Stop();
		}
	}
}