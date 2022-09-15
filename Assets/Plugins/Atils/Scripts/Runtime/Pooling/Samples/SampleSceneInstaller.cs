using UnityEngine;
using Zenject;

namespace Atils.Runtime.Pooling
{
	public class SampleSceneInstaller : MonoInstaller
    {
		[SerializeField] private ScenePool _scenePool = default;

		public override void InstallBindings()
		{
			PoolingSystemBinder.Bind(Container, _scenePool);
		}
	}
}
