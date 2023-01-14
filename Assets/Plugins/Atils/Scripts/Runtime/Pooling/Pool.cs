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
	public abstract class Pool : PausableMonoBehaviour
	{
		[SerializeField] protected List<PoolObject> _poolObjectPrefabs = default;

		[Inject] protected DiContainer _diContainer = default;

		protected Dictionary<Type, PoolObjectsHolderView> _poolObjectsHolderViews = new Dictionary<Type, PoolObjectsHolderView>();

		public virtual List<PoolObject> PoolObjectPrefabs => _poolObjectPrefabs;

		protected virtual void Awake()
		{
			InitializePoolObjectHolders();
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
			return _poolObjectPrefabs.Find(x => x.GetType() == typeof(T));
		}

		public virtual T GetObject<T>() where T : IPoolObject
		{
			return (T)GetPoolObjectsHolderViewOfType<T>().GetObject();
		}

		public virtual T GetRandomObject<T>() where T : IPoolObject
		{
			return (T)GetPoolObjectsHolderViewsOfType<T>().GetRandom().GetObject();
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
		/// Get a copy of the list of active and inactive objects
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public virtual List<IPoolObject> GetAllObjectsOfType<T>() where T : IPoolObject
		{
			return GetPoolObjectsHolderViewOfType<T>().AllObjects;
		}

		/// <summary>
		/// Get a copy of the list of active objects
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public virtual List<IPoolObject> GetActiveObjectsOfType<T>() where T : IPoolObject
		{
			return GetPoolObjectsHolderViewOfType<T>().ActiveObjects;
		}

		/// <summary>
		/// Get a copy of the list of inactive objects
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public virtual List<IPoolObject> GetInactiveObjectsOfType<T>() where T : IPoolObject
		{
			return GetPoolObjectsHolderViewOfType<T>().InactiveObjects;
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
			GetPoolObjectsHolderViewOfType<T>().ReturnToPoolAllObjects();
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
			return _poolObjectsHolderViews[typeof(T)];
		}

		protected virtual List<PoolObjectsHolderView> GetPoolObjectsHolderViewsOfType<T>()
		{
			List<PoolObjectsHolderView> poolObjectsHolderViews = new List<PoolObjectsHolderView>();

			foreach (KeyValuePair<Type, PoolObjectsHolderView> keyValuePair in _poolObjectsHolderViews)
			{
				if (keyValuePair.Key.IsSubclassOf(typeof(T)) || keyValuePair.Key == typeof(T))
				{
					poolObjectsHolderViews.Add(keyValuePair.Value);
				}
			}

			return poolObjectsHolderViews;
		}
	}
}
