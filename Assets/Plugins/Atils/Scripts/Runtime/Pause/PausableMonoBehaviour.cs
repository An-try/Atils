using UnityEngine;

namespace Atils.Runtime.Pause
{
	public abstract class PausableMonoBehaviour : MonoBehaviour, IPausable
	{
		public bool IsPaused { get; private set; }

		public void Pause()
		{
			IsPaused = true;
			OnPaused();
		}

		public void Unpause()
		{
			IsPaused = false;
			OnUnpaused();
		}

		protected abstract void OnPaused();
		protected abstract void OnUnpaused();
	}
}
