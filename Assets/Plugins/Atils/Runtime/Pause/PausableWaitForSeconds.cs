using System;
using UnityEngine;

namespace Atils.Runtime.Pause
{
	public class PausableWaitForSeconds : CustomYieldInstruction
	{
		public override bool keepWaiting => !IsTimeUp();

		private readonly Func<bool> _isPaused;
		private float _timeLeft;

		public PausableWaitForSeconds(float seconds, Func<bool> isPaused)
		{
			_isPaused = isPaused;
			_timeLeft = seconds;
		}

		private bool IsTimeUp()
		{
			if (_isPaused != null && !_isPaused())
			{
				_timeLeft -= Time.deltaTime;
			}

			return _timeLeft <= 0f;
		}
	}
}
