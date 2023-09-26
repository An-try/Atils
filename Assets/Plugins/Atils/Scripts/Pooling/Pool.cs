using Atils.Runtime.Extensions;
using Atils.Runtime.Generics;
using Atils.Runtime.Pause;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Zenject;

namespace Atils.Runtime.Pooling
{
	public abstract class Pool : PausableMonoBehaviour, IDisposable
	{
		public Action<IPoolObject> OnObjectPulledOutFromPoolEvent { get; set; }
		public Action<IPoolObject> OnObjectPulledOutFromPoolAndInitializedEvent { get; set; }
		public Action<IPoolObject> OnObjectReturnedToPoolEvent { get; set; }

		[SerializeField] protected List<PoolObject> _poolObjectPrefabs = default;

		[Inject] protected DiContainer _diContainer = default;

		protected Dictionary<Type, PoolObjectsHolderView> _poolObjectsHolderViews = new Dictionary<Type, PoolObjectsHolderView>();

		public virtual List<PoolObject> PoolObjectPrefabs => _poolObjectPrefabs;

		[Inject]
		public virtual void Construct()
		{
			InitializePoolObjectHolders();

			foreach (KeyValuePair<Type, PoolObjectsHolderView> keyValuePair in _poolObjectsHolderViews)
			{
				keyValuePair.Value.OnObjectPulledOutFromPoolEvent += OnObjectPulledOutFromPool;
				keyValuePair.Value.OnObjectPulledOutFromPoolAndInitializedEvent += OnObjectPulledOutFromPoolAndInitialized;
				keyValuePair.Value.OnObjectReturnedToPoolEvent += OnObjectReturnedToPool;
			}
		}

		public virtual void Dispose()
		{
			foreach (KeyValuePair<Type, PoolObjectsHolderView> keyValuePair in _poolObjectsHolderViews)
			{
				keyValuePair.Value.OnObjectPulledOutFromPoolEvent -= OnObjectPulledOutFromPool;
				keyValuePair.Value.OnObjectPulledOutFromPoolAndInitializedEvent -= OnObjectPulledOutFromPoolAndInitialized;
				keyValuePair.Value.OnObjectReturnedToPoolEvent -= OnObjectReturnedToPool;
			}
		}

		private void OnObjectPulledOutFromPool(IPoolObject poolObject)
		{
			OnObjectPulledOutFromPoolEvent?.Invoke(poolObject);
		}

		private void OnObjectPulledOutFromPoolAndInitialized(IPoolObject poolObject)
		{
			OnObjectPulledOutFromPoolAndInitializedEvent?.Invoke(poolObject);
		}

		private void OnObjectReturnedToPool(IPoolObject poolObject)
		{
			OnObjectReturnedToPoolEvent?.Invoke(poolObject);
		}

		protected virtual void InitializePoolObjectHolders()
		{
			for (int i = 0; i < _poolObjectPrefabs.Count; i++)
			{
				GameObject parentForPoolObjects = new GameObject(_poolObjectPrefabs[i].Name + "ObjectsPoolView");
				PoolObjectsHolderView poolObjectsHolderView = parentForPoolObjects.AddComponent<PoolObjectsHolderView>();
				poolObjectsHolderView.transform.parent = transform;

				Type objectType = _poolObjectPrefabs[i].GetType();
				Type objectFactoryType = objectType.GetNestedType("Factory");

				PlaceholderFactory<IPoolObject> factory = GenericMethodGenerator
					.GetGenericMethod(typeof(DiContainer), nameof(DiContainer.Resolve), _diContainer)
					.WithBindingFlags(BindingFlags.Instance | BindingFlags.Public)
					.WithTypeArguments(objectFactoryType)
					.Invoke<PlaceholderFactory<IPoolObject>>();

				poolObjectsHolderView.Initialize(factory, _poolObjectPrefabs[i].Name);

				_poolObjectsHolderViews.Add(objectType, poolObjectsHolderView);
			}
		}

		[EasyButtons.Button]
		public virtual bool ValidatePoolObjects()
		{
			bool isSuccess = true;

			if (_poolObjectPrefabs.Count != _poolObjectPrefabs.Distinct().Count())
			{
				Debug.LogError($"There are duplicates in pool objects list on prefab {name}. Remove them!");
				isSuccess = false;
				//_poolObjectPrefabs = _poolObjectPrefabs.Distinct().ToList(); // If you want to fix it in code
			}

			foreach (PoolObject prefab in _poolObjectPrefabs)
			{
				if (prefab == null)
				{
					Debug.LogError($"There are null objects in pool objects list on prefab {name}. Remove them!");
					isSuccess = false;
					//_poolObjectPrefabs.RemoveAll(x => x == null); // // If you want to fix it in code
					break;
				}
			}

			return isSuccess;
		}

		public virtual PoolObject GetObjectPrefab<T>() where T : IPoolObject
		{
			return GetObjectPrefab(typeof(T));
		}

		public virtual PoolObject GetObjectPrefab(Type type)
		{
			return _poolObjectPrefabs.Find(x => x.GetType() == type);
		}

