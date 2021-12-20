using Atils.Runtime.Pooling;
using Zenject;

public class SphereView : PoolObject
{
	public class Factory : PlaceholderFactory<IPoolObject>
	{ }

	protected override void ResetObject()
	{ }
}
