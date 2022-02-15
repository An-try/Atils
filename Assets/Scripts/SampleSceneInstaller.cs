using Atils.Runtime.Inputs;
using Atils.Runtime.Pooling;
using Atils.Runtime.ScreenUtils;
using UnityEngine;
using Zenject;

public class SampleSceneInstaller : MonoInstaller
{
	[SerializeField] private ObjectsPoolView _objectsPoolView = default;
	[SerializeField] private MouseInputService _mouseInputService = default;
	[SerializeField] private TouchInputService _touchInputService = default;
	[SerializeField] private ScreenService _screenService = default;

	public override void InstallBindings()
	{
		// TODO save-load system

		PoolingSystemBinder.Bind(Container, _objectsPoolView);

		//if (JSLibraryProvider.IsMobile())
		{
			//Container.Bind<IInputService>().FromComponentInNewPrefab(_touchInputService).AsSingle().NonLazy();
			//Debug.Log(this + " Binding touch input service.");
		}
		//else
		{
			Container.Bind<IInputService>().FromComponentInNewPrefab(_mouseInputService).AsSingle().NonLazy();
			Debug.Log(this + " Binding mouse input service.");
		}

		Container.Bind<ScreenService>().FromComponentInNewPrefab(_screenService).AsSingle().NonLazy();
	}
}
