using UnityEngine;

namespace Atils.Runtime.Extensions
{
	public static class UnityObjectExtensions
	{
		public static void DestroySafely(this Object unityObject)
		{
			if (unityObject == null)
			{
				return;
			}

#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				Object.DestroyImmediate(unityObject);
				return;
			}
#endif

			Object.Destroy(unityObject);
		}
	}
}
