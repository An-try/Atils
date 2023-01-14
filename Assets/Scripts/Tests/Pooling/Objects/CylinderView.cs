using Atils.Runtime.Pooling;
using Zenject;

public class CylinderView : PoolObject
{
	public class Factory : PlaceholderFactory<IPoolObject>
	{ }

	public override void UpdateObject(float timeStep)
	{
		base.UpdateObject(timeStep);

		if (IsPaused)
		{
			return;
		}
	}

	protected override void ResetObject()
	{ }
}
