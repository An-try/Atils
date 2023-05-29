using Atils.Runtime.Pause;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Atils.Runtime.Pooling
{
	public class PoolObjectsHolderView : PausableMonoBehaviour
	{
		public Action<IPoolObject> OnObjectPulledOutFromPoolEvent { get; set; }
		public Action<IPoolObject> OnObjectPulledOutFromPoolAndInitializedEvent { get; set; }
		public Action<IPoolObject> OnObjectReturnedToPoolEvent { get; set; }

		private PlaceholderFactory<IPoolObject> _factory = default;
		private string _poolObjectName = default;

		private List<IPoolObject> _enabledPoolObjects = new List<IPoolObject>();
		private List<IPoolObject> _disabledPoolObjects = new List<IPoolObject>();

		/// <summary>
		/// Get a copy of the list of active and inactive objects
		/// </summary>
		public List<IPoolObject> AllObjects => _enabledPoolObjects.Concat(_disabledPoolObjects).ToList();

		/// <summary>
		/// Get a copy of the list of active objects
		/// </summary>
		public List<IPoolObject> ActiveObjects => new List<IPoolObject>(_enabledPoolObjects);

		/// <summary>
		/// Get a copy of the list of inactive objects
		/// </summary>
		public List<IPoolObject> InactiveObjects => new List<IPoolObject>(_disabledPoolObjects);

		public void Initialize(PlaceholderFactory<IPoolObject> factory, string poolObjectName)
		{
			_factory = factory;
			_poolObjectName = poolObjectName;
		}

		private void FixedUpdate()
		{
			float timeStep = Time.fixedDeltaTime;

			for (int i = 0; i < _enabledPoolObjects.Count; i++)
			{
				_enabledPoolObjects[i].UpdateObject(timeStep);
			}
		}

		public IPoolObject GetObject()
		{
			return GetAvailableObjectOrCreateNew();
		}

		public void ReturnToPoolAllObjects()
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

		private void OnObjectPulledOutFromPoolAndInitialized(IPoolObject poolObject)
		{
			OnObjectPulledOutFromPoolAndInitializedEvent?.Invoke(poolObject);
		}

		private void OnObjectReturnedToPool(IPoolObject poolObject)
		{
			_enabledPoolObjects.Remove(poolObject);
			_disabledPoolObjects.Add(poolObject);
			OnObjectReturnedToPoolEvent?.Invoke(poolObject);
		}

		private IPoolObject GetAvailableObjectOrCreateNew()
		{
			if (_disabledPoolObjects.Count > 0)
			{
				return InitializePoolObjectAtEnd(_disabledPoolObjects[_disabledPoolObjects.Count - 1]);
			}

			return InitializePoolObjectAtEnd(AddObject(transform));
		}

		private IPoolObject AddObject(Transform parent = null)
		{
			IPoolObject poolObject = _factory.Create();

			poolObject.Name = _poolObjectName;
			poolObject.Transform.parent = parent;
			poolObject.GameObject.SetActive(false);
			poolObject.OnInitializedEvent += OnObjectPulledOutFromPoolAndInitialized;
			poolObject.OnReturnedToPoolEvent += OnObjectReturnedToPool;

			_disabledPoolObjects.Add(poolObject);
			return poolObject;
		}

		private T InitializePoolObjectAtEnd<T>(T poolObject) where T : IPoolObject
		{
			// TODO
			poolObject.GameObject.SetActive(true);
			_disabledPoolObjects.RemoveAt(_disabledPoolObjects.Count - 1);
			_enabledPoolObjects.Add(poolObject);

			if (IsPaused)
			{
				poolObject.Pause();
			}

			OnObjectPulledOutFromPoolEvent?.Invoke(poolObject);
			return poolObject;
		}
	}
}
