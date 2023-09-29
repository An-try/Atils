using System;
using UnityEngine;

namespace Atils.Runtime.Pooling
{
	public interface IPoolObject
	{
		Action<IPoolObject> OnInitializedEvent { get; set; }
		Action<IPoolObject> OnReturnedToPoolEvent { get; set; }

		string Name { set; get; }
		Transform Transform { get; }
		GameObject GameObject { get; }

		void UpdateObject(float timeStep);
		void Pause();
		void Unpause();
		void ReturnToPool(Action onReturnedToPool = null);
	}
}