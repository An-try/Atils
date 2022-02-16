using UnityEngine;

namespace Atils.Runtime.Pooling
{
	public class PoolObjectProvider<T> where T : IPoolObject
	{
		public readonly T PoolObject = default;

		public PoolObjectProvider(T poolObject)
		{
			PoolObject = poolObject;
		}

		public T GetObject()
		{
			return PoolObject;
		}

		public PoolObjectProvider<T> SetPosition(Vector3 position)
		{
			PoolObject.Transform.position = position;
			return this;
		}

		public PoolObjectProvider<T> SetRotation(Quaternion rotation)
		{
			PoolObject.Transform.rotation = rotation;
			return this;
		}

		public PoolObjectProvider<T> SetLocalScale(Vector3 localScale)
		{
			PoolObject.Transform.localScale = localScale;
			return this;
		}

		public PoolObjectProvider<T> SetParent(Transform parent)
		{
			PoolObject.Transform.SetParent(parent);
			return this;
		}
	}
}
