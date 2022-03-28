using System;
using UnityEngine;

namespace Atils.Runtime.Pooling
{
	public interface IPoolObject
	{
		Action<IPoolObject> OnReturnedToPool { get; set; }

		string Name { set; get; }
		Transform Transform { get; }
		GameObject GameObject { get; }

		IPoolObject SetPosition(Vector3 position);
		IPoolObject SetRotation(Quaternion rotation);
		IPoolObject SetLocalScale(Vector3 localScale);
		IPoolObject SetParent(Transform parent);

		void UpdateObject(float timeStep);
		void Pause();
		void Unpause();
		void ReturnToPool(Action onReturnedToPool = null);

		//void SetLocation(Vector3 position, Quaternion rotation, Vector3 localScale);
		void Initialize();
	}
}
