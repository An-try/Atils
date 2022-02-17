using Atils.Runtime.Pooling;
using Zenject;

public class CubeView : PoolObject
{
	public class Factory : PlaceholderFactory<IPoolObject>
	{ }

	protected override void ResetObject()
	{ }
}
