using UnityEngine;

namespace Atils.Runtime.Pooling
{
	public class PoolObjectProvider
	{
		public readonly IPoolObject PoolObject = default;

		public PoolObjectProvider(IPoolObject poolObject)
		{
			PoolObject = poolObject;
		}

		public IPoolObject GetObject()
		{
			return PoolObject;
		}

		public PoolObjectProvider SetPosition(Vector3 position)
		{
			PoolObject.Transform.position = position;
			return this;
		}

		public PoolObjectProvider SetRotation(Quaternion rotation)
		{
			PoolObject.Transform.rotation = rotation;
			return this;
		}

		public PoolObjectProvider SetLocalScale(Vector3 localScale)
		{
			PoolObject.Transform.localScale = localScale;
			return this;
		}

		public PoolObjectProvider SetParent(Transform parent)
		{
			PoolObject.Transform.SetParent(parent);
			return this;
		}
	}
}
