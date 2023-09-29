using Atils.Runtime.Extensions;
using Atils.Runtime.Pause;
using System;
using System.Collections;
using UnityEngine;

namespace Atils.Runtime.Pooling
{
	public abstract class PoolObject : PausableMonoBehaviour, IPoolObject
	{
		public Action<IPoolObject> OnInitializedEvent { get; set; }
		public Action<IPoolObject> OnReturnedToPoolEvent { get; set; }

		/// <summary>
		/// Prevention of looping when returning to the pool.
		/// </summary>
		private bool _isReturningToPool = false;

		private Coroutine _returnToPoolCoroutine = default;

		public string Name { get => name; set => name = value; }
		public GameObject GameObject => gameObject;
		public Transform Transform => transform;

		public bool IsActive => gameObject.activeSelf;

		protected virtual void OnDestroy()
		{
			this.KillCoroutine(ref _returnToPoolCoroutine);
			OnInitializedEvent = null;
			OnReturnedToPoolEvent = null;
		}

		public void ReturnToPool(Action onReturnedToPool = null)
		{
			if (_isReturningToPool)
			{
				return;
			}

			_isReturningToPool = true;

			this.KillCoroutine(ref _returnToPoolCoroutine);

			if (IsActive)
			{
				OnPreReturnedToPool();
				gameObject.SetActive(false);
				OnAfterReturnedToPool();
				onReturnedToPool?.Invoke();
				OnReturnedToPoolEvent?.Invoke(this);
			}

			_isReturningToPool = false;
		}

		public void ReturnToPool(float delay, Action onFinish = null)
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

		public virtual void UpdateObject(float timeStep)
		{ }

		protected virtual void OnPreReturnedToPool()
		{ }

		protected virtual void OnAfterReturnedToPool()
		{ }

		protected override void OnPaused()
		{ }

		protected override void OnUnpaused()
		{ }

		private IEnumerator ReturnToPoolCoroutine(float delay, Action onFinish = null)
		{
			yield return new PausableWaitForSeconds(delay, () => IsPaused);

			ReturnToPool(onFinish);
		}
	}
}
