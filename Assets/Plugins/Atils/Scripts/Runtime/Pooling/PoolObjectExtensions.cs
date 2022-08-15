using UnityEngine;

namespace Atils.Runtime.Pooling
{
	public static class PoolObjectExtensions
	{
		public static T SetPosition<T>(this T source, Vector3 position) where T : IPoolObject
		{
			source.Transform.position = position;
			return source;
		}

		public static T SetLocalPosition<T>(this T source, Vector3 localPosition) where T : IPoolObject
		{
			source.Transform.localPosition = localPosition;
			return source;
		}

		public static T SetRotation<T>(this T source, Quaternion rotation) where T : IPoolObject
		{
			source.Transform.rotation = rotation;
			return source;
		}

		public static T SetLocalScale<T>(this T source, Vector3 localScale) where T : IPoolObject
		{
			source.Transform.localScale = localScale;
			return source;
		}

		public static T SetParent<T>(this T source, Transform parent) where T : IPoolObject
		{
			source.Transform.parent = parent;
			return source;
		}
	}
}
