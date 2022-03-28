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
	public class ObjectsPoolView : PausableMonoBehaviour
	{
		[SerializeField] protected List<PoolObject> _poolObjectPrefabs = default;

		protected DiContainer _diContainer = default;

		protected Dictionary<Type, PoolObjectsHolderView> _poolObjectsHolderViews = new Dictionary<Type, PoolObjectsHolderView>();

		public virtual List<PoolObject> PoolObjectPrefabs => _poolObjectPrefabs;

		[Inject]
		protected virtual void Construct(DiContainer diContainer)
		{
			_diContainer = diContainer;
		}

#if UNITY_EDITOR
		[ContextMenu("ValidatePoolObjects")]
		protected virtual void ValidatePoolObjects()
		{
			_poolObjectPrefabs = _poolObjectPrefabs.Distinct().ToList();
		}
#endif

		protected virtual void Awake()
		{
			Initialize();
		}

		protected virtual void Initialize()
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

				PlaceholderFactory<IPoolObject> factory = GenericMethodGenerator.GetGenericMethod(typeof(DiContainer), nameof(DiContainer.Resolve), _diContainer)
					.WithBindingFlags(BindingFlags.Instance | BindingFlags.Public)
					.WithTypeArguments(objectFactoryType)
					.Invoke<PlaceholderFactory<IPoolObject>>();

				poolObjectsHolderView.Initialize(factory, _poolObjectPrefabs[i].Name);

				_poolObjectsHolderViews.Add(objectType, poolObjectsHolderView);
			}
		}

		public virtual PoolObject GetObjectPrefab<T>() where T : IPoolObject
		{
			return _poolObjectPrefabs.Find(x => x.GetType() == typeof(T));
		}

		public virtual T GetObject<T>() where T : IPoolObject
		{
			return (T)GetPoolObjectsHolderView<T>().GetObject();
		}

		public virtual T GetRandomObject<T>() where T : IPoolObject
		{
			return (T)GetPoolObjectsHolderViews<T>().GetRandom().GetObject();
		}

		public virtual PoolObjectProvider<T> GetObjectProvider<T>() where T : IPoolObject
		{
			return new PoolObjectProvider<T>((T)GetPoolObjectsHolderView<T>().GetObject());
		}

		public virtual List<IPoolObject> GetActiveObjectsOfType<T>() where T : IPoolObject
		{
			return GetPoolObjectsHolderView<T>().ActiveObjects;
		}

		public virtual List<IPoolObject> GetActiveObjects()
		{
			List<IPoolObject> activeObjects = new List<IPoolObject>();

			foreach (KeyValuePair<Type, PoolObjectsHolderView> keyValuePair in _poolObjectsHolderViews)
			{
				activeObjects.AddRange(keyValuePair.Value.ActiveObjects);
			}

			return activeObjects;
		}

		public virtual void ReturnToPoolObjectsOfType<T>() where T : IPoolObject
		{
			GetPoolObjectsHolderView<T>().ReturnToPoolObjects();
		}

		public virtual void ReturnToPoolObjects()
		{
			foreach (KeyValuePair<Type, PoolObjectsHolderView> keyValuePair in _poolObjectsHolderViews)
			{
				keyValuePair.Value.ReturnToPoolObjects();
			}
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

		protected virtual PoolObjectsHolderView GetPoolObjectsHolderView<T>()
		{
			return _poolObjectsHolderViews[typeof(T)];
		}

		protected virtual List<PoolObjectsHolderView> GetPoolObjectsHolderViews<T>()
		{
			List<PoolObjectsHolderView> poolObjectsHolderViews = new List<PoolObjectsHolderView>();

			foreach (KeyValuePair<Type, PoolObjectsHolderView> keyValuePair in _poolObjectsHolderViews)
			{
				if (keyValuePair.Key is T)
				{
					poolObjectsHolderViews.Add(keyValuePair.Value);
				}
			}

			return poolObjectsHolderViews;
		}
	}
}
