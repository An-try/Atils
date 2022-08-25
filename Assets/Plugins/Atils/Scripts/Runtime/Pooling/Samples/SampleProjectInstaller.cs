using UnityEngine;
using Zenject;

namespace Atils.Runtime.Pooling
{
	public class SampleProjectInstaller : MonoInstaller
	{
		[SerializeField] private ProjectPool _projectPool = default;

		public override void InstallBindings()
		{
			PoolingSystemBinder.Bind<ProjectPool>(Container, _projectPool);
		}
	}
}
