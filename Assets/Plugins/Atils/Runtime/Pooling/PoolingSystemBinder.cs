using System;
using System.Reflection;
using Zenject;

namespace Atils.Runtime.Pooling
{
	public static class PoolingSystemBinder
    {
		public static void Bind(DiContainer diContainer, ObjectsPoolView objectsPoolView)
		{
			BindPoolObjectFactories(diContainer, objectsPoolView);
		}

		private static void BindPoolObjectFactories(DiContainer diContainer, ObjectsPoolView objectsPoolView)
		{
			for (int i = 0; i < objectsPoolView.PoolObjectPrefabs.Count; i++)
			{
				Type objectType = objectsPoolView.PoolObjectPrefabs[i].GetType();
				Type objectFactoryType = objectType.GetNestedType("Factory");

				MethodInfo methodInfo = typeof(PoolingSystemBinder).GetMethod(nameof(BindFactoryFromComponentInNewPrefab),
																			  BindingFlags.Static | BindingFlags.NonPublic,
																			  null,
																			  new Type[] { typeof(DiContainer), typeof(IPoolView) },
																			  null);
				MethodInfo genericMethod = methodInfo.MakeGenericMethod(objectType, objectFactoryType);
				genericMethod.Invoke(null, new object[] { diContainer, objectsPoolView });
			}
		}

		private static void BindFactoryFromComponentInNewPrefab<TObject, TFactory>(DiContainer diContainer, IPoolView poolView) where TObject : IPoolObject where TFactory : PlaceholderFactory<IPoolObject>
		{
			diContainer.BindFactory<IPoolObject, TFactory>().FromComponentInNewPrefab(poolView.GetObjectPrefab<TObject>());
		}
	}
}
