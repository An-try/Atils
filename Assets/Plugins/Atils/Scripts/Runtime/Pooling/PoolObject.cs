using Atils.Runtime.Pause;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Atils.Runtime.Pooling
{
	public abstract class PoolObject : PausableMonoBehaviour, IPoolObject
    {
		private Coroutine _returnToPoolCoroutine = default;

		public string Name { get => name; set => name = value; }
		public bool IsActiveInPool => gameObject.activeSelf;
		public GameObject GameObject => gameObject;
		public Transform Transform => transform;

		//public virtual void Initialize(IInitializeData initializeData)
		//{
		//	GameObjectInitializeData gameObjectInitializeData = (GameObjectInitializeData)initializeData;
		//	SetLocation(gameObjectInitializeData.Position, gameObjectInitializeData.Rotation, gameObjectInitializeData.LocalScale);
		//}

		public virtual void UpdateObject(float timeStep)
		{
			if (IsPaused)
			{
				return;
			}
		}

		//public void SetLocation(Vector3 position, Quaternion rotation, Vector3 localScale)
		//{
		//	transform.position = position;
		//	transform.rotation = rotation;
		//	transform.localScale = localScale;
		//}

		public virtual void ReturnToPool(Action onReturnedToPool = null)
		{
			KillCoroutine(ref _returnToPoolCoroutine);
			gameObject.SetActive(false);
			ResetObject();
			onReturnedToPool?.Invoke();
		}

		public virtual void ReturnToPool(float delay, Action onFinish = null)
		{
			if (delay <= 0)
			{
				ReturnToPool(onFinish);
			}
			else
			{
				KillCoroutine(ref _returnToPoolCoroutine);
				_returnToPoolCoroutine = StartCoroutine(ReturnToPoolCoroutine(delay, onFinish));
			}
		}

		protected override void OnPaused()
		{ }

		protected override void OnUnpaused()
		{ }

		protected abstract void ResetObject();

		private void KillCoroutine(ref Coroutine coroutine)
		{
			if (coroutine != null)
			{
				StopCoroutine(coroutine);
				coroutine = null;
			}
		}

		private IEnumerator ReturnToPoolCoroutine(float delay, Action onFinish = null)
		{
			yield return new PausableWaitForSeconds(delay, () => IsPaused);

			ReturnToPool(onFinish);
		}
	}
}
