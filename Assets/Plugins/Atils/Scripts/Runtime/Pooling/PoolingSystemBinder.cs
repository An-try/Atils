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

		public static void Bind(DiContainer diContainer, ObjectsPoolView objectsPoolView)
		{
			if (!ValidatePoolObjects(objectsPoolView))
			{
				Debug.LogError("Cannot bind pool system. Fix all errors.");
				return;
			}

			BindPoolObjectFactories(diContainer, objectsPoolView);

			diContainer.Bind<ObjectsPoolView>().FromComponentInNewPrefab(objectsPoolView).AsSingle().NonLazy();
		}

		private static bool ValidatePoolObjects(ObjectsPoolView objectsPoolView)
		{
			bool isSuccess = true;

			isSuccess = objectsPoolView.ValidatePoolObjects();

			for (int i = 0; i < objectsPoolView.PoolObjectPrefabs.Count; i++)
			{
				Type objectType = objectsPoolView.PoolObjectPrefabs[i].GetType();
				Type objectFactoryType = objectType.GetNestedType(FACTORY_CLASS_NAME);

				if (objectFactoryType == null)
				{
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
						Debug.LogError(nameof(PoolingSystemBinder) + ": The nested \"" + FACTORY_CLASS_NAME + "\" class in the \"" + objectType + "\" " +
							"class does not inherit from \"PlaceholderFactory<IPoolObject>\".");
						isSuccess = false;
					}
				}
			}

			return isSuccess;
		}

		private static void BindPoolObjectFactories(DiContainer diContainer, ObjectsPoolView objectsPoolView)
		{
			for (int i = 0; i < objectsPoolView.PoolObjectPrefabs.Count; i++)
			{
				Type objectType = objectsPoolView.PoolObjectPrefabs[i].GetType();
				Type objectFactoryType = objectType.GetNestedType(FACTORY_CLASS_NAME);

				GenericMethodGenerator.GetGenericMethod(typeof(PoolingSystemBinder), nameof(BindFactoryFromComponentInNewPrefab), null)
					.WithBindingFlags(BindingFlags.Static | BindingFlags.NonPublic)
					.WithTypes(typeof(DiContainer), typeof(ObjectsPoolView))
					.WithTypeArguments(objectType, objectFactoryType)
					.WithParameters(diContainer, objectsPoolView)
					.Invoke();
			}
		}

		private static void BindFactoryFromComponentInNewPrefab<TObject, TFactory>(DiContainer diContainer, ObjectsPoolView objectsPoolView)
			where TObject : IPoolObject
			where TFactory : PlaceholderFactory<IPoolObject>
		{
			diContainer.BindFactory<IPoolObject, TFactory>().FromComponentInNewPrefab(objectsPoolView.GetObjectPrefab<TObject>());
		}
	}
}
