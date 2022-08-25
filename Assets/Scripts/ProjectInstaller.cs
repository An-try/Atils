using Atils.Runtime.Pooling;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
	[SerializeField] private ProjectPool _projectPool = default;

	public override void InstallBindings()
	{
		PoolingSystemBinder.Bind<ProjectPool>(Container, _projectPool);
	}
}
