using UnityEngine;
using Zenject;

#if UNITY_WEBGL
using Atils.Runtime.WebGL;
#endif

namespace Atils.Runtime.Inputs
{
	public static class InputServiceBinder
	{
		public static void Bind(DiContainer diContainer, InputServicesConfig inputServicesConfig)
		{
			CheckForPlatformAndBind(diContainer, inputServicesConfig);
		}

		private static void CheckForPlatformAndBind(DiContainer diContainer, InputServicesConfig inputServicesConfig)
		{
#if UNITY_WEBGL
			if (PlatformProvider.IsMobile())
			{
				diContainer.Bind<IInputService>().FromComponentInNewPrefab(inputServicesConfig.TouchInputService).AsSingle().NonLazy();
				Debug.Log(typeof(InputServiceBinder) + ": Binding touch input service.");
			}
			else
			{
				diContainer.Bind<IInputService>().FromComponentInNewPrefab(inputServicesConfig.MouseInputService).AsSingle().NonLazy();
				Debug.Log(typeof(InputServiceBinder) + ": Binding mouse input service.");
			}
#elif UNITY_ANDROID || UNITY_IOS
			diContainer.Bind<IInputService>().FromComponentInNewPrefab(inputServicesConfig.TouchInputService).AsSingle().NonLazy();
			Debug.Log(typeof(InputServiceBinder) + ": Binding touch input service.");
#else
			diContainer.Bind<IInputService>().FromComponentInNewPrefab(inputServicesConfig.MouseInputService).AsSingle().NonLazy();
			Debug.Log(typeof(InputServiceBinder) + ": Binding mouse input service.");
#endif
		}
	}
}
