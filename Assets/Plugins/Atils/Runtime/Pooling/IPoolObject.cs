using System;
using UnityEngine;
using Zenject;

namespace Atils.Runtime.Pooling
{
	public interface IPoolObject
	{
		string Name { set; get; }
		Transform Transform { get; }
		GameObject GameObject { get; }
		bool IsActiveInPool { get; }

		void Pause();
		void Unpause();
		void ReturnToPool(Action onReturnedToPool = null);

		//void SetLocation(Vector3 position, Quaternion rotation, Vector3 localScale);
		//void Initialize(IInitializeData initializeData);
	}
}
