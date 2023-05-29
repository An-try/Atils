using UnityEngine;

namespace Atils.Runtime.Handlers
{
	public class ColliderHandler : BaseHandler
	{
		public ColliderEnterHandler OnColliderEnterEvent = default;
		public ColliderStayHandler OnColliderStayEvent = default;
		public ColliderExitHandler OnColliderExitEvent = default;

		protected override void Awake()
		{
			base.Awake();

			if (Collider.isTrigger)
			{
				Debug.LogWarning(this + ": The collider is set as trigger. It should not be designated as a trigger. Fixing.");
				Collider.isTrigger = false;
			}
		}

		private void OnCollisionEnter(Collision collision)
		{
			OnColliderEnterEvent?.Invoke(collision);
		}

		private void OnCollisionStay(Collision collision)
		{
			OnColliderStayEvent?.Invoke(collision);
		}

		private void OnCollisionExit(Collision collision)
		{
			OnColliderExitEvent?.Invoke(collision);
		}
	}
}
