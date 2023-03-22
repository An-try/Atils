using UnityEngine;

namespace Atils.Runtime.Handlers
{
	public abstract class BaseHandler : MonoBehaviour
	{
		[SerializeField] private Collider _collider = default;

		protected Collider Collider => _collider;

		protected virtual void Awake()
		{
			if (_collider == null)
			{
				Debug.LogError(this + ": Collider reference is null.");
			}
		}

		public void Enable()
		{
			_collider.enabled = true;
		}

		public void Disable()
		{
			_collider.enabled = false;
		}
	}
}
