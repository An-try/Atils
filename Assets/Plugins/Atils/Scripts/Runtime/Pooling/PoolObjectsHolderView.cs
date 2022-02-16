using Atils.Runtime.Pause;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Atils.Runtime.Pooling
{
	public class PoolObjectsHolderView : PausableMonoBehaviour
	{
		private PlaceholderFactory<IPoolObject> _factory = default;

		private List<IPoolObject> _enabledPoolObjects = new List<IPoolObject>();
		private List<IPoolObject> _disabledPoolObjects = new List<IPoolObject>();

		public List<IPoolObject> ActiveObjects => _enabledPoolObjects.Concat(_disabledPoolObjects).ToList();

		public void Initialize(PlaceholderFactory<IPoolObject> factory)
		{
			_factory = factory;
		}

		private void FixedUpdate()
		{
			float timeStep = Time.fixedDeltaTime;

			for (int i = 0; i < _enabledPoolObjects.Count; i++)
			{
				_enabledPoolObjects[i].UpdateObject(timeStep);
			}
		}

		public T GetObject<T>() where T : IPoolObject
		{
			return GetAvailableObjectOrCreateNew<T>();
		}

		public void ReturnToPoolObjects()
		{
			while (_enabledPoolObjects.Count > 0)
			{
				_enabledPoolObjects[_enabledPoolObjects.Count - 1].ReturnToPool();
			}
			_enabledPoolObjects.Clear();
		}

		protected override void OnPaused()
		{
			foreach (IPoolObject poolObject in _enabledPoolObjects)
			{
				poolObject.Pause();
			}
		}

		protected override void OnUnpaused()
		{
			foreach (IPoolObject poolObject in _enabledPoolObjects)
			{
				poolObject.Unpause();
			}
		}

		private void OnObjectReturnedToPool(IPoolObject poolObject)
		{
			_enabledPoolObjects.Remove(poolObject);
			_disabledPoolObjects.Add(poolObject);
		}

		private T GetAvailableObjectOrCreateNew<T>() where T : IPoolObject
		{
			if (_disabledPoolObjects.Count > 0)
			{
				return (T)InitializePoolObjectAtEnd(_disabledPoolObjects[_disabledPoolObjects.Count - 1]);
			}

			return (T)InitializePoolObjectAtEnd(AddObject<T>(typeof(T).ToString(), transform));
		}

		private IPoolObject AddObject<T>(string objectName, Transform objectParent = null) where T : IPoolObject
		{
			Transform parent = objectParent == null ? transform : objectParent;
			IPoolObject poolObject = _factory.Create();

			poolObject.Name = objectName;
			poolObject.Transform.SetParent(parent);
			poolObject.GameObject.SetActive(false);
			poolObject.OnReturnedToPool += OnObjectReturnedToPool;

			_disabledPoolObjects.Add(poolObject);
			return poolObject;
		}

		private T InitializePoolObjectAtEnd<T>(T poolObject) where T : IPoolObject
		{
			// TODO
			poolObject.GameObject.SetActive(true);
			poolObject.Initialize();
			_disabledPoolObjects.RemoveAt(_disabledPoolObjects.Count - 1);
			_enabledPoolObjects.Add(poolObject);

			if (IsPaused)
			{
				poolObject.Pause();
			}

			return poolObject;
		}
	}
}
