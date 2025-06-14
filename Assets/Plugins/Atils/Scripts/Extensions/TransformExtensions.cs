using System.Collections.Generic;
using UnityEngine;

namespace Atils.Runtime.Extensions
{
	public static class TransformExtensions
	{
		public static void SetX(this Transform source, float value)
		{
			source.position = source.position.GetSetX(value);
		}

		public static void SetY(this Transform source, float value)
		{
			source.position = source.position.GetSetY(value);
		}

		public static void SetZ(this Transform source, float value)
		{
			source.position = source.position.GetSetZ(value);
		}

		public static void SetXLocal(this Transform source, float value)
		{
			source.localPosition = source.localPosition.GetSetX(value);
		}

		public static void SetYLocal(this Transform source, float value)
		{
			source.localPosition = source.localPosition.GetSetY(value);
		}

		public static void SetZLocal(this Transform source, float value)
		{
			source.localPosition = source.localPosition.GetSetZ(value);
		}

		public static void DestroyChildrenSafely(this Transform source)
		{
			Transform[] children = new Transform[source.childCount];

			for (int i = 0; i < source.childCount; i++)
			{
				children[i] = source.GetChild(i);
			}

			foreach (Transform child in children)
			{
				child?.gameObject?.DestroySafely();
			}
		}
	}
}
