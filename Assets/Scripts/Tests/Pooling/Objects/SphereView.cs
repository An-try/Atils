using Atils.Runtime.Pooling;
using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class SphereView : PoolObject
{
	public class Factory : PlaceholderFactory<IPoolObject>
	{ }

	public Rigidbody Rigidbody;
	Vector3 _direction = default;

	public void Initialize()
	{
		ReturnToPool(5);

		float random = 10;

		float x = Random.Range(-random, random);
		float y = Random.Range(-random, random);
		float z = Random.Range(0, random);

		Vector3 randomScatter = new Vector3(x, y, z);
		Vector3 direction = (Vector3.forward + randomScatter).normalized;

		_direction = direction;

		OnInitializedEvent.Invoke(this);
	}

	public override void UpdateObject(float timeStep)
	{
		if (IsPaused)
		{
			return;
		}

		transform.Translate(_direction * 10 * timeStep, Space.World);
	}

	protected override void OnAfterReturnedToPool()
	{
		//Rigidbody.velocity = new Vector3();
	}
}
