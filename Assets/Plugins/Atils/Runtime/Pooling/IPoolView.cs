using System.Collections.Generic;

namespace Atils.Runtime.Pooling
{
	public interface IPoolView
    {
		PoolObject GetObjectPrefab<T>() where T : IPoolObject;

		//PoolObject PoolObjectPrefab { get; }
		//IEnumerable<IPoolObject> ActiveObjects { get; }

			//IPoolObject GetObject();
			//void ReturnToPoolAllObjects();
			//void Pause();
			//void Unpause();
	}
}
