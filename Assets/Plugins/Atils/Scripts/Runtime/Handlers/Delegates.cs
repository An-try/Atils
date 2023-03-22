using UnityEngine;

namespace Atils.Runtime.Handlers
{
	public delegate void ColliderEnterHandler(Collision collision);
	public delegate void ColliderStayHandler(Collision collision);
	public delegate void ColliderExitHandler(Collision collision);
	public delegate void TriggerEnterHandler(Collider other);
	public delegate void TriggerStayHandler(Collider other);
	public delegate void TriggerExitHandler(Collider other);
}
