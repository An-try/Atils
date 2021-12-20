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

		public PoolObjectProvider WithPosition(Vector3 position)
		{
			PoolObject.Transform.position = position;
			return this;
		}

		public PoolObjectProvider WithRotation(Quaternion rotation)
		{
			PoolObject.Transform.rotation = rotation;
			return this;
		}

		public PoolObjectProvider WithLocalScale(Vector3 localScale)
		{
			PoolObject.Transform.localScale = localScale;
			return this;
		}

		public PoolObjectProvider WithParent(Transform parent)
		{
			PoolObject.Transform.SetParent(parent);
			return this;
		}
	}
}
