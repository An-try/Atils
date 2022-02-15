using Atils.Runtime.Pause;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Atils.Runtime.Pooling
{
	public class PoolObjectsHolderView : PausableMonoBehaviour
	{
		private PlaceholderFactory<IPoolObject> _factory = default;

		private List<IPoolObject> _poolObjects = new List<IPoolObject>();

		public List<IPoolObject> ActiveObjects => _poolObjects.FindAll(x => x.IsActiveInPool);

		public void Initialize(PlaceholderFactory<IPoolObject> factory)
		{
			_factory = factory;
		}

		public IPoolObject GetObject<T>() where T : IPoolObject
		{
			return GetAvailableObjectOrCreateNew<T>();
		}

		public void ReturnToPoolObjects()
		{
			for (int i = 0; i < _poolObjects.Count; i++)
			{
				_poolObjects[i].ReturnToPool();
			}
		}

		protected override void OnPaused()
		{
			for (int i = 0; i < _poolObjects.Count; i++)
			{
				if (_poolObjects[i].IsActiveInPool)
				{
					_poolObjects[i].Pause();
				}
			}
		}

		protected override void OnUnpaused()
		{
			for (int i = 0; i < _poolObjects.Count; i++)
			{
				if (_poolObjects[i].IsActiveInPool)
				{
					_poolObjects[i].Unpause();
				}
			}
		}

		private IPoolObject GetAvailableObjectOrCreateNew<T>() where T : IPoolObject
		{
			for (int i = 0; i < _poolObjects.Count; i++)
			{
				if (!_poolObjects[i].IsActiveInPool)
				{
					return InitializePoolObject(_poolObjects[i]);
				}
			}

			IPoolObject poolObject = AddObject<T>(typeof(T).ToString(), transform);
			return InitializePoolObject(poolObject);
		}

		private IPoolObject AddObject<T>(string objectName, Transform objectParent = null) where T : IPoolObject
		{
			Transform parent = objectParent == null ? transform : objectParent;
			IPoolObject poolObject = _factory.Create();

			poolObject.Name = objectName;
			poolObject.Transform.SetParent(parent);
			poolObject.GameObject.SetActive(false);

			_poolObjects.Add(poolObject);
			return poolObject;
		}

		private IPoolObject InitializePoolObject(IPoolObject poolObject)
		{
			// TODO
			poolObject.GameObject.SetActive(true);

			if (IsPaused)
			{
				poolObject.Pause();
			}

			return poolObject;
		}
	}
}
