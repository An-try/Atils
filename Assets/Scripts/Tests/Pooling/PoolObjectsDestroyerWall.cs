using Atils.Runtime.Pooling;
using UnityEngine;

public class PoolObjectsDestroyerWall : MonoBehaviour
{
	private void OnTriggerEnter(Collider collider)
	{
		IPoolObject poolObject = collider.GetComponent<IPoolObject>();
		poolObject?.ReturnToPool();
	}
}
