using Atils.Runtime.Pause;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Zenject;

namespace Atils.Runtime.Pooling
{
	public abstract class ObjectsPoolView : PausableMonoBehaviour, IPoolView
	{
		[SerializeField] private List<PoolObject> _poolObjectPrefabs = default;
		Dictionary<Type, PoolObjectsHolderView> _poolObjectsHolderViews = new Dictionary<Type, PoolObjectsHolderView>();
		public List<PoolObject> PoolObjectPrefabs => _poolObjectPrefabs;

		//[SerializeField] private PoolObject _poolObjectPrefab = default;
		[SerializeField] private int _starterAmount = 1;

		//private T _factory = default;

		//private List<PoolObject> _poolObjects = default;

		//public PoolObject PoolObjectPrefab => _poolObjectPrefab;
		//public IEnumerable<IPoolObject> ActiveObjects => _poolObjects.Where(@object => @object.IsActiveInPool);

		//[Inject]
		//private void Construct(T factory)
		//{
		//	_factory = factory;
		//}

		private DiContainer _diContainer = default;

		[Inject]
		private void Construct(DiContainer diContainer)
		{
			_diContainer = diContainer;
		}

#if UNITY_EDITOR
		[ContextMenu("ValidatePoolObjects")]
		private void ValidatePoolObjects()
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
			for (int i = 0; i < _poolObjectPrefabs.Count; i++)
			{
				GameObject parentForPoolObjects = new GameObject(_poolObjectPrefabs[i].Name + "ObjectsPoolView");
				PoolObjectsHolderView poolObjectsHolderView = parentForPoolObjects.AddComponent<PoolObjectsHolderView>();
				poolObjectsHolderView.transform.parent = transform;

				Type objectType = _poolObjectPrefabs[i].GetType();
				Type objectFactoryType = objectType.GetNestedType("Factory");

				MethodInfo methodInfo = typeof(DiContainer).GetMethod(nameof(DiContainer.Resolve),
																	  BindingFlags.Instance | BindingFlags.Public,
																	  null,
																	  new Type[] { },
																	  null);
				MethodInfo genericMethod = methodInfo.MakeGenericMethod(objectFactoryType);
				PlaceholderFactory<IPoolObject> factory = (PlaceholderFactory<IPoolObject>)genericMethod.Invoke(_diContainer, null);

				poolObjectsHolderView.Initialize(factory);

				_poolObjectsHolderViews.Add(objectType, poolObjectsHolderView);
			}




			//_poolObjects = new List<PoolObject>();

			//for (int i = 0; i < _starterAmount; i++)
			//{
			//	AddObject(_poolObjectPrefab.name, transform);
			//}
		}

		public PoolObject GetObjectPrefab<T>() where T : IPoolObject
		{
			return _poolObjectPrefabs.Find(x => x.GetType() == typeof(T));
		}

		public IPoolObject GetObject<T>() where T : IPoolObject
		{
			return GetPoolObjectsHolderView<T>().GetObject<T>();
		}

		public PoolObjectProvider GetObjectProvider<T>() where T : IPoolObject
		{
			return new PoolObjectProvider(GetPoolObjectsHolderView<T>().GetObject<T>());
		}

		public List<IPoolObject> GetActiveObjectsOfType<T>() where T : IPoolObject
		{
			return GetPoolObjectsHolderView<T>().ActiveObjects;
		}

		public List<IPoolObject> GetActiveObjects()
		{
			List<IPoolObject> activeObjects = new List<IPoolObject>();

			foreach (KeyValuePair<Type, PoolObjectsHolderView> keyValuePair in _poolObjectsHolderViews)
			{
				activeObjects.AddRange(keyValuePair.Value.ActiveObjects);
			}

			return activeObjects;
		}

		public void ReturnToPoolObjectsOfType<T>() where T : IPoolObject
		{
			GetPoolObjectsHolderView<T>().ReturnToPoolObjects();
		}

		public void ReturnToPoolObjects()
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

		protected PoolObjectsHolderView GetPoolObjectsHolderView<T>()
		{
			return _poolObjectsHolderViews[typeof(T)];
		}
	}
}
