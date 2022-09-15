using Atils.Runtime.Inputs;
using Atils.Runtime.Pooling;
using Atils.Runtime.ScreenUtils;
using UnityEngine;
using Zenject;

namespace Test
{
	public class SampleSceneInstaller : MonoInstaller
	{
		[SerializeField] private InputServicesConfig _inputServicesConfig = default;
		[SerializeField] private ScreenServicesConfig _screenServicesConfig = default;

		[SerializeField] private ScenePool _scenePool = default;

		public override void InstallBindings()
		{
			// TODO save-load system

			InputServiceBinder.Bind(Container, _inputServicesConfig);
			ScreenServiceBinder.Bind(Container, _screenServicesConfig);

			PoolingSystemBinder.Bind(Container, _scenePool);
		}
	}
}
