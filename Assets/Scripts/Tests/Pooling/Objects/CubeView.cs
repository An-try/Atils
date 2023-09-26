using Atils.Runtime.Pooling;
using System;
using Zenject;

public class CubeView : PoolObject
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

	protected override void OnAfterReturnedToPool()
	{ }
}
