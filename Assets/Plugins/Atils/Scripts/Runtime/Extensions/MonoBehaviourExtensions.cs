using UnityEngine;

namespace Atils.Runtime.Extensions
{
	public static class MonoBehaviourExtensions
	{
		public static void KillCoroutine(this MonoBehaviour monoBehaviour, ref Coroutine coroutine)
		{
			if (coroutine != null)
			{
				monoBehaviour.StopCoroutine(coroutine);
				coroutine = null;
			}
		}
	}
}
