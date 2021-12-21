using Atils.Runtime.Pooling;
using UnityEngine;
using Zenject;

public class SampleSceneInstaller : MonoInstaller
{
	[SerializeField] private ObjectsPoolView _objectsPoolView = default;

	public override void InstallBindings()
	{
		// TODO save-load system

		PoolingSystemBinder.Bind(Container, _objectsPoolView);
	}
}
