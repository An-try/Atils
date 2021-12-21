using Atils.Runtime.Generics;
using System;
using System.Reflection;
using UnityEngine;
using Zenject;

namespace Atils.Runtime.Pooling
{
	public static class PoolingSystemBinder
    {
		private static string FactoryClassName => "Factory";

		public static void Bind(DiContainer diContainer, ObjectsPoolView objectsPoolView)
		{
			ValidatePoolObjects(objectsPoolView);
			BindPoolObjectFactories(diContainer, objectsPoolView);

			diContainer.Bind<ObjectsPoolView>().FromComponentInNewPrefab(objectsPoolView).AsSingle().NonLazy();
		}

		private static bool ValidatePoolObjects(ObjectsPoolView objectsPoolView)
		{
			bool isSuccess = true;

			for (int i = 0; i < objectsPoolView.PoolObjectPrefabs.Count; i++)
			{
				Type objectType = objectsPoolView.PoolObjectPrefabs[i].GetType();
				Type objectFactoryType = objectType.GetNestedType(FactoryClassName);
				
				if (objectFactoryType == null)
				{
					Debug.LogError(nameof(PoolingSystemBinder) + ": There is no nested \"" + FactoryClassName + "\" class in the \"" + objectType + "\" class. " +
						"You need to declare the \"" + FactoryClassName + "\" class inherited from \"PlaceholderFactory<IPoolObject>\". " +
						"Also check the namings.");
					isSuccess = false;
				}

				object objectFactoryInstance = Activator.CreateInstance(objectFactoryType);

				if (objectFactoryType != null && !(objectFactoryInstance is PlaceholderFactory<IPoolObject>))
				{
					Debug.LogError(nameof(PoolingSystemBinder) + ": The nested \"" + FactoryClassName + "\" class in the \"" + objectType + "\" " +
						"class does not inherit from \"PlaceholderFactory<IPoolObject>\".");
					isSuccess = false;
				}
			}

			return isSuccess;
		}

		private static void BindPoolObjectFactories(DiContainer diContainer, ObjectsPoolView objectsPoolView)
		{
			for (int i = 0; i < objectsPoolView.PoolObjectPrefabs.Count; i++)
			{
				Type objectType = objectsPoolView.PoolObjectPrefabs[i].GetType();
				Type objectFactoryType = objectType.GetNestedType(FactoryClassName);

				GenericMethodGenerator.GetGenericMethod(typeof(PoolingSystemBinder), nameof(BindFactoryFromComponentInNewPrefab), null)
					.WithBindingFlags(BindingFlags.Static | BindingFlags.NonPublic)
					.WithTypes(typeof(DiContainer), typeof(ObjectsPoolView))
					.SetTypeArguments(objectType, objectFactoryType)
					.SetParameters(diContainer, objectsPoolView)
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
