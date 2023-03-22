using UnityEngine;

namespace Atils.Runtime.Handlers
{
	public class TriggerHandler : BaseHandler
	{
		public TriggerEnterHandler OnTriggerEnterEvent = default;
		public TriggerStayHandler OnTriggerStayEvent = default;
		public TriggerExitHandler OnTriggerExitEvent = default;

		protected override void Awake()
		{
			base.Awake();

			if (!Collider.isTrigger)
			{
				Debug.LogWarning(this + ": The collider is not set as trigger. It should be designated as a trigger. Fixing.");
				Collider.isTrigger = true;
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			OnTriggerEnterEvent?.Invoke(other);
		}

		private void OnTriggerStay(Collider other)
		{
			OnTriggerStayEvent?.Invoke(other);
		}

		private void OnTriggerExit(Collider other)
		{
			OnTriggerExitEvent?.Invoke(other);
		}
	}
}
