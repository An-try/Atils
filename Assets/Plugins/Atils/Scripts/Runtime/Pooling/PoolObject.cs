using Atils.Runtime.Extensions;
using Atils.Runtime.Pause;
using System;
using System.Collections;
using UnityEngine;

namespace Atils.Runtime.Pooling
{
	public abstract class PoolObject : PausableMonoBehaviour, IPoolObject
	{
		public Action<IPoolObject> OnReturnedToPool { get; set; }

		private Coroutine _returnToPoolCoroutine = default;

		public string Name { get => name; set => name = value; }
		public GameObject GameObject => gameObject;
		public Transform Transform => transform;

		public virtual void UpdateObject(float timeStep)
		{ }

		public virtual void ReturnToPool(Action onReturnedToPool = null)
		{
			this.KillCoroutine(ref _returnToPoolCoroutine);
			gameObject.SetActive(false);
			ResetObject();
			onReturnedToPool?.Invoke();
			OnReturnedToPool?.Invoke(this);
		}

		public virtual void ReturnToPool(float delay, Action onFinish = null)
		{
			if (delay <= 0)
			{
				ReturnToPool(onFinish);
			}
			else
			{
				this.KillCoroutine(ref _returnToPoolCoroutine);
				_returnToPoolCoroutine = StartCoroutine(ReturnToPoolCoroutine(delay, onFinish));
			}
		}

		protected override void OnPaused()
		{ }

		protected override void OnUnpaused()
		{ }

		/// <summary>
		/// Called then returning to pool.
		/// </summary>
		protected abstract void ResetObject();

		private IEnumerator ReturnToPoolCoroutine(float delay, Action onFinish = null)
		{
			yield return new PausableWaitForSeconds(delay, () => IsPaused);

			ReturnToPool(onFinish);
		}

		protected virtual void OnDestroy()
		{
			this.KillCoroutine(ref _returnToPoolCoroutine);
			OnReturnedToPool = null;
		}
	}
}
