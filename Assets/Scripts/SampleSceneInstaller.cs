using Atils.Runtime.Inputs;
using Atils.Runtime.Pooling;
using Atils.Runtime.ScreenUtils;
using UnityEngine;
using Zenject;

public class SampleSceneInstaller : MonoInstaller
{
	[SerializeField] private ObjectsPoolView _objectsPoolView = default;

	[SerializeField] private InputServicesConfig _inputServicesConfig = default;
	[SerializeField] private ScreenServicesConfig _screenServicesConfig = default;

	public override void InstallBindings()
	{
		// TODO save-load system

		InputServiceBinder.Bind(Container, _inputServicesConfig);
		ScreenServiceBinder.Bind(Container, _screenServicesConfig);

		PoolingSystemBinder.Bind(Container, _objectsPoolView);
	}
}
