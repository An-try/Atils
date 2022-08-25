using Atils.Runtime.Generics;
using System;
using System.Reflection;
using UnityEngine;
using Zenject;

namespace Atils.Runtime.Pooling
{
	public static class PoolingSystemBinder
	{
		private const string FACTORY_CLASS_NAME = "Factory";

		public static void Bind<T>(DiContainer diContainer, Pool pool)
		{
			if (!ValidatePoolObjects(pool))
			{
				Debug.LogError("Cannot bind pool system. Fix all errors.");
				return;
			}

			BindPoolObjectFactories(diContainer, pool);

			diContainer.Bind<T>().FromComponentInNewPrefab(pool).AsSingle().NonLazy();
		}

		private static bool ValidatePoolObjects(Pool pool)
		{
			bool isSuccess = true;

			isSuccess = pool.ValidatePoolObjects();

			for (int i = 0; i < pool.PoolObjectPrefabs.Count; i++)
			{
				Type objectType = pool.PoolObjectPrefabs[i].GetType();
				Type objectFactoryType = objectType.GetNestedType(FACTORY_CLASS_NAME);

				if (objectFactoryType == null)
				{
					/// How to fix this: <see cref="SamplePoolObject.Factory"/>
					
					Debug.LogError(nameof(PoolingSystemBinder) + ": There is no nested \"" + FACTORY_CLASS_NAME + "\" class in the \"" + objectType + "\" class. " +
						"You need to declare the \"" + FACTORY_CLASS_NAME + "\" class inherited from \"PlaceholderFactory<IPoolObject>\". " +
						"Also check the namings.");
					isSuccess = false;
				}

				if (isSuccess)
				{
					object objectFactoryInstance = Activator.CreateInstance(objectFactoryType);

					if (objectFactoryType != null && !(objectFactoryInstance is PlaceholderFactory<IPoolObject>))
					{
						/// How to fix this: <see cref="SamplePoolObject.Factory"/>

						Debug.LogError(nameof(PoolingSystemBinder) + ": The nested \"" + FACTORY_CLASS_NAME + "\" class in the \"" + objectType + "\" " +
							"class does not inherit from \"PlaceholderFactory<IPoolObject>\".");
						isSuccess = false;
					}
				}
			}

			return isSuccess;
		}

		private static void BindPoolObjectFactories(DiContainer diContainer, Pool pool)
		{
			for (int i = 0; i < pool.PoolObjectPrefabs.Count; i++)
			{
				Type objectType = pool.PoolObjectPrefabs[i].GetType();
				Type objectFactoryType = objectType.GetNestedType(FACTORY_CLASS_NAME);

				GenericMethodGenerator.GetGenericMethod(typeof(PoolingSystemBinder), nameof(BindFactoryFromComponentInNewPrefab), null)
					.WithBindingFlags(BindingFlags.Static | BindingFlags.NonPublic)
					.WithTypes(typeof(DiContainer), typeof(Pool))
					.WithTypeArguments(objectType, objectFactoryType)
					.WithParameters(diContainer, pool)
					.Invoke();
			}
		}

		private static void BindFactoryFromComponentInNewPrefab<TObject, TFactory>(DiContainer diContainer, Pool pool)
			where TObject : IPoolObject
			where TFactory : PlaceholderFactory<IPoolObject>
		{
			diContainer.BindFactory<IPoolObject, TFactory>().FromComponentInNewPrefab(pool.GetObjectPrefab<TObject>());
		}
	}
}
