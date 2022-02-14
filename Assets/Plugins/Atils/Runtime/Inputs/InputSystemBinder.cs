using Atils.Runtime.Extensions;
using Atils.Runtime.WebGL;
using UnityEngine;
using Zenject;

namespace Atils.Runtime.Inputs
{
	public static class InputSystemBinder
	{
		public static void Bind(DiContainer diContainer, params InputService[] inputServices)
		{
			CheckForPlatformAndBind(diContainer, inputServices);
		}

		private static void CheckForPlatformAndBind(DiContainer diContainer, params InputService[] inputServices)
		{
#if UNITY_WEBGL
			if (PlatformProvider.IsMobile())
			{
				diContainer.Bind<IInputService>().FromComponentInNewPrefab(inputServices.Find(x => x is TouchInputService)).AsSingle().NonLazy();
				Debug.Log(typeof(InputSystemBinder) + ": Binding touch input service.");
			}
			else
			{
				diContainer.Bind<IInputService>().FromComponentInNewPrefab(inputServices.Find(x => x is MouseInputService)).AsSingle().NonLazy();
				Debug.Log(typeof(InputSystemBinder) + ": Binding mouse input service.");
			}
#elif UNITY_ANDROID || UNITY_IOS
			diContainer.Bind<IInputService>().FromComponentInNewPrefab(inputServices.Find(x => x is MouseInputService)).AsSingle().NonLazy();
			Debug.Log(typeof(InputSystemBinder) + ": Binding mouse input service.");
#else
			diContainer.Bind<IInputService>().FromComponentInNewPrefab(inputServices.Find(x => x is TouchInputService)).AsSingle().NonLazy();
			Debug.Log(typeof(InputSystemBinder) + ": Binding touch input service.");
#endif
		}
	}
}