		public virtual T GetObject<T>() where T : IPoolObject
		{
			return (T)GetObject(typeof(T));
		}

		public virtual IPoolObject GetObject(Type type)
		{
			return GetPoolObjectsHolderViewOfType(type).GetObject();
		}

		public virtual T GetRandomObject<T>() where T : IPoolObject
		{
			return (T)GetRandomObject(typeof(T));
		}

		public virtual IPoolObject GetRandomObject(Type type)
		{
			return GetPoolObjectsHolderViewsOfType(type).GetRandom().GetObject();
		}

		/// <summary>
		/// Get a copy of the list of active and inactive objects
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public virtual List<IPoolObject> GetAllObjectsOfType<T>() where T : IPoolObject
		{
			return GetAllObjectsOfType(typeof(T));
		}

		/// <summary>
		/// Get a copy of the list of active and inactive objects
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public virtual List<IPoolObject> GetAllObjectsOfType(Type type)
		{
			return GetPoolObjectsHolderViewOfType(type).AllObjects;
		}

		/// <summary>
		/// Get a copy of the list of active and inactive objects
		/// </summary>
		/// <returns></returns>
		public virtual List<IPoolObject> GetAllObjects()
		{
			List<IPoolObject> activeObjects = new List<IPoolObject>();

			foreach (KeyValuePair<Type, PoolObjectsHolderView> keyValuePair in _poolObjectsHolderViews)
			{
				activeObjects.AddRange(keyValuePair.Value.AllObjects);
			}

			return activeObjects;
		}

		/// <summary>
		/// Get a copy of the list of active objects
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public virtual List<IPoolObject> GetActiveObjectsOfType<T>() where T : IPoolObject
		{
			return GetActiveObjectsOfType(typeof(T));
		}

		/// <summary>
		/// Get a copy of the list of active objects
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public virtual List<IPoolObject> GetActiveObjectsOfType(Type type)
		{
			return GetPoolObjectsHolderViewOfType(type).ActiveObjects;
		}

		/// <summary>
		/// Get a copy of the list of inactive objects
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public virtual List<IPoolObject> GetInactiveObjectsOfType<T>() where T : IPoolObject
		{
			return GetInactiveObjectsOfType(typeof(T));
		}

		/// <summary>
		/// Get a copy of the list of inactive objects
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public virtual List<IPoolObject> GetInactiveObjectsOfType(Type type)
		{
			return GetPoolObjectsHolderViewOfType(type).InactiveObjects;
		}

		public virtual void ReturnToPoolAllObjects()
		{
			foreach (KeyValuePair<Type, PoolObjectsHolderView> keyValuePair in _poolObjectsHolderViews)
			{
				keyValuePair.Value.ReturnToPoolAllObjects();
			}
		}

		public virtual void ReturnToPoolAllObjectsOfType<T>() where T : IPoolObject
		{
			ReturnToPoolAllObjectsOfType(typeof(T));
		}

		public virtual void ReturnToPoolAllObjectsOfType(Type type)
		{
			GetPoolObjectsHolderViewOfType(type).ReturnToPoolAllObjects();
		}

		protected override void OnPaused()
		{
			foreach (KeyValuePair<Type, PoolObjectsHolderView> keyValuePair in _poolObjectsHolderViews)
			{
				keyValuePair.Value.Pause();
			}
		}

		protected override void OnUnpaused()
		{
			foreach (KeyValuePair<Type, PoolObjectsHolderView> keyValuePair in _poolObjectsHolderViews)
			{
				keyValuePair.Value.Unpause();
			}
		}

		protected virtual PoolObjectsHolderView GetPoolObjectsHolderViewOfType<T>()
		{
			return GetPoolObjectsHolderViewOfType(typeof(T));
		}

		protected virtual PoolObjectsHolderView GetPoolObjectsHolderViewOfType(Type type)
		{
			if (_poolObjectsHolderViews.Count <= 0)
			{
				Debug.LogError($"Pool is empty.");
				return default;
			}

			if (!_poolObjectsHolderViews.ContainsKey(type))
			{
				Debug.LogError($"Pool does not contain an object of type {type}");
				return default;
			}

			return _poolObjectsHolderViews[type];
		}

		protected virtual List<PoolObjectsHolderView> GetPoolObjectsHolderViewsOfType<T>()
		{
			return GetPoolObjectsHolderViewsOfType(typeof(T));
		}

		protected virtual List<PoolObjectsHolderView> GetPoolObjectsHolderViewsOfType(Type type)
		{
			List<PoolObjectsHolderView> poolObjectsHolderViews = new List<PoolObjectsHolderView>();

			foreach (KeyValuePair<Type, PoolObjectsHolderView> keyValuePair in _poolObjectsHolderViews)
			{
				if (keyValuePair.Key.IsSubclassOf(type) || keyValuePair.Key == type)
				{
					poolObjectsHolderViews.Add(keyValuePair.Value);
				}
			}

			return poolObjectsHolderViews;
		}
	}
}
