using Zenject;

namespace Atils.Runtime.ScreenUtils
{
	public static class ScreenServiceBinder
    {
        public static void Bind(DiContainer diContainer, ScreenServicesConfig screenServicesConfig)
        {
            diContainer.Bind<IScreenService>().FromComponentInNewPrefab(screenServicesConfig.ScreenService).AsSingle().NonLazy();
        }
    }
}
