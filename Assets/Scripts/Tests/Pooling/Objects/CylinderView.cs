using Atils.Runtime.Pooling;
using Zenject;

public class CylinderView : PoolObject
{
	public class Factory : PlaceholderFactory<IPoolObject>
	{ }

	protected override void ResetObject()
	{ }
}
